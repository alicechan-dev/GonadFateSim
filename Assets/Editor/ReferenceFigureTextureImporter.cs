#if UNITY_EDITOR
using UnityEditor;

namespace GonadFateSim.Editor
{
    public sealed class ReferenceFigureTextureImporter : AssetPostprocessor
    {
        private const string ReferenceFigurePath = "Assets/Art/ReferenceFigures/Matson2011_Figure1.png";

        private void OnPreprocessTexture()
        {
            if (assetPath != ReferenceFigurePath)
            {
                return;
            }

            TextureImporter textureImporter = (TextureImporter)assetImporter;
            textureImporter.textureType = TextureImporterType.Default;
            textureImporter.mipmapEnabled = false;
            textureImporter.alphaIsTransparency = false;
            textureImporter.sRGBTexture = true;
        }
    }
}
#endif
