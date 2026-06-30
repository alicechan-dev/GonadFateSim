using UnityEngine;

namespace GonadFateSim.Visualization3D
{
    public sealed class Gonad3DFactory
    {
        private readonly Material neutralMaterial;
        private readonly Material testisMaterial;
        private readonly Material ovaryMaterial;
        private readonly Material mixedMaterial;
        private readonly Material unstableMaterial;
        private readonly Material follicleMaterial;
        private readonly Material labelAnchorMaterial;

        public Gonad3DFactory()
        {
            neutralMaterial = RuntimeMaterialFactory.CreateOpaque("Gonad Neutral", new Color(0.58f, 0.64f, 0.68f));
            testisMaterial = RuntimeMaterialFactory.CreateOpaque("Gonad TestisLike", new Color(0.18f, 0.72f, 0.58f));
            ovaryMaterial = RuntimeMaterialFactory.CreateOpaque("Gonad OvaryLike", new Color(0.9f, 0.45f, 0.62f));
            mixedMaterial = RuntimeMaterialFactory.CreateOpaque("Gonad Mixed", new Color(0.66f, 0.58f, 0.88f));
            unstableMaterial = RuntimeMaterialFactory.CreateOpaque("Gonad Unstable", new Color(0.86f, 0.72f, 0.28f));
            follicleMaterial = RuntimeMaterialFactory.CreateOpaque("Follicle Highlight", new Color(1f, 0.78f, 0.86f));
            labelAnchorMaterial = RuntimeMaterialFactory.CreateTransparent("Gonad Label Anchor", Color.white, 0.18f);
        }

        public GameObject CreateUndetermined(Transform parent)
        {
            GameObject root = CreateRoot("3D Gonad - Undetermined", parent);
            GameObject organ = CreatePrimitive("Neutral oval organ", PrimitiveType.Capsule, root.transform, neutralMaterial);
            organ.transform.localScale = new Vector3(0.75f, 0.38f, 0.38f);
            organ.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            CreateAnchorRing(root.transform, 0.68f);
            return root;
        }

        public GameObject CreateTestisLike(Transform parent)
        {
            GameObject root = CreateRoot("3D Gonad - TestisLike", parent);
            GameObject left = CreatePrimitive("Left smooth lobe", PrimitiveType.Sphere, root.transform, testisMaterial);
            left.transform.localPosition = new Vector3(-0.28f, 0f, 0f);
            left.transform.localScale = new Vector3(0.46f, 0.62f, 0.38f);

            GameObject right = CreatePrimitive("Right smooth lobe", PrimitiveType.Sphere, root.transform, testisMaterial);
            right.transform.localPosition = new Vector3(0.28f, 0f, 0f);
            right.transform.localScale = new Vector3(0.46f, 0.62f, 0.38f);

            GameObject cord = CreatePrimitive("Schematic cord", PrimitiveType.Cylinder, root.transform, testisMaterial);
            cord.transform.localPosition = new Vector3(0f, -0.35f, 0f);
            cord.transform.localScale = new Vector3(0.06f, 0.42f, 0.06f);
            cord.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            return root;
        }

        public GameObject CreateOvaryLike(Transform parent)
        {
            GameObject root = CreateRoot("3D Gonad - OvaryLike", parent);
            GameObject organ = CreatePrimitive("Rounded ovary-like organ", PrimitiveType.Sphere, root.transform, ovaryMaterial);
            organ.transform.localScale = new Vector3(0.78f, 0.56f, 0.44f);

            Vector3[] folliclePositions =
            {
                new Vector3(-0.24f, 0.18f, -0.28f),
                new Vector3(0.2f, 0.22f, -0.25f),
                new Vector3(-0.1f, -0.18f, -0.32f),
                new Vector3(0.28f, -0.06f, -0.26f),
                new Vector3(-0.32f, -0.04f, -0.24f)
            };

            foreach (Vector3 position in folliclePositions)
            {
                GameObject follicle = CreatePrimitive("Follicle-like sphere", PrimitiveType.Sphere, root.transform, follicleMaterial);
                follicle.transform.localPosition = position;
                follicle.transform.localScale = Vector3.one * 0.16f;
            }

            return root;
        }

        public GameObject CreateOvotestis(Transform parent)
        {
            GameObject root = CreateRoot("3D Gonad - Ovotestis", parent);

            GameObject testisSide = CreatePrimitive("Mixed testis-like side", PrimitiveType.Capsule, root.transform, testisMaterial);
            testisSide.transform.localPosition = new Vector3(-0.28f, 0f, 0f);
            testisSide.transform.localScale = new Vector3(0.34f, 0.58f, 0.34f);
            testisSide.transform.localRotation = Quaternion.Euler(0f, 0f, 18f);

            GameObject ovarySide = CreatePrimitive("Mixed ovary-like side", PrimitiveType.Sphere, root.transform, ovaryMaterial);
            ovarySide.transform.localPosition = new Vector3(0.3f, 0f, 0f);
            ovarySide.transform.localScale = new Vector3(0.48f, 0.52f, 0.38f);

            GameObject divider = CreatePrimitive("Mixed state divider", PrimitiveType.Cube, root.transform, mixedMaterial);
            divider.transform.localScale = new Vector3(0.045f, 0.82f, 0.04f);

            GameObject follicle = CreatePrimitive("Mixed follicle-like marker", PrimitiveType.Sphere, root.transform, follicleMaterial);
            follicle.transform.localPosition = new Vector3(0.33f, 0.19f, -0.28f);
            follicle.transform.localScale = Vector3.one * 0.14f;
            return root;
        }

        public GameObject CreateUnstable(Transform parent)
        {
            GameObject root = CreateRoot("3D Gonad - Unstable", parent);
            GameObject main = CreatePrimitive("Neutral irregular organ", PrimitiveType.Sphere, root.transform, unstableMaterial);
            main.transform.localScale = new Vector3(0.66f, 0.44f, 0.38f);

            GameObject offset = CreatePrimitive("Irregular lobe", PrimitiveType.Sphere, root.transform, neutralMaterial);
            offset.transform.localPosition = new Vector3(0.24f, 0.12f, -0.06f);
            offset.transform.localScale = new Vector3(0.32f, 0.26f, 0.26f);

            GameObject marker = CreatePrimitive("Warning marker", PrimitiveType.Cylinder, root.transform, unstableMaterial);
            marker.transform.localPosition = new Vector3(0f, 0.55f, 0f);
            marker.transform.localScale = new Vector3(0.035f, 0.2f, 0.035f);
            return root;
        }

        private static GameObject CreateRoot(string name, Transform parent)
        {
            GameObject root = new GameObject(name);
            root.transform.SetParent(parent, false);
            return root;
        }

        private static GameObject CreatePrimitive(string name, PrimitiveType type, Transform parent, Material material)
        {
            GameObject primitive = GameObject.CreatePrimitive(type);
            primitive.name = name;
            primitive.transform.SetParent(parent, false);
            primitive.GetComponent<Renderer>().material = material;
            return primitive;
        }

        private void CreateAnchorRing(Transform parent, float scale)
        {
            GameObject ring = CreatePrimitive("Neutral state halo", PrimitiveType.Cylinder, parent, labelAnchorMaterial);
            ring.transform.localScale = new Vector3(scale, 0.012f, scale);
        }
    }
}
