using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace GonadFateSim.UI
{
    public sealed class GlobalInputController : MonoBehaviour
    {
        [SerializeField] private AppQuitController quitController;

        public void Bind(AppQuitController appQuitController)
        {
            quitController = appQuitController;
        }

        private void Update()
        {
            if (!WasEscapePressed())
            {
                return;
            }

            if (ModalManager.Instance != null && ModalManager.Instance.CloseTopmost())
            {
                return;
            }

            quitController?.RequestExit();
        }

        private static bool WasEscapePressed()
        {
#if ENABLE_INPUT_SYSTEM
            return Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame;
#elif ENABLE_LEGACY_INPUT_MANAGER
            return Input.GetKeyDown(KeyCode.Escape);
#else
            return false;
#endif
        }
    }
}
