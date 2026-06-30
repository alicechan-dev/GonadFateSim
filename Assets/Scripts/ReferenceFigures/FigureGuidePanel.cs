using UnityEngine;
using UnityEngine.UI;
using GonadFateSim.UI;

namespace GonadFateSim.ReferenceFigures
{
    public sealed class FigureGuidePanel : MonoBehaviour
    {
        [SerializeField] private GameObject panelRoot;
        [SerializeField] private Text bodyLabel;

        private const string GuideText =
            "How to read this figure\n\n" +
            "SOX9 = green: marker associated with Sertoli/testis-like pathway identity in this educational context.\n\n" +
            "FOXL2 = red: marker associated with granulosa/ovary-like pathway identity in this educational context.\n\n" +
            "DAPI = blue: nuclear counterstain that helps show where cells are located.\n\n" +
            "Compare rows and columns by asking which marker dominates, where markers overlap, and whether the pattern changes across conditions.\n\n" +
            "Green-dominant patterns suggest stronger SOX9-associated signal. Red-dominant patterns suggest stronger FOXL2-associated signal. Mixed red/green patterns suggest a mixed or transitional marker pattern.\n\n" +
            "This guide is for reading reference material and simulated marker schematics. It is not medical or laboratory guidance.";

        public void Bind(GameObject root, Text body)
        {
            panelRoot = root;
            bodyLabel = body;
            if (bodyLabel != null)
            {
                bodyLabel.text = GuideText;
            }

            Close();
        }

        public void Open()
        {
            if (bodyLabel != null)
            {
                bodyLabel.text = GuideText;
            }

            if (panelRoot != null)
            {
                panelRoot.SetActive(true);
                ModalManager.Instance?.RegisterOpen(panelRoot, Close);
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
    }
}
