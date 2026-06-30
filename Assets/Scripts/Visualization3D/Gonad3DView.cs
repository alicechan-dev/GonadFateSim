using GonadFateSim.Simulation;
using UnityEngine;

namespace GonadFateSim.Visualization3D
{
    public sealed class Gonad3DView : MonoBehaviour
    {
        private const string Gonad3DLayerName = "Gonad3D";

        private Gonad3DFactory factory;
        private Transform modelRoot;
        private int gonadLayer = -1;

        private void Awake()
        {
            EnsureRoots();
        }

        public void UpdateFromState(GonadState state)
        {
            EnsureRoots();
            ClearModel();

            GameObject model = state.Fate switch
            {
                GonadFate.TestisLike => factory.CreateTestisLike(modelRoot),
                GonadFate.OvaryLike => factory.CreateOvaryLike(modelRoot),
                GonadFate.Ovotestis => factory.CreateOvotestis(modelRoot),
                GonadFate.Unstable => factory.CreateUnstable(modelRoot),
                _ => factory.CreateUndetermined(modelRoot)
            };

            model.transform.localPosition = Vector3.zero;
            SetLayerRecursively(model, gonadLayer);
        }

        private void EnsureRoots()
        {
            if (gonadLayer < 0)
            {
                gonadLayer = LayerMask.NameToLayer(Gonad3DLayerName);
                if (gonadLayer < 0)
                {
                    gonadLayer = 8;
                }
            }

            if (modelRoot == null)
            {
                GameObject root = new GameObject("Gonad 3D Model Root");
                root.transform.SetParent(transform, false);
                root.layer = gonadLayer;
                modelRoot = root.transform;
            }

            if (factory == null)
            {
                factory = new Gonad3DFactory();
            }
        }

        private void ClearModel()
        {
            for (int index = modelRoot.childCount - 1; index >= 0; index--)
            {
                Destroy(modelRoot.GetChild(index).gameObject);
            }
        }

        private static void SetLayerRecursively(GameObject target, int layer)
        {
            target.layer = layer;
            for (int index = 0; index < target.transform.childCount; index++)
            {
                SetLayerRecursively(target.transform.GetChild(index).gameObject, layer);
            }
        }
    }
}
