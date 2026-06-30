using GonadFateSim.Simulation;
using UnityEngine;

namespace GonadFateSim.MarkerSimulation
{
    public sealed class TransitionController
    {
        private readonly VisualStateMapper mapper = new VisualStateMapper();
        private bool initialized;

        public DisplayedVisualState DisplayedVisualState { get; private set; }
        public TargetVisualState TargetVisualState { get; private set; }

        public void SetTarget(GonadState state)
        {
            TargetVisualState = new TargetVisualState(mapper.Map(state));
            if (initialized)
            {
                return;
            }

            DisplayedVisualState = new DisplayedVisualState(TargetVisualState.Descriptor);
            initialized = true;
        }

        public VisualStateDescriptor Tick(float deltaTime)
        {
            if (!initialized)
            {
                return default;
            }

            VisualStateDescriptor displayed = DisplayedVisualState.Descriptor;
            VisualStateDescriptor target = TargetVisualState.Descriptor;
            float fast = BlendFactor(8.5f, deltaTime);
            float medium = BlendFactor(3.2f, deltaTime);
            float slow = BlendFactor(1.25f, deltaTime);

            displayed.Sox9Signal = Mathf.Lerp(displayed.Sox9Signal, target.Sox9Signal, fast);
            displayed.Foxl2Signal = Mathf.Lerp(displayed.Foxl2Signal, target.Foxl2Signal, fast);
            displayed.MarkerPatchiness = Mathf.Lerp(displayed.MarkerPatchiness, target.MarkerPatchiness, fast);
            displayed.DapiDensity = Mathf.Lerp(displayed.DapiDensity, target.DapiDensity, fast);

            displayed.Roundness = Mathf.Lerp(displayed.Roundness, target.Roundness, medium);
            displayed.Elongation = Mathf.Lerp(displayed.Elongation, target.Elongation, medium);
            displayed.LumenSize = Mathf.Lerp(displayed.LumenSize, target.LumenSize, medium);
            displayed.LumenSoftness = Mathf.Lerp(displayed.LumenSoftness, target.LumenSoftness, medium);

            displayed.Connectedness = Mathf.Lerp(displayed.Connectedness, target.Connectedness, slow);
            displayed.Fragmentation = Mathf.Lerp(displayed.Fragmentation, target.Fragmentation, slow);
            displayed.CompartmentSpacing = Mathf.Lerp(displayed.CompartmentSpacing, target.CompartmentSpacing, slow);
            displayed.TestisBias = Mathf.Lerp(displayed.TestisBias, target.TestisBias, slow);
            displayed.OvaryBias = Mathf.Lerp(displayed.OvaryBias, target.OvaryBias, slow);
            displayed.Mixedness = Mathf.Lerp(displayed.Mixedness, target.Mixedness, slow);
            displayed.Instability = Mathf.Lerp(displayed.Instability, target.Instability, slow);

            displayed.Clamp01();
            DisplayedVisualState = new DisplayedVisualState(displayed);
            return displayed;
        }

        private static float BlendFactor(float speed, float deltaTime)
        {
            return 1f - (float)System.Math.Exp(-Mathf.Max(0f, speed) * Mathf.Max(0f, deltaTime));
        }
    }
}
