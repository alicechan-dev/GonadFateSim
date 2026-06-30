using UnityEngine;

namespace GonadFateSim.Visualization3D
{
    public static class RuntimeMaterialFactory
    {
        public static Material CreateOpaque(string name, Color color)
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard") ?? Shader.Find("Sprites/Default");
            Material material = new Material(shader);
            material.name = name;
            material.color = color;
            return material;
        }

        public static Material CreateTransparent(string name, Color color, float alpha)
        {
            Material material = CreateOpaque(name, new Color(color.r, color.g, color.b, Mathf.Clamp01(alpha)));
            material.SetFloat("_Surface", 1f);
            material.SetFloat("_Blend", 0f);
            material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetFloat("_ZWrite", 0f);
            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            material.renderQueue = 3000;
            return material;
        }
    }
}
