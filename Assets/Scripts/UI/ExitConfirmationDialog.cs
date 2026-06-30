using UnityEngine;

namespace GonadFateSim.UI
{
    public sealed class ExitConfirmationDialog : MonoBehaviour
    {
        [SerializeField] private GameObject panelRoot;
        [SerializeField] private AppQuitController quitController;

        public void Bind(GameObject root, AppQuitController controller)
        {
            panelRoot = root;
            quitController = controller;
            Close();
        }

        public void Open()
        {
            if (panelRoot == null)
            {
                return;
            }

            panelRoot.SetActive(true);
            ModalManager.Instance?.RegisterOpen(panelRoot, Close);
        }

        public void Close()
        {
            if (panelRoot != null)
            {
                ModalManager.Instance?.Unregister(panelRoot);
                panelRoot.SetActive(false);
            }
        }

        public void ConfirmExit()
        {
            Close();
            quitController?.QuitNow();
        }
    }
}
