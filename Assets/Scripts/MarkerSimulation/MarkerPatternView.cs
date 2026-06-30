using GonadFateSim.Simulation;
using UnityEngine;
using UnityEngine.UI;

namespace GonadFateSim.MarkerSimulation
{
    public sealed class MarkerPatternView : MonoBehaviour
    {
        [SerializeField] private RectTransform markerRoot;
        [SerializeField] private RawImage markerImage;
        [SerializeField] private Text stateLabel;
        [SerializeField] private Text interpretationLabel;

        private readonly MarkerPatternRenderer renderer = new MarkerPatternRenderer();
        private Texture2D currentTexture;

        public void Bind(RectTransform root, Text label, Text interpretation)
        {
            markerRoot = root;
            stateLabel = label;
            interpretationLabel = interpretation;
            EnsureImage();
        }

        public void Render(GonadState state)
        {
            if (markerRoot == null)
            {
                return;
            }

            EnsureImage();
            currentTexture = renderer.Render(state);
            markerImage.texture = currentTexture;
            markerImage.color = Color.white;

            if (stateLabel != null)
            {
                stateLabel.text = "Simulated marker pattern. Educational abstraction, not real microscopy.";
            }

            if (interpretationLabel != null)
            {
                interpretationLabel.text = InterpretationFor(state.Fate);
            }
        }

        private void EnsureImage()
        {
            if (markerRoot == null || markerImage != null)
            {
                return;
            }

            GameObject imageObject = new GameObject("Synthetic Microscopy Texture", typeof(RectTransform), typeof(RawImage));
            imageObject.transform.SetParent(markerRoot, false);
            RectTransform rect = imageObject.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
            markerImage = imageObject.GetComponent<RawImage>();
            markerImage.color = Color.white;
        }

        private static string InterpretationFor(GonadFate fate)
        {
            return fate switch
            {
                GonadFate.TestisLike => "Interpretation: dense DAPI-like nuclei in organized tissue regions; SOX9/FOXL2 channels come later.",
                GonadFate.OvaryLike => "Interpretation: dense DAPI-like nuclei in clustered tissue regions; SOX9/FOXL2 channels come later.",
                GonadFate.Ovotestis => "Interpretation: dense DAPI-like nuclei across mixed tissue regions; marker channels come later.",
                GonadFate.Unstable => "Interpretation: patchier DAPI-like tissue base in this educational model.",
                _ => "Interpretation: low-confidence synthetic nuclei base in this educational schematic."
            };
        }
    }
}
