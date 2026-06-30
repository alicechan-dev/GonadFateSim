using System.IO;
using GonadFateSim.UI;
using UnityEngine;
using UnityEngine.UI;

namespace GonadFateSim.ReferenceFigures
{
    public sealed class ReferenceFigurePanel : MonoBehaviour
    {
        private const string DefaultAssetPath = "Assets/Art/ReferenceFigures/Matson2011_Figure1.png";
        private const string DefaultTitle = "Reference Figure";
        private const string DefaultCaption = "Reference figure from mouse-model literature showing marker patterns used for educational comparison. Displayed as source material, not as simulated output.";
        private const string DefaultCitation = "Matson CK et al. (2011). DMRT1 prevents female reprogramming in the postnatal mammalian testis.";
        private const string DefaultSourceUrl = "https://pmc.ncbi.nlm.nih.gov/articles/PMC3150961/";
        private const string DefaultGuide =
            "How to read this figure:\n" +
            "SOX9 = green; FOXL2 = red; DAPI = blue.\n" +
            "Green-dominant patterns suggest more testis-like marker interpretation. " +
            "Red-dominant patterns suggest more ovary-like marker interpretation. " +
            "Mixed patterns suggest mixed or reprogramming-like marker interpretation.";

        [SerializeField] private GameObject panelRoot;
        [SerializeField] private RawImage figureImage;
        [SerializeField] private AspectRatioFitter aspectRatioFitter;
        [SerializeField] private Text missingImageLabel;
        [SerializeField] private Text titleLabel;
        [SerializeField] private Text captionLabel;
        [SerializeField] private Text citationLabel;
        [SerializeField] private Text sourceLabel;
        [SerializeField] private Text guideLabel;
        [SerializeField] private FigureZoomPanView zoomPanView;
        [SerializeField] private string assetPath = DefaultAssetPath;
        [SerializeField] private string figureTitle = DefaultTitle;
        [TextArea]
        [SerializeField] private string figureCaption = DefaultCaption;
        [TextArea]
        [SerializeField] private string citationText = DefaultCitation;
        [SerializeField] private string sourceUrl = DefaultSourceUrl;
        [TextArea]
        [SerializeField] private string readingGuide = DefaultGuide;

        private Texture2D loadedTexture;

        public void Bind(GameObject root, RawImage image, AspectRatioFitter fitter, Text missingLabel, Text title, Text caption, Text citation, Text source, Text guide, FigureZoomPanView zoomPan)
        {
            panelRoot = root;
            figureImage = image;
            aspectRatioFitter = fitter;
            missingImageLabel = missingLabel;
            titleLabel = title;
            captionLabel = caption;
            citationLabel = citation;
            sourceLabel = source;
            guideLabel = guide;
            zoomPanView = zoomPan;
            RenderMetadata();
            Close();
        }

        public void SetReference(Texture2D texture, string title, string caption, string url)
        {
            loadedTexture = texture;
            figureTitle = title;
            figureCaption = caption;
            sourceUrl = url;
            RenderMetadata();
        }

        public void Open()
        {
            RenderMetadata();
            if (panelRoot != null)
            {
                panelRoot.SetActive(true);
                ModalManager.Instance?.RegisterOpen(panelRoot, Close);
            }

            if (zoomPanView != null)
            {
                zoomPanView.ResetView();
            }
        }

        public void Close()
        {
            if (panelRoot != null)
            {
                ModalManager.Instance?.Unregister(panelRoot);
                panelRoot.SetActive(false);
            }
        }

        public void OpenSource()
        {
            if (!string.IsNullOrWhiteSpace(sourceUrl))
            {
                Application.OpenURL(sourceUrl);
            }
        }

        private void RenderMetadata()
        {
            EnsureReferenceFigureLoaded();

            if (figureImage != null)
            {
                figureImage.texture = loadedTexture;
                figureImage.color = loadedTexture == null ? new Color(0.06f, 0.07f, 0.08f, 1f) : Color.white;
            }

            if (aspectRatioFitter != null && loadedTexture != null && loadedTexture.height > 0)
            {
                aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
                aspectRatioFitter.aspectRatio = (float)loadedTexture.width / loadedTexture.height;
            }

            if (missingImageLabel != null)
            {
                missingImageLabel.text = "Reference figure image not found. Expected path: Assets/Art/ReferenceFigures/Matson2011_Figure1.png";
                missingImageLabel.gameObject.SetActive(loadedTexture == null);
            }

            if (titleLabel != null)
            {
                titleLabel.text = figureTitle;
            }

            if (captionLabel != null)
            {
                captionLabel.text = figureCaption;
            }

            if (citationLabel != null)
            {
                citationLabel.text = citationText;
            }

            if (sourceLabel != null)
            {
                sourceLabel.text = $"Source: {sourceUrl}";
            }

            if (guideLabel != null)
            {
                guideLabel.text = readingGuide;
            }
        }

        private void EnsureReferenceFigureLoaded()
        {
            if (loadedTexture != null)
            {
                return;
            }

            string fullPath = GetFullAssetPath(assetPath);
            if (!File.Exists(fullPath))
            {
                return;
            }

            byte[] pngBytes = File.ReadAllBytes(fullPath);
            Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, false);
            if (texture.LoadImage(pngBytes))
            {
                texture.name = Path.GetFileNameWithoutExtension(fullPath);
                loadedTexture = texture;
            }
        }

        private static string GetFullAssetPath(string relativeAssetPath)
        {
            if (string.IsNullOrWhiteSpace(relativeAssetPath))
            {
                return string.Empty;
            }

            if (Path.IsPathRooted(relativeAssetPath))
            {
                return relativeAssetPath;
            }

            if (relativeAssetPath.StartsWith("Assets/"))
            {
                string projectRoot = Directory.GetParent(Application.dataPath)?.FullName ?? Application.dataPath;
                return Path.Combine(projectRoot, relativeAssetPath);
            }

            return Path.Combine(Application.dataPath, relativeAssetPath);
        }
    }
}
