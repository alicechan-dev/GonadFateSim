using UnityEngine;

namespace GonadFateSim.UI
{
    public sealed class AppQuitController : MonoBehaviour
    {
        [SerializeField] private ExitConfirmationDialog exitDialog;

        public void Bind(ExitConfirmationDialog dialog)
        {
            exitDialog = dialog;
        }

        public void RequestExit()
        {
            if (exitDialog != null)
            {
                exitDialog.Open();
            }
        }

        public void QuitNow()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
