using UnityEngine;

namespace GonadFateSim.Visualization3D
{
    public sealed class LabelBillboard : MonoBehaviour
    {
        [SerializeField] private Camera targetCamera;

        public void SetTargetCamera(Camera camera)
        {
            targetCamera = camera;
        }

        private void LateUpdate()
        {
            Camera cameraToFace = targetCamera != null ? targetCamera : Camera.main;
            if (cameraToFace == null)
            {
                return;
            }

            transform.rotation = Quaternion.LookRotation(transform.position - cameraToFace.transform.position);
        }
    }
}
