using System;
using UnityEngine;

namespace GonadFateSim.MarkerSimulation
{
    public enum HoverZoneCategory
    {
        None = 0,
        Sox9Dominant = 1,
        Foxl2Dominant = 2,
        Mixed = 3,
        NucleiRichLowMarker = 4
    }

    public readonly struct HoverZoneInfo
    {
        public readonly HoverZoneCategory Category;
        public readonly string Name;
        public readonly string Interpretation;
        public readonly float Sox9Like;
        public readonly float Foxl2Like;
        public readonly float DapiLike;
        public readonly Vector2 NormalizedPosition;

        public HoverZoneInfo(
            HoverZoneCategory category,
            string name,
            string interpretation,
            float sox9Like,
            float foxl2Like,
            float dapiLike,
            Vector2 normalizedPosition)
        {
            Category = category;
            Name = name;
            Interpretation = interpretation;
            Sox9Like = Mathf.Clamp01(sox9Like);
            Foxl2Like = Mathf.Clamp01(foxl2Like);
            DapiLike = Mathf.Clamp01(dapiLike);
            NormalizedPosition = normalizedPosition;
        }
    }

    public sealed class HoverZoneInterpreter
    {
        public HoverZoneInfo Interpret(Vector2 uv, VisualStateDescriptor visualState)
        {
            float tissue = EstimateTissuePresence(uv, visualState);
            float localCluster = SmoothNoise(uv * 11.7f + new Vector2(0.13f, 0.61f));
            float localPatch = SmoothNoise(uv * 5.1f + new Vector2(0.72f, 0.29f));
            float dapi = Mathf.Clamp01(tissue * (0.42f + visualState.DapiDensity * 0.58f) * (0.72f + localCluster * 0.42f));
            float sox9 = Mathf.Clamp01(tissue * visualState.Sox9Signal * (0.36f + localPatch * 0.82f));
            float foxl2 = Mathf.Clamp01(tissue * visualState.Foxl2Signal * (0.36f + (1f - localPatch) * 0.78f));

            if (visualState.Mixedness > 0.45f)
            {
                sox9 = Mathf.Clamp01(sox9 + tissue * visualState.Mixedness * 0.18f);
                foxl2 = Mathf.Clamp01(foxl2 + tissue * visualState.Mixedness * 0.18f);
            }

            HoverZoneCategory category;
            string name;
            string interpretation;

            if (sox9 >= 0.52f && foxl2 < 0.38f)
            {
                category = HoverZoneCategory.Sox9Dominant;
                name = "SOX9-like dominant zone";
                interpretation = "This region shows stronger simulated SOX9-like marker signal.\nIn this educational model, that suggests a more testis-like / Sertoli-like local identity.";
            }
            else if (foxl2 >= 0.52f && sox9 < 0.38f)
            {
                category = HoverZoneCategory.Foxl2Dominant;
                name = "FOXL2-like dominant zone";
                interpretation = "This region shows stronger simulated FOXL2-like marker signal.\nIn this educational model, that suggests a more ovary-like / granulosa-like local identity.";
            }
            else if (sox9 >= 0.34f && foxl2 >= 0.34f)
            {
                category = HoverZoneCategory.Mixed;
                name = "Mixed marker zone";
                interpretation = "This region contains both simulated SOX9-like and FOXL2-like signals.\nIn this educational model, that suggests a mixed or reprogramming-like local state.";
            }
            else
            {
                category = HoverZoneCategory.NucleiRichLowMarker;
                name = "Nuclei-rich low-marker zone";
                interpretation = "This region contains mostly DAPI-like nuclei signal with low visible SOX9-like and FOXL2-like marker signal.\nThis local area is not strongly emphasized by the current marker channels in the simplified model.";
            }

            return new HoverZoneInfo(category, name, interpretation, sox9, foxl2, dapi, uv);
        }

        private static float EstimateTissuePresence(Vector2 uv, VisualStateDescriptor visualState)
        {
            float left = Ellipse(uv, new Vector2(0.32f, 0.56f), new Vector2(0.24f, 0.13f), 0.14f);
            float center = Ellipse(uv, new Vector2(0.53f, 0.48f), new Vector2(0.27f, 0.16f), -0.08f);
            float right = Ellipse(uv, new Vector2(0.71f, 0.56f), new Vector2(0.2f, 0.14f), 0.12f);
            float follicle = Ellipse(uv, new Vector2(0.55f, 0.6f), new Vector2(0.18f, 0.18f), 0f);
            float unstablePatch = SmoothNoise(uv * 3.2f + new Vector2(0.31f, 0.47f));

            float elongated = Mathf.Max(left, Mathf.Max(center, right));
            float rounded = Mathf.Max(follicle, Mathf.Max(
                Ellipse(uv, new Vector2(0.37f, 0.42f), new Vector2(0.14f, 0.13f), 0f),
                Ellipse(uv, new Vector2(0.72f, 0.4f), new Vector2(0.13f, 0.12f), 0f)));
            float mixed = Mathf.Max(left, rounded);
            float fragmented = Mathf.Clamp01(Mathf.Max(elongated, rounded) * (0.55f + unstablePatch * 0.7f));

            float tissue =
                elongated * visualState.TestisBias +
                rounded * visualState.OvaryBias +
                mixed * visualState.Mixedness +
                fragmented * visualState.Instability;

            if (tissue <= 0.01f)
            {
                tissue = Mathf.Max(elongated, rounded) * 0.45f;
            }

            return Mathf.Clamp01(tissue);
        }

        private static float Ellipse(Vector2 uv, Vector2 center, Vector2 radius, float rotation)
        {
            float cos = (float)Math.Cos(rotation);
            float sin = (float)Math.Sin(rotation);
            Vector2 p = uv - center;
            float x = p.x * cos - p.y * sin;
            float y = p.x * sin + p.y * cos;
            float d = (x * x) / (radius.x * radius.x) + (y * y) / (radius.y * radius.y);
            return Mathf.Clamp01((1.18f - d) * 2.4f);
        }

        private static float SmoothNoise(Vector2 value)
        {
            float x = value.x;
            float y = value.y;
            double raw = Math.Sin(x * 12.9898f + y * 78.233f) * 43758.5453f;
            return Mathf.Clamp01((float)(raw - Math.Floor(raw)));
        }
    }
}
