using GonadFateSim.Simulation;
using UnityEngine;
using UnityEngine.UI;

namespace GonadFateSim.MarkerSimulation
{
    public sealed class MarkerPatternShaderView : MonoBehaviour
    {
        private const string ShaderName = "GonadFateSim/SyntheticMarkerPattern";

        [SerializeField] private RectTransform markerRoot;
        [SerializeField] private RawImage markerImage;
        [SerializeField] private Text stateLabel;
        [SerializeField] private Text interpretationLabel;
        [SerializeField] private MarkerPatternRenderMode renderMode = MarkerPatternRenderMode.Shader;
        [SerializeField] private MarkerChannelViewMode channelViewMode = MarkerChannelViewMode.Merged;

        private readonly MarkerShaderParameterBinder parameterBinder = new MarkerShaderParameterBinder();
        private readonly TissueShapeController tissueShapeController = new TissueShapeController();
        private readonly MarkerPatternRenderer cpuFallbackRenderer = new MarkerPatternRenderer();
        private readonly TransitionController transitionController = new TransitionController();
        private Material shaderMaterial;
        private Texture2D whiteTexture;
        private GonadState latestState;
        private MarkerPatternHoverInspector hoverInspector;
        private bool hasState;

        public void Bind(RectTransform root, Text label, Text interpretation)
        {
            markerRoot = root;
            stateLabel = label;
            interpretationLabel = interpretation;
            EnsureImage();
            EnsureMaterial();
        }

        public void Render(GonadState state)
        {
            if (markerRoot == null)
            {
                return;
            }

            EnsureImage();
            EnsureMaterial();
            latestState = state;
            hasState = true;
            transitionController.SetTarget(state);

            if (renderMode == MarkerPatternRenderMode.Shader && shaderMaterial != null)
            {
                ApplyDisplayedVisualState(0f);
            }
            else
            {
                markerImage.material = null;
                markerImage.texture = cpuFallbackRenderer.Render(state);
                markerImage.color = Color.white;
            }

            if (stateLabel != null)
            {
                stateLabel.text = "Simulated marker pattern. Educational abstraction, not real microscopy.";
            }

            if (interpretationLabel != null)
            {
                interpretationLabel.text = InterpretationFor(state.Fate);
            }
        }

        private void Update()
        {
            if (!hasState || renderMode != MarkerPatternRenderMode.Shader || shaderMaterial == null)
            {
                return;
            }

            ApplyDisplayedVisualState(1f / 60f);
        }

        public void SetChannelViewMode(MarkerChannelViewMode mode)
        {
            channelViewMode = mode;
            if (shaderMaterial != null)
            {
                shaderMaterial.SetFloat("_ChannelMode", (float)channelViewMode);
            }
        }

        private Texture2D WhiteTexture
        {
            get
            {
                if (whiteTexture != null)
                {
                    return whiteTexture;
                }

                whiteTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
                whiteTexture.SetPixel(0, 0, Color.white);
                whiteTexture.Apply();
                return whiteTexture;
            }
        }

        private void EnsureImage()
        {
            if (markerRoot == null || markerImage != null)
            {
                return;
            }

            GameObject imageObject = new GameObject("Shader Synthetic Microscopy", typeof(RectTransform), typeof(RawImage));
            imageObject.transform.SetParent(markerRoot, false);
            RectTransform rect = imageObject.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            markerImage = imageObject.GetComponent<RawImage>();
            markerImage.color = Color.white;
            markerImage.raycastTarget = true;
            hoverInspector = imageObject.AddComponent<MarkerPatternHoverInspector>();
            hoverInspector.Initialize(rect, markerRoot.parent as RectTransform);
        }

        private void EnsureMaterial()
        {
            if (shaderMaterial != null)
            {
                return;
            }

            Shader shader = Shader.Find(ShaderName);
            if (shader != null)
            {
                shaderMaterial = new Material(shader);
            }
        }

        private void ApplyDisplayedVisualState(float deltaTime)
        {
            VisualStateDescriptor displayedState = transitionController.Tick(deltaTime);
            parameterBinder.Bind(shaderMaterial, latestState, displayedState, tissueShapeController);
            shaderMaterial.SetFloat("_ChannelMode", (float)channelViewMode);
            markerImage.material = shaderMaterial;
            markerImage.texture = WhiteTexture;
            markerImage.color = Color.white;
            hoverInspector?.SetVisualState(displayedState);
        }

        private static string InterpretationFor(GonadFate fate)
        {
            return fate switch
            {
                GonadFate.TestisLike => "Interpretation: SOX9-dominant testis-like marker pattern with low FOXL2-like signal.",
                GonadFate.OvaryLike => "Interpretation: shader-rendered DAPI texture with FOXL2-like red channel emphasis.",
                GonadFate.Ovotestis => "Interpretation: shader-rendered mixed SOX9-like and FOXL2-like fluorescence fields.",
                GonadFate.Unstable => "Interpretation: shader-rendered weaker, patchier fluorescence pattern.",
                _ => "Interpretation: shader-rendered low-confidence synthetic marker pattern."
            };
        }
    }
}
