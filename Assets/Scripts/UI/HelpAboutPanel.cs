using UnityEngine;
using UnityEngine.UI;

namespace GonadFateSim.UI
{
    public sealed class HelpAboutPanel : MonoBehaviour
    {
        private const string AboutText =
            "This is an educational Unity simulator for mouse developmental biology.\n\n" +
            "It visualizes simplified relationships between gene/pathway activity, gonadal fate, and marker-pattern interpretation.\n\n" +
            "It is not a medical tool.\n\n" +
            "It is not a laboratory protocol.\n\n" +
            "It does not provide gene editing, CRISPR, animal procedure, surgery, injection, or human-application guidance.\n\n" +
            "Synthetic marker patterns are educational abstractions, not real microscopy.\n\n" +
            "Mouse-model findings must not be treated as direct human clinical guidance.";

        [SerializeField] private GameObject panelRoot;
        [SerializeField] private Text bodyLabel;

        public void Bind(GameObject root, Text body)
        {
            panelRoot = root;
            bodyLabel = body;
            if (bodyLabel != null)
            {
                bodyLabel.text = AboutText;
            }

            Close();
        }

        public void Open()
        {
            if (bodyLabel != null)
            {
                bodyLabel.text = AboutText;
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
