using GonadFateSim.Simulation;
using UnityEngine;

namespace GonadFateSim.MarkerSimulation
{
    public sealed class MarkerShaderParameterBinder
    {
        public void Bind(Material material, GonadState state, TissueShapeController tissueShape)
        {
            VisualStateMapper mapper = new VisualStateMapper();
            Bind(material, state, mapper.Map(state), tissueShape);
        }

        public void Bind(Material material, GonadState state, VisualStateDescriptor visualState, TissueShapeController tissueShape)
        {
            if (material == null)
            {
                return;
            }

            visualState.Clamp01();
            tissueShape.UpdateForVisualState(visualState);

            material.SetFloat("_Seed", 19.37f);
            material.SetFloat("_TissueMode", (float)state.Fate);
            material.SetFloat("_TestisBias", visualState.TestisBias);
            material.SetFloat("_OvaryBias", visualState.OvaryBias);
            material.SetFloat("_Mixedness", visualState.Mixedness);
            material.SetFloat("_Instability", visualState.Instability);
            material.SetFloat("_DapiIntensity", visualState.DapiDensity);
            material.SetFloat("_NucleiDensity", visualState.DapiDensity);
            material.SetFloat("_Sox9Intensity", visualState.Sox9Signal);
            material.SetFloat("_Foxl2Intensity", visualState.Foxl2Signal);
            material.SetFloat("_Irregularity", Mathf.Lerp(0.34f, 0.86f, visualState.Fragmentation));
            material.SetFloat("_MarkerSpotDensity", Mathf.Lerp(0.48f, 0.98f, 1f - visualState.MarkerPatchiness * 0.35f));
            material.SetFloat("_GapSoftness", visualState.LumenSoftness);

            for (int index = 0; index < tissueShape.Regions.Count; index++)
            {
                material.SetVector($"_Region{index}", tissueShape.Regions[index].ToVector());
            }

            for (int index = 0; index < tissueShape.Gaps.Count; index++)
            {
                material.SetVector($"_Gap{index}", tissueShape.Gaps[index].ToVector());
            }
        }

        private static float Quantize(float value)
        {
            return (int)(Mathf.Clamp01(value) * 100f) / 100f;
        }
    }
}
