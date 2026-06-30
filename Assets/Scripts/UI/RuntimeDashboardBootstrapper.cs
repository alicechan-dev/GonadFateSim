using System;
using GonadFateSim.MarkerSimulation;
using GonadFateSim.ReferenceFigures;
using GonadFateSim.Simulation;
using GonadFateSim.Visualization3D;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GonadFateSim.UI
{
    public sealed class RuntimeDashboardBootstrapper : MonoBehaviour
    {
        private readonly SexDeterminationSimulator simulator = new SexDeterminationSimulator();
        private readonly VisualStateMapper visualStateMapper = new VisualStateMapper();
        private SimulationParameters parameters = SimulationParameters.Default;
        private Text fateLabel;
        private Text explanationLabel;
        private Text visualStateDebugLabel;
        private readonly Slider[] scoreBars = new Slider[4];
        private readonly Text[] scoreLabels = new Text[4];
        private readonly Slider[] parameterSliders = new Slider[6];
        private Gonad3DView gonad3DView;
        private Text gonad3DStateLabel;
        private MarkerPatternShaderView markerPatternView;
        private Image sryNodeImage;
        private Image sox9NodeImage;
        private Image testisNodeImage;
        private Image wntNodeImage;
        private Image ovaryNodeImage;
        private Text pathwayResponseLabel;
        private ReferenceFigurePanel referenceFigurePanel;
        private FigureGuidePanel figureGuidePanel;
        private HelpAboutPanel helpAboutPanel;
        private ExitConfirmationDialog exitConfirmationDialog;
        private AppQuitController appQuitController;
        private GameObject expandedMarkerPanelRoot;
        private MarkerPatternShaderView expandedMarkerPatternView;
        private MarkerChannelViewMode markerChannelViewMode = MarkerChannelViewMode.Merged;
        private static Texture2D markerLegendTexture;

        private static Texture2D MarkerLegendTexture
        {
            get
            {
                if (markerLegendTexture != null)
                {
                    return markerLegendTexture;
                }

                const int size = 24;
                markerLegendTexture = new Texture2D(size, size, TextureFormat.RGBA32, false);
                markerLegendTexture.name = "Marker Legend Circle";
                float center = (size - 1) * 0.5f;
                float radius = center;

                for (int y = 0; y < size; y++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        float dx = (x - center) / radius;
                        float dy = (y - center) / radius;
                        float distance = Mathf.Sqrt(dx * dx + dy * dy);
                        float alpha = Mathf.Clamp01((1f - distance) * 2.3f);
                        markerLegendTexture.SetPixel(x, y, new Color(1f, 1f, 1f, alpha));
                    }
                }

                markerLegendTexture.Apply();
                return markerLegendTexture;
            }
        }

        private void Start()
        {
            BuildDashboard();
            Refresh();
        }

        private void BuildDashboard()
        {
            EnsureEventSystem();

            Canvas canvas = CreateCanvas();
            canvas.gameObject.AddComponent<ModalManager>();
            appQuitController = canvas.gameObject.AddComponent<AppQuitController>();
            GlobalInputController inputController = canvas.gameObject.AddComponent<GlobalInputController>();
            inputController.Bind(appQuitController);

            RectTransform root = CreatePanel("Dashboard", canvas.transform, new Color(0.08f, 0.1f, 0.12f, 0.96f));
            Stretch(root, Vector2.zero, Vector2.one, new Vector2(16f, 16f), new Vector2(-16f, -16f));

            Text title = CreateText("Title", root, "Gonad Fate Simulator", 30, FontStyle.Bold, TextAnchor.MiddleLeft);
            Anchor(title.rectTransform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(20f, -64f), new Vector2(-20f, -16f));

            Text subtitle = CreateText("Subtitle", root, "Educational abstraction. Not a medical or laboratory tool.", 15, FontStyle.Normal, TextAnchor.MiddleLeft);
            subtitle.color = new Color(0.74f, 0.8f, 0.86f);
            Anchor(subtitle.rectTransform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(22f, -92f), new Vector2(-20f, -62f));
            BuildUtilityControls(root);

            RectTransform left = CreatePanel("Parameters", root, new Color(0.12f, 0.15f, 0.18f, 1f));
            Anchor(left, new Vector2(0f, 0.18f), new Vector2(0.25f, 0.86f), new Vector2(20f, 0f), new Vector2(-8f, 0f));

            RectTransform center = CreatePanel("Pathway Graph", root, new Color(0.1f, 0.13f, 0.16f, 1f));
            Anchor(center, new Vector2(0.25f, 0.18f), new Vector2(0.55f, 0.86f), new Vector2(8f, 0f), new Vector2(-8f, 0f));

            RectTransform gonad3D = CreatePanel("3D Gonad View", root, new Color(0.095f, 0.115f, 0.135f, 1f));
            Anchor(gonad3D, new Vector2(0.55f, 0.18f), new Vector2(0.76f, 0.86f), new Vector2(8f, 0f), new Vector2(-8f, 0f));

            RectTransform right = CreatePanel("Output", root, new Color(0.12f, 0.15f, 0.18f, 1f));
            Anchor(right, new Vector2(0.76f, 0.18f), new Vector2(1f, 0.86f), new Vector2(8f, 0f), new Vector2(-20f, 0f));

            RectTransform bottom = CreatePanel("Timeline", root, new Color(0.11f, 0.13f, 0.15f, 1f));
            Anchor(bottom, new Vector2(0f, 0f), new Vector2(1f, 0.18f), new Vector2(20f, 20f), new Vector2(-20f, -12f));

            BuildParameterPanel(left);
            BuildGraphPanel(center);
            BuildGonad3DPanel(gonad3D);
            BuildOutputPanel(right);
            BuildTimeline(bottom);
            BuildReferenceFigurePanels(root);
            BuildExpandedMarkerPatternPanel(root);
            helpAboutPanel = BuildHelpAboutPanel(root);
            exitConfirmationDialog = BuildExitConfirmationDialog(root);
            appQuitController.Bind(exitConfirmationDialog);
        }

        private void BuildUtilityControls(RectTransform root)
        {
            RectTransform utilityPanel = CreatePanel("Utility Controls", root, new Color(0.08f, 0.1f, 0.12f, 0f));
            Anchor(utilityPanel, new Vector2(0.78f, 0.885f), new Vector2(0.985f, 0.97f), Vector2.zero, Vector2.zero);

            Button helpButton = CreateButton(utilityPanel, "Help / About", new Vector2(0f, 0.12f), new Vector2(0.54f, 0.88f));
            helpButton.onClick.AddListener(() => helpAboutPanel?.Open());

            Button exitButton = CreateButton(utilityPanel, "Exit", new Vector2(0.6f, 0.12f), new Vector2(1f, 0.88f));
            exitButton.onClick.AddListener(() => appQuitController?.RequestExit());
        }

        private void BuildParameterPanel(RectTransform parent)
        {
            CreateSectionTitle(parent, "Parameters");
            parameterSliders[0] = CreateParameterSlider(parent, "SRY strength", 0, parameters.SryStrength, value => parameters.SryStrength = value);
            parameterSliders[1] = CreateParameterSlider(parent, "SRY timing", 1, parameters.SryTiming, value => parameters.SryTiming = value);
            parameterSliders[2] = CreateParameterSlider(parent, "SOX9 sensitivity", 2, parameters.Sox9Sensitivity, value => parameters.Sox9Sensitivity = value);
            parameterSliders[3] = CreateParameterSlider(parent, "WNT/beta-catenin", 3, parameters.WntBetaCateninStrength, value => parameters.WntBetaCateninStrength = value);
            parameterSliders[4] = CreateParameterSlider(parent, "Background modifier", 4, parameters.GeneticBackgroundModifier, value => parameters.GeneticBackgroundModifier = value);
            parameterSliders[5] = CreateParameterSlider(parent, "Noise", 5, parameters.Noise, value => parameters.Noise = value);

            CreateScenarioButton(parent, "Normal XY", 0, new SimulationParameters { SryStrength = 0.95f, SryTiming = 0.5f, Sox9Sensitivity = 0.95f, WntBetaCateninStrength = 0.2f, GeneticBackgroundModifier = 0.5f, Noise = 0f });
            CreateScenarioButton(parent, "Sry knockout-like", 1, new SimulationParameters { SryStrength = 0.02f, SryTiming = 0.5f, Sox9Sensitivity = 0.75f, WntBetaCateninStrength = 0.75f, GeneticBackgroundModifier = 0.5f, Noise = 0f });
            CreateScenarioButton(parent, "Delayed Sry", 2, new SimulationParameters { SryStrength = 0.85f, SryTiming = 0.86f, Sox9Sensitivity = 0.75f, WntBetaCateninStrength = 0.55f, GeneticBackgroundModifier = 0.5f, Noise = 0.05f });
            CreateScenarioButton(parent, "High WNT", 3, new SimulationParameters { SryStrength = 0.65f, SryTiming = 0.5f, Sox9Sensitivity = 0.65f, WntBetaCateninStrength = 0.95f, GeneticBackgroundModifier = 0.45f, Noise = 0f });
            CreateScenarioButton(parent, "Mixed", 4, new SimulationParameters { SryStrength = 0.85f, SryTiming = 0.5f, Sox9Sensitivity = 0.75f, WntBetaCateninStrength = 0.82f, GeneticBackgroundModifier = 0.45f, Noise = 0.08f });
            CreateScenarioButton(parent, "Reset", 5, SimulationParameters.Default);
        }

        private void BuildGraphPanel(RectTransform parent)
        {
            CreateSectionTitle(parent, "Pathway Competition");
            sryNodeImage = CreateNode(parent, "SRY", new Vector2(0.16f, 0.62f), new Color(0.35f, 0.72f, 0.9f));
            sox9NodeImage = CreateNode(parent, "SOX9", new Vector2(0.46f, 0.7f), new Color(0.2f, 0.68f, 0.58f));
            testisNodeImage = CreateNode(parent, "Testis-like fate", new Vector2(0.78f, 0.7f), new Color(0.26f, 0.74f, 0.48f));
            wntNodeImage = CreateNode(parent, "WNT/beta-catenin", new Vector2(0.46f, 0.42f), new Color(0.88f, 0.55f, 0.26f));
            ovaryNodeImage = CreateNode(parent, "Ovary-like fate", new Vector2(0.78f, 0.42f), new Color(0.9f, 0.44f, 0.55f));

            CreateTextBlock(parent, "SRY -> SOX9 -> Testis-like bias\nWNT/beta-catenin -> Ovary-like bias\nSOX9 and WNT inhibit each other in this simplified model.", new Vector2(0.08f, 0.12f), new Vector2(0.92f, 0.28f), 15);
            pathwayResponseLabel = CreateTextBlock(parent, "Pathway emphasis: --", new Vector2(0.08f, 0.045f), new Vector2(0.92f, 0.095f), 13);
            pathwayResponseLabel.color = new Color(0.78f, 0.84f, 0.9f);
        }

        private void BuildGonad3DPanel(RectTransform parent)
        {
            CreateSectionTitle(parent, "3D Gonad View");
            Text subtitle = CreateTextBlock(parent, "Stylized mouse-model educational schematic", new Vector2(0.08f, 0.82f), new Vector2(0.92f, 0.88f), 13);
            subtitle.color = new Color(0.74f, 0.8f, 0.86f);

            RawImage viewport = CreateRawImage(parent, new Vector2(0.08f, 0.45f), new Vector2(0.92f, 0.78f));
            RenderTexture renderTexture = new RenderTexture(768, 432, 24);
            renderTexture.name = "Runtime Gonad 3D View";
            viewport.texture = renderTexture;

            gonad3DStateLabel = CreateTextBlock(parent, "Current 3D state: --", new Vector2(0.08f, 0.38f), new Vector2(0.92f, 0.43f), 14);
            gonad3DStateLabel.fontStyle = FontStyle.Bold;

            BuildMarkerPatternPanel(parent);

            Text safety = CreateTextBlock(parent, "Not medical or laboratory guidance.", new Vector2(0.08f, 0.025f), new Vector2(0.92f, 0.06f), 11);
            safety.color = new Color(0.74f, 0.8f, 0.86f);

            GameObject sceneRoot = new GameObject("Runtime Gonad 3D Scene");
            sceneRoot.transform.position = new Vector3(100f, 0f, 0f);
            int gonadLayer = GetGonad3DLayer();
            sceneRoot.layer = gonadLayer;

            GameObject cameraObject = new GameObject("Gonad 3D Camera", typeof(Camera));
            cameraObject.transform.SetParent(sceneRoot.transform, false);
            cameraObject.transform.localPosition = new Vector3(0f, 1.2f, -5f);
            Camera camera = cameraObject.GetComponent<Camera>();
            camera.targetTexture = renderTexture;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.055f, 0.065f, 0.075f, 1f);
            camera.orthographic = true;
            camera.orthographicSize = 1.55f;
            camera.cullingMask = 1 << gonadLayer;
            cameraObject.transform.LookAt(sceneRoot.transform.position);

            GameObject lightObject = new GameObject("Gonad 3D Key Light", typeof(Light));
            lightObject.transform.SetParent(sceneRoot.transform, false);
            lightObject.layer = gonadLayer;
            lightObject.transform.localPosition = new Vector3(1.5f, 2f, -2.5f);
            Light light = lightObject.GetComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 2.1f;
            lightObject.transform.rotation = Quaternion.Euler(38f, -24f, 0f);

            GameObject viewObject = new GameObject("Gonad 3D View", typeof(Gonad3DView));
            viewObject.transform.SetParent(sceneRoot.transform, false);
            viewObject.transform.localPosition = new Vector3(0f, -0.05f, 0f);
            viewObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            viewObject.layer = gonadLayer;
            gonad3DView = viewObject.GetComponent<Gonad3DView>();
        }

        private void BuildMarkerPatternPanel(RectTransform parent)
        {
            Text title = CreateTextBlock(parent, "Simulated Marker Pattern", new Vector2(0.08f, 0.31f), new Vector2(0.5f, 0.36f), 14);
            title.fontStyle = FontStyle.Bold;

            RectTransform markerPanel = CreatePanel("Marker Pattern Panel", parent, new Color(0.055f, 0.065f, 0.075f, 1f));
            Anchor(markerPanel, new Vector2(0.08f, 0.145f), new Vector2(0.92f, 0.29f), Vector2.zero, Vector2.zero);

            CreateMarkerLegend(parent);

            Text markerState = CreateTextBlock(parent, "Pattern state: --", new Vector2(0.08f, 0.095f), new Vector2(0.92f, 0.13f), 10);
            markerState.color = new Color(0.74f, 0.8f, 0.86f);
            Text interpretation = CreateTextBlock(parent, "Interpretation: --", new Vector2(0.08f, 0.06f), new Vector2(0.92f, 0.09f), 10);
            interpretation.color = new Color(0.82f, 0.86f, 0.9f);
            visualStateDebugLabel = CreateTextBlock(parent, "Visual state: --", new Vector2(0.08f, 0.025f), new Vector2(0.92f, 0.055f), 9);
            visualStateDebugLabel.color = new Color(0.58f, 0.68f, 0.76f);

            Button expandButton = CreateButton(parent, "Expand Marker Pattern", new Vector2(0.52f, 0.315f), new Vector2(0.92f, 0.36f));
            expandButton.onClick.AddListener(OpenExpandedMarkerPattern);

            GameObject viewObject = new GameObject("Marker Pattern View", typeof(MarkerPatternShaderView));
            viewObject.transform.SetParent(parent, false);
            markerPatternView = viewObject.GetComponent<MarkerPatternShaderView>();
            markerPatternView.Bind(markerPanel, markerState, interpretation);
            markerPatternView.SetChannelViewMode(markerChannelViewMode);
        }

        private void BuildExpandedMarkerPatternPanel(RectTransform root)
        {
            RectTransform overlay = CreatePanel("Expanded Marker Pattern Modal", root, new Color(0.015f, 0.018f, 0.023f, 0.97f));
            Stretch(overlay, Vector2.zero, Vector2.one, new Vector2(140f, 70f), new Vector2(-140f, -70f));
            expandedMarkerPanelRoot = overlay.gameObject;

            Text title = CreateTextBlock(overlay, "Synthetic Marker Pattern", new Vector2(0.04f, 0.91f), new Vector2(0.76f, 0.97f), 24);
            title.fontStyle = FontStyle.Bold;
            Text subtitle = CreateTextBlock(overlay, "Shader-generated synthetic marker pattern. Educational abstraction, not real microscopy.", new Vector2(0.04f, 0.865f), new Vector2(0.76f, 0.91f), 14);
            subtitle.color = new Color(0.76f, 0.82f, 0.88f);

            RectTransform sidebar = CreatePanel("Marker Pattern Sidebar", overlay, new Color(0.035f, 0.043f, 0.052f, 1f));
            Anchor(sidebar, new Vector2(0.8f, 0.16f), new Vector2(0.96f, 0.91f), Vector2.zero, Vector2.zero);

            RectTransform largePanel = CreatePanel("Expanded Synthetic Micrograph", overlay, new Color(0.025f, 0.03f, 0.04f, 1f));
            Anchor(largePanel, new Vector2(0.04f, 0.16f), new Vector2(0.78f, 0.84f), Vector2.zero, Vector2.zero);

            Button closeButton = CreateButton(sidebar, "Close", new Vector2(0.08f, 0.91f), new Vector2(0.92f, 0.975f));
            closeButton.onClick.AddListener(CloseExpandedMarkerPattern);
            Text channelTitle = CreateTextBlock(sidebar, "Channels", new Vector2(0.08f, 0.82f), new Vector2(0.92f, 0.875f), 15);
            channelTitle.fontStyle = FontStyle.Bold;
            CreateMarkerChannelButtons(sidebar, 0.745f);
            Text legendTitle = CreateTextBlock(sidebar, "Legend", new Vector2(0.08f, 0.48f), new Vector2(0.92f, 0.535f), 15);
            legendTitle.fontStyle = FontStyle.Bold;
            CreateMarkerLegend(sidebar, 0.41f, 0.31f, 0.21f);

            Text markerState = CreateTextBlock(sidebar, "Simulated marker pattern. Educational abstraction, not real microscopy.", new Vector2(0.08f, 0.12f), new Vector2(0.92f, 0.205f), 12);
            markerState.color = new Color(0.78f, 0.84f, 0.9f);
            Text interpretation = CreateTextBlock(sidebar, "Interpretation: --", new Vector2(0.08f, 0.22f), new Vector2(0.92f, 0.43f), 13);
            interpretation.color = new Color(0.86f, 0.9f, 0.95f);
            Text scope = CreateTextBlock(overlay, "Reference figure guides only the broad fluorescence style. This is generated, synthetic, and not a real micrograph.", new Vector2(0.04f, 0.065f), new Vector2(0.96f, 0.125f), 13);
            scope.color = new Color(0.74f, 0.8f, 0.86f);

            GameObject viewObject = new GameObject("Expanded Marker Pattern View", typeof(MarkerPatternShaderView));
            viewObject.transform.SetParent(overlay, false);
            expandedMarkerPatternView = viewObject.GetComponent<MarkerPatternShaderView>();
            expandedMarkerPatternView.Bind(largePanel, markerState, interpretation);
            expandedMarkerPatternView.SetChannelViewMode(markerChannelViewMode);

            expandedMarkerPanelRoot.SetActive(false);
        }

        private HelpAboutPanel BuildHelpAboutPanel(RectTransform root)
        {
            RectTransform overlay = CreatePanel("About Simulator Modal", root, new Color(0.02f, 0.025f, 0.03f, 0.97f));
            Stretch(overlay, Vector2.zero, Vector2.one, new Vector2(220f, 120f), new Vector2(-220f, -120f));

            Text title = CreateTextBlock(overlay, "About This Simulator", new Vector2(0.08f, 0.86f), new Vector2(0.74f, 0.95f), 26);
            title.fontStyle = FontStyle.Bold;

            Button closeButton = CreateButton(overlay, "Close", new Vector2(0.82f, 0.88f), new Vector2(0.94f, 0.94f));
            Text body = CreateTextBlock(overlay, string.Empty, new Vector2(0.08f, 0.14f), new Vector2(0.92f, 0.82f), 17);
            body.color = new Color(0.9f, 0.94f, 0.98f);

            HelpAboutPanel panel = overlay.gameObject.AddComponent<HelpAboutPanel>();
            panel.Bind(overlay.gameObject, body);
            closeButton.onClick.AddListener(panel.Close);
            return panel;
        }

        private ExitConfirmationDialog BuildExitConfirmationDialog(RectTransform root)
        {
            RectTransform overlay = CreatePanel("Exit Confirmation Modal", root, new Color(0.02f, 0.025f, 0.03f, 0.98f));
            Anchor(overlay, new Vector2(0.34f, 0.34f), new Vector2(0.66f, 0.66f), Vector2.zero, Vector2.zero);

            Text title = CreateTextBlock(overlay, "Exit Simulator?", new Vector2(0.08f, 0.68f), new Vector2(0.92f, 0.88f), 24);
            title.fontStyle = FontStyle.Bold;

            Text message = CreateTextBlock(overlay, "Are you sure you want to exit? No simulation data is saved.", new Vector2(0.08f, 0.38f), new Vector2(0.92f, 0.62f), 16);
            message.color = new Color(0.9f, 0.94f, 0.98f);

            Button cancelButton = CreateButton(overlay, "Cancel", new Vector2(0.08f, 0.12f), new Vector2(0.44f, 0.28f));
            Button exitButton = CreateButton(overlay, "Exit", new Vector2(0.56f, 0.12f), new Vector2(0.92f, 0.28f));

            ExitConfirmationDialog dialog = overlay.gameObject.AddComponent<ExitConfirmationDialog>();
            dialog.Bind(overlay.gameObject, appQuitController);
            cancelButton.onClick.AddListener(dialog.Close);
            exitButton.onClick.AddListener(dialog.ConfirmExit);
            return dialog;
        }

        private void OpenExpandedMarkerPattern()
        {
            if (expandedMarkerPanelRoot == null)
            {
                return;
            }

            expandedMarkerPanelRoot.SetActive(true);
            ModalManager.Instance?.RegisterOpen(expandedMarkerPanelRoot, CloseExpandedMarkerPattern);
        }

        private void CloseExpandedMarkerPattern()
        {
            if (expandedMarkerPanelRoot == null)
            {
                return;
            }

            ModalManager.Instance?.Unregister(expandedMarkerPanelRoot);
            expandedMarkerPanelRoot.SetActive(false);
        }

        private void CreateMarkerChannelButtons(RectTransform parent, float y)
        {
            CreateMarkerChannelButton(parent, "Merged", MarkerChannelViewMode.Merged, new Vector2(0.08f, y), new Vector2(0.92f, y + 0.045f));
            CreateMarkerChannelButton(parent, "SOX9", MarkerChannelViewMode.Sox9, new Vector2(0.08f, y - 0.055f), new Vector2(0.92f, y - 0.01f));
            CreateMarkerChannelButton(parent, "FOXL2", MarkerChannelViewMode.Foxl2, new Vector2(0.08f, y - 0.11f), new Vector2(0.92f, y - 0.065f));
            CreateMarkerChannelButton(parent, "DAPI", MarkerChannelViewMode.Dapi, new Vector2(0.08f, y - 0.165f), new Vector2(0.92f, y - 0.12f));
        }

        private void CreateMarkerChannelButton(RectTransform parent, string label, MarkerChannelViewMode mode, Vector2 min, Vector2 max)
        {
            Button button = CreateButton(parent, label, min, max);
            button.onClick.AddListener(() =>
            {
                markerChannelViewMode = mode;
                markerPatternView?.SetChannelViewMode(markerChannelViewMode);
                expandedMarkerPatternView?.SetChannelViewMode(markerChannelViewMode);
                Refresh();
            });
        }

        private void CreateMarkerLegend(RectTransform parent)
        {
            CreateLegendItem(parent, new Vector2(0.08f, 0.295f), new Color(0.1f, 0.95f, 0.35f, 1f), "SOX9-like");
            CreateLegendItem(parent, new Vector2(0.39f, 0.295f), new Color(1f, 0.18f, 0.25f, 1f), "FOXL2-like");
            CreateLegendItem(parent, new Vector2(0.7f, 0.295f), new Color(0.22f, 0.38f, 1f, 1f), "DAPI/nuclei-like");
        }

        private void CreateMarkerLegend(RectTransform parent, float sox9Y, float foxl2Y, float dapiY)
        {
            CreateLegendItem(parent, new Vector2(0.1f, sox9Y), new Color(0.1f, 0.95f, 0.35f, 1f), "SOX9-like");
            CreateLegendItem(parent, new Vector2(0.1f, foxl2Y), new Color(1f, 0.18f, 0.25f, 1f), "FOXL2-like");
            CreateLegendItem(parent, new Vector2(0.1f, dapiY), new Color(0.22f, 0.38f, 1f, 1f), "DAPI/nuclei-like");
        }

        private void BuildOutputPanel(RectTransform parent)
        {
            CreateSectionTitle(parent, "Output");
            fateLabel = CreateTextBlock(parent, string.Empty, new Vector2(0.08f, 0.78f), new Vector2(0.92f, 0.9f), 22);
            fateLabel.fontStyle = FontStyle.Bold;
            explanationLabel = CreateTextBlock(parent, string.Empty, new Vector2(0.08f, 0.56f), new Vector2(0.92f, 0.76f), 16);

            string[] labels = { "Testis", "Ovary", "Ovotestis", "Instability" };
            for (int i = 0; i < labels.Length; i++)
            {
                float top = 0.48f - i * 0.095f;
                CreateTextBlock(parent, labels[i], new Vector2(0.08f, top), new Vector2(0.42f, top + 0.052f), 15);
                scoreBars[i] = CreateReadOnlyBar(parent, new Vector2(0.44f, top), new Vector2(0.82f, top + 0.052f));
                scoreLabels[i] = CreateTextBlock(parent, "0.00", new Vector2(0.84f, top), new Vector2(0.95f, top + 0.052f), 14);
            }

            Button referenceButton = CreateButton(parent, "View Reference Figure", new Vector2(0.08f, 0.085f), new Vector2(0.92f, 0.14f));
            referenceButton.onClick.AddListener(() => referenceFigurePanel?.Open());

            Button guideButton = CreateButton(parent, "How to Read This Figure", new Vector2(0.08f, 0.02f), new Vector2(0.92f, 0.075f));
            guideButton.onClick.AddListener(() => figureGuidePanel?.Open());
        }

        private void BuildReferenceFigurePanels(RectTransform root)
        {
            referenceFigurePanel = BuildReferenceFigurePanel(root);
            figureGuidePanel = BuildFigureGuidePanel(root);
        }

        private ReferenceFigurePanel BuildReferenceFigurePanel(RectTransform root)
        {
            RectTransform overlay = CreatePanel("Reference Figure Modal", root, new Color(0.02f, 0.025f, 0.03f, 0.96f));
            Stretch(overlay, Vector2.zero, Vector2.one, new Vector2(120f, 80f), new Vector2(-120f, -80f));

            Text title = CreateTextBlock(overlay, "Reference Figure", new Vector2(0.04f, 0.9f), new Vector2(0.74f, 0.965f), 24);
            title.fontStyle = FontStyle.Bold;

            Button closeButton = CreateButton(overlay, "Close", new Vector2(0.84f, 0.905f), new Vector2(0.96f, 0.96f));

            RectTransform figureFrame = CreatePanel("Reference Figure Frame", overlay, new Color(0.04f, 0.045f, 0.05f, 1f));
            Anchor(figureFrame, new Vector2(0.04f, 0.13f), new Vector2(0.66f, 0.87f), Vector2.zero, Vector2.zero);

            GameObject figureObject = new GameObject("Reference Figure Image", typeof(RectTransform), typeof(RawImage), typeof(AspectRatioFitter), typeof(FigureZoomPanView));
            figureObject.transform.SetParent(figureFrame, false);
            RectTransform figureRect = figureObject.GetComponent<RectTransform>();
            Stretch(figureRect, Vector2.zero, Vector2.one, new Vector2(10f, 10f), new Vector2(-10f, -10f));
            RawImage figureImage = figureObject.GetComponent<RawImage>();
            figureImage.color = Color.white;
            AspectRatioFitter aspectRatioFitter = figureObject.GetComponent<AspectRatioFitter>();
            aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
            FigureZoomPanView zoomPan = figureObject.GetComponent<FigureZoomPanView>();
            zoomPan.Bind(figureRect);

            Text missingImage = CreateTextBlock(figureFrame, "Reference figure image not found. Expected path: Assets/Art/ReferenceFigures/Matson2011_Figure1.png", new Vector2(0.12f, 0.42f), new Vector2(0.88f, 0.58f), 16);
            missingImage.alignment = TextAnchor.MiddleCenter;
            missingImage.color = new Color(0.8f, 0.84f, 0.88f);

            Text caption = CreateTextBlock(overlay, string.Empty, new Vector2(0.7f, 0.69f), new Vector2(0.96f, 0.86f), 14);
            Text citation = CreateTextBlock(overlay, string.Empty, new Vector2(0.7f, 0.55f), new Vector2(0.96f, 0.67f), 13);
            Text source = CreateTextBlock(overlay, string.Empty, new Vector2(0.7f, 0.46f), new Vector2(0.96f, 0.53f), 12);
            Text guide = CreateTextBlock(overlay, string.Empty, new Vector2(0.7f, 0.2f), new Vector2(0.96f, 0.43f), 13);
            Text scope = CreateTextBlock(overlay, "Real paper figure = reference material. Simulated marker pattern = educational abstraction. Not medical or laboratory guidance.", new Vector2(0.04f, 0.045f), new Vector2(0.96f, 0.105f), 13);
            scope.color = new Color(0.74f, 0.8f, 0.86f);

            Button sourceButton = CreateButton(overlay, "Open Source Link", new Vector2(0.7f, 0.125f), new Vector2(0.82f, 0.18f));
            Button guideButton = CreateButton(overlay, "How to Read This Figure", new Vector2(0.835f, 0.125f), new Vector2(0.96f, 0.18f));

            ReferenceFigurePanel panel = overlay.gameObject.AddComponent<ReferenceFigurePanel>();
            panel.Bind(overlay.gameObject, figureImage, aspectRatioFitter, missingImage, title, caption, citation, source, guide, zoomPan);
            closeButton.onClick.AddListener(panel.Close);
            sourceButton.onClick.AddListener(panel.OpenSource);
            guideButton.onClick.AddListener(() => figureGuidePanel?.Open());
            return panel;
        }

        private FigureGuidePanel BuildFigureGuidePanel(RectTransform root)
        {
            RectTransform overlay = CreatePanel("Figure Reading Guide Modal", root, new Color(0.02f, 0.025f, 0.03f, 0.96f));
            Stretch(overlay, Vector2.zero, Vector2.one, new Vector2(180f, 100f), new Vector2(-180f, -100f));

            Text title = CreateTextBlock(overlay, "How to Read This Figure", new Vector2(0.06f, 0.86f), new Vector2(0.74f, 0.95f), 24);
            title.fontStyle = FontStyle.Bold;

            Button closeButton = CreateButton(overlay, "Close", new Vector2(0.82f, 0.88f), new Vector2(0.94f, 0.94f));
            Text body = CreateTextBlock(overlay, string.Empty, new Vector2(0.08f, 0.1f), new Vector2(0.92f, 0.82f), 16);
            FigureGuidePanel panel = overlay.gameObject.AddComponent<FigureGuidePanel>();
            panel.Bind(overlay.gameObject, body);
            closeButton.onClick.AddListener(panel.Close);
            return panel;
        }

        private void BuildTimeline(RectTransform parent)
        {
            CreateSectionTitle(parent, "Timeline");
            CreateTextBlock(parent, "Early gonad  ->  SRY window  ->  SOX9 stabilization  ->  Pathway competition  ->  Fate output", new Vector2(0.04f, 0.24f), new Vector2(0.96f, 0.68f), 20);
        }

        private Slider CreateParameterSlider(RectTransform parent, string label, int index, float value, Action<float> setter)
        {
            float top = 0.82f - index * 0.105f;
            CreateTextBlock(parent, label, new Vector2(0.08f, top), new Vector2(0.88f, top + 0.045f), 14);
            Slider slider = CreateSlider(parent, new Vector2(0.08f, top - 0.055f), new Vector2(0.88f, top - 0.01f));
            slider.SetValueWithoutNotify(Mathf.Clamp01(value));
            slider.onValueChanged.AddListener(newValue =>
            {
                setter(Mathf.Clamp01(newValue));
                Refresh();
            });
            return slider;
        }

        private void CreateScenarioButton(RectTransform parent, string label, int index, SimulationParameters scenario)
        {
            int column = index % 2;
            int row = index / 2;
            float xMin = column == 0 ? 0.08f : 0.5f;
            float xMax = column == 0 ? 0.46f : 0.88f;
            float yMax = 0.2f - row * 0.07f;
            Button button = CreateButton(parent, label, new Vector2(xMin, yMax - 0.052f), new Vector2(xMax, yMax));
            button.onClick.AddListener(() =>
            {
                parameters = scenario.Normalized();
                SyncParameterSliders();
                Refresh();
            });
        }

        private void Refresh()
        {
            GonadState state = simulator.Simulate(parameters, 42);
            VisualStateDescriptor visualState = visualStateMapper.Map(state);
            fateLabel.text = $"Predicted outcome: {FormatFate(state.Fate)}";
            explanationLabel.text = state.Explanation;
            SetScore(0, state.TestisScore);
            SetScore(1, state.OvaryScore);
            SetScore(2, state.OvotestisScore);
            SetScore(3, state.InstabilityScore);
            gonad3DView?.UpdateFromState(state);
            markerPatternView?.Render(state);
            expandedMarkerPatternView?.Render(state);
            UpdatePathwayGraph(state.Fate);
            if (gonad3DStateLabel != null)
            {
                gonad3DStateLabel.text = $"Current 3D state: {FormatFate(state.Fate)}";
            }
            if (visualStateDebugLabel != null)
            {
                visualStateDebugLabel.text = $"Visual state: T {visualState.TestisBias:0.00} / O {visualState.OvaryBias:0.00} / M {visualState.Mixedness:0.00} / I {visualState.Instability:0.00}";
            }
        }

        private void UpdatePathwayGraph(GonadFate fate)
        {
            float testisEmphasis = fate switch
            {
                GonadFate.TestisLike => 1f,
                GonadFate.Ovotestis => 0.72f,
                GonadFate.Unstable => 0.34f,
                _ => 0.42f
            };

            float ovaryEmphasis = fate switch
            {
                GonadFate.OvaryLike => 1f,
                GonadFate.Ovotestis => 0.72f,
                GonadFate.Unstable => 0.34f,
                _ => 0.42f
            };

            ApplyNodeColor(sryNodeImage, new Color(0.35f, 0.72f, 0.9f), testisEmphasis);
            ApplyNodeColor(sox9NodeImage, new Color(0.2f, 0.68f, 0.58f), testisEmphasis);
            ApplyNodeColor(testisNodeImage, new Color(0.26f, 0.74f, 0.48f), testisEmphasis);
            ApplyNodeColor(wntNodeImage, new Color(0.88f, 0.55f, 0.26f), ovaryEmphasis);
            ApplyNodeColor(ovaryNodeImage, new Color(0.9f, 0.44f, 0.55f), ovaryEmphasis);

            if (pathwayResponseLabel != null)
            {
                pathwayResponseLabel.text = fate switch
                {
                    GonadFate.TestisLike => "Pathway emphasis: SRY -> SOX9 -> Testis-like fate",
                    GonadFate.OvaryLike => "Pathway emphasis: WNT/beta-catenin -> Ovary-like fate",
                    GonadFate.Ovotestis => "Pathway emphasis: mixed activity in both simplified pathways",
                    GonadFate.Unstable => "Pathway emphasis: uncertain, low-confidence pathway competition",
                    _ => "Pathway emphasis: undetermined early-state balance"
                };
            }
        }

        private void SyncParameterSliders()
        {
            parameterSliders[0]?.SetValueWithoutNotify(parameters.SryStrength);
            parameterSliders[1]?.SetValueWithoutNotify(parameters.SryTiming);
            parameterSliders[2]?.SetValueWithoutNotify(parameters.Sox9Sensitivity);
            parameterSliders[3]?.SetValueWithoutNotify(parameters.WntBetaCateninStrength);
            parameterSliders[4]?.SetValueWithoutNotify(parameters.GeneticBackgroundModifier);
            parameterSliders[5]?.SetValueWithoutNotify(parameters.Noise);
        }

        private void SetScore(int index, float value)
        {
            float clampedValue = Mathf.Clamp01(value);
            scoreBars[index].SetValueWithoutNotify(clampedValue);
            scoreLabels[index].text = clampedValue.ToString("0.00");
        }

        private static Canvas CreateCanvas()
        {
            GameObject canvasObject = new GameObject("Runtime Dashboard Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1600f, 900f);
            scaler.matchWidthOrHeight = 0.5f;
            return canvas;
        }

        private static void EnsureEventSystem()
        {
            if (FindObjectOfType<EventSystem>() != null)
            {
                return;
            }

            GameObject eventSystem = new GameObject("EventSystem", typeof(EventSystem));
            Type inputSystemModule = Type.GetType("UnityEngine.InputSystem.UI.InputSystemUIInputModule, Unity.InputSystem");
            if (inputSystemModule != null)
            {
                eventSystem.AddComponent(inputSystemModule);
            }
            else
            {
                eventSystem.AddComponent<StandaloneInputModule>();
            }
        }

        private static RectTransform CreatePanel(string name, Transform parent, Color color)
        {
            GameObject gameObject = new GameObject(name, typeof(RectTransform), typeof(Image));
            gameObject.transform.SetParent(parent, false);
            Image image = gameObject.GetComponent<Image>();
            image.color = color;
            return gameObject.GetComponent<RectTransform>();
        }

        private static void CreateSectionTitle(RectTransform parent, string text)
        {
            Text title = CreateText("Section Title", parent, text, 20, FontStyle.Bold, TextAnchor.MiddleLeft);
            Anchor(title.rectTransform, new Vector2(0.06f, 0.9f), new Vector2(0.94f, 0.98f), Vector2.zero, Vector2.zero);
        }

        private static Image CreateNode(RectTransform parent, string label, Vector2 center, Color color)
        {
            RectTransform node = CreatePanel(label, parent, color);
            Anchor(node, center, center, new Vector2(-82f, -32f), new Vector2(82f, 32f));
            Text text = CreateText("Label", node, label, 16, FontStyle.Bold, TextAnchor.MiddleCenter);
            Stretch(text.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            return node.GetComponent<Image>();
        }

        private static void CreateLegendItem(RectTransform parent, Vector2 anchor, Color color, string label)
        {
            GameObject swatchObject = new GameObject("Marker Legend Swatch", typeof(RectTransform), typeof(RawImage));
            swatchObject.transform.SetParent(parent, false);
            RectTransform swatchRect = swatchObject.GetComponent<RectTransform>();
            Anchor(swatchRect, anchor, anchor, new Vector2(0f, -4f), new Vector2(14f, 10f));
            RawImage swatch = swatchObject.GetComponent<RawImage>();
            swatch.texture = MarkerLegendTexture;
            swatch.color = color;

            Text text = CreateTextBlock(parent, label, new Vector2(anchor.x + 0.04f, anchor.y - 0.014f), new Vector2(anchor.x + 0.25f, anchor.y + 0.018f), 9);
            text.color = new Color(0.78f, 0.84f, 0.9f);
        }

        private static void ApplyNodeColor(Image image, Color baseColor, float emphasis)
        {
            if (image == null)
            {
                return;
            }

            float clamped = Mathf.Clamp01(emphasis);
            image.color = new Color(
                Mathf.Lerp(0.12f, baseColor.r, clamped),
                Mathf.Lerp(0.13f, baseColor.g, clamped),
                Mathf.Lerp(0.15f, baseColor.b, clamped),
                1f);
        }

        private static Text CreateTextBlock(RectTransform parent, string text, Vector2 min, Vector2 max, int size)
        {
            Text label = CreateText("Text", parent, text, size, FontStyle.Normal, TextAnchor.UpperLeft);
            label.horizontalOverflow = HorizontalWrapMode.Wrap;
            label.verticalOverflow = VerticalWrapMode.Overflow;
            Anchor(label.rectTransform, min, max, Vector2.zero, Vector2.zero);
            return label;
        }

        private static Text CreateText(string name, Transform parent, string text, int size, FontStyle style, TextAnchor anchor)
        {
            GameObject gameObject = new GameObject(name, typeof(RectTransform), typeof(Text));
            gameObject.transform.SetParent(parent, false);
            Text label = gameObject.GetComponent<Text>();
            label.text = text;
            label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            label.fontSize = size;
            label.fontStyle = style;
            label.alignment = anchor;
            label.color = Color.white;
            return label;
        }

        private static Slider CreateReadOnlyBar(RectTransform parent, Vector2 min, Vector2 max)
        {
            Slider slider = CreateSlider(parent, min, max);
            slider.interactable = false;
            return slider;
        }

        private static Slider CreateSlider(RectTransform parent, Vector2 min, Vector2 max)
        {
            GameObject sliderObject = new GameObject("Slider", typeof(RectTransform), typeof(Slider), typeof(Image));
            sliderObject.transform.SetParent(parent, false);
            Anchor(sliderObject.GetComponent<RectTransform>(), min, max, Vector2.zero, Vector2.zero);
            Image background = sliderObject.GetComponent<Image>();
            background.color = new Color(0.22f, 0.25f, 0.28f);

            GameObject fillObject = new GameObject("Fill", typeof(RectTransform), typeof(Image));
            fillObject.transform.SetParent(sliderObject.transform, false);
            Image fill = fillObject.GetComponent<Image>();
            fill.color = new Color(0.28f, 0.68f, 0.78f);
            Stretch(fill.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);

            Slider slider = sliderObject.GetComponent<Slider>();
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.fillRect = fill.rectTransform;
            slider.targetGraphic = background;
            return slider;
        }

        private static RawImage CreateRawImage(RectTransform parent, Vector2 min, Vector2 max)
        {
            GameObject imageObject = new GameObject("Gonad 3D Render View", typeof(RectTransform), typeof(RawImage));
            imageObject.transform.SetParent(parent, false);
            Anchor(imageObject.GetComponent<RectTransform>(), min, max, Vector2.zero, Vector2.zero);
            RawImage image = imageObject.GetComponent<RawImage>();
            image.color = Color.white;
            return image;
        }

        private static int GetGonad3DLayer()
        {
            int layer = LayerMask.NameToLayer("Gonad3D");
            return layer >= 0 ? layer : 8;
        }

        private static Button CreateButton(RectTransform parent, string label, Vector2 min, Vector2 max)
        {
            RectTransform rectTransform = CreatePanel(label, parent, new Color(0.2f, 0.3f, 0.38f));
            Anchor(rectTransform, min, max, Vector2.zero, Vector2.zero);
            Button button = rectTransform.gameObject.AddComponent<Button>();
            Text text = CreateText("Label", rectTransform, label, 13, FontStyle.Bold, TextAnchor.MiddleCenter);
            Stretch(text.rectTransform, Vector2.zero, Vector2.one, Vector2.zero, Vector2.zero);
            return button;
        }

        private static void Anchor(RectTransform transform, Vector2 min, Vector2 max, Vector2 offsetMin, Vector2 offsetMax)
        {
            transform.anchorMin = min;
            transform.anchorMax = max;
            transform.offsetMin = offsetMin;
            transform.offsetMax = offsetMax;
        }

        private static void Stretch(RectTransform transform, Vector2 min, Vector2 max, Vector2 offsetMin, Vector2 offsetMax)
        {
            Anchor(transform, min, max, offsetMin, offsetMax);
        }

        private static string FormatFate(GonadFate fate)
        {
            return fate switch
            {
                GonadFate.Undetermined => "Undetermined",
                GonadFate.TestisLike => "Testis-like",
                GonadFate.OvaryLike => "Ovary-like",
                GonadFate.Ovotestis => "Ovotestis",
                _ => "Unstable"
            };
        }
    }
}
