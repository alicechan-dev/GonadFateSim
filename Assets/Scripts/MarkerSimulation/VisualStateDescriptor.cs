using System;
using UnityEngine;

namespace GonadFateSim.MarkerSimulation
{
    [Serializable]
    public struct VisualStateDescriptor
    {
        public float TestisBias;
        public float OvaryBias;
        public float Mixedness;
        public float Instability;
        public float Sox9Signal;
        public float Foxl2Signal;
        public float DapiDensity;
        public float Elongation;
        public float Roundness;
        public float Connectedness;
        public float Fragmentation;
        public float LumenSize;
        public float LumenSoftness;
        public float CompartmentSpacing;
        public float MarkerPatchiness;

        public void Clamp01()
        {
            TestisBias = Mathf.Clamp01(TestisBias);
            OvaryBias = Mathf.Clamp01(OvaryBias);
            Mixedness = Mathf.Clamp01(Mixedness);
            Instability = Mathf.Clamp01(Instability);
            Sox9Signal = Mathf.Clamp01(Sox9Signal);
            Foxl2Signal = Mathf.Clamp01(Foxl2Signal);
            DapiDensity = Mathf.Clamp01(DapiDensity);
            Elongation = Mathf.Clamp01(Elongation);
            Roundness = Mathf.Clamp01(Roundness);
            Connectedness = Mathf.Clamp01(Connectedness);
            Fragmentation = Mathf.Clamp01(Fragmentation);
            LumenSize = Mathf.Clamp01(LumenSize);
            LumenSoftness = Mathf.Clamp01(LumenSoftness);
            CompartmentSpacing = Mathf.Clamp01(CompartmentSpacing);
            MarkerPatchiness = Mathf.Clamp01(MarkerPatchiness);
        }

        public readonly VisualStateDescriptor Clamped()
        {
            VisualStateDescriptor descriptor = this;
            descriptor.Clamp01();
            return descriptor;
        }

        public override readonly string ToString()
        {
            return string.Format(
                "VisualState(TestisBias={0:0.00}, OvaryBias={1:0.00}, Mixedness={2:0.00}, Instability={3:0.00}, Sox9={4:0.00}, Foxl2={5:0.00}, Dapi={6:0.00}, Elongation={7:0.00}, Roundness={8:0.00}, Connectedness={9:0.00}, Fragmentation={10:0.00}, LumenSize={11:0.00}, LumenSoftness={12:0.00}, Spacing={13:0.00}, Patchiness={14:0.00})",
                TestisBias,
                OvaryBias,
                Mixedness,
                Instability,
                Sox9Signal,
                Foxl2Signal,
                DapiDensity,
                Elongation,
                Roundness,
                Connectedness,
                Fragmentation,
                LumenSize,
                LumenSoftness,
                CompartmentSpacing,
                MarkerPatchiness);
        }
    }
}
