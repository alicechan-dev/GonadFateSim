Shader "GonadFateSim/SyntheticMarkerPattern"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Seed ("Seed", Float) = 1
        _TissueMode ("Tissue Mode", Float) = 0
        _NucleiDensity ("Nuclei Density", Float) = 1
        _DapiIntensity ("DAPI Intensity", Float) = 1
        _Sox9Intensity ("SOX9-like Intensity", Float) = 0.2
        _Foxl2Intensity ("FOXL2-like Intensity", Float) = 0.2
        _Irregularity ("Tissue Irregularity", Float) = 0.45
        _ChannelMode ("Channel Mode", Float) = 0
        _MarkerSpotDensity ("Marker Spot Density", Float) = 1
        _GapSoftness ("Internal Gap Softness", Float) = 0.6
        _TestisBias ("Testis Bias", Float) = 0
        _OvaryBias ("Ovary Bias", Float) = 0
        _Mixedness ("Mixedness", Float) = 0
        _Instability ("Instability", Float) = 0
        _Region0 ("Tissue Region 0", Vector) = (0.25, 0.48, 0.25, 0.2)
        _Region1 ("Tissue Region 1", Vector) = (0.5, 0.58, 0.28, 0.18)
        _Region2 ("Tissue Region 2", Vector) = (0.72, 0.42, 0.22, 0.2)
        _Region3 ("Tissue Region 3", Vector) = (0.48, 0.28, 0.2, 0.14)
        _Gap0 ("Gap 0", Vector) = (0.34, 0.48, 0.08, 0.1)
        _Gap1 ("Gap 1", Vector) = (0.62, 0.56, 0.1, 0.08)
        _Gap2 ("Gap 2", Vector) = (0.5, 0.34, 0.08, 0.07)
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Seed;
            float _TissueMode;
            float _NucleiDensity;
            float _DapiIntensity;
            float _Sox9Intensity;
            float _Foxl2Intensity;
            float _Irregularity;
            float _ChannelMode;
            float _MarkerSpotDensity;
            float _GapSoftness;
            float _TestisBias;
            float _OvaryBias;
            float _Mixedness;
            float _Instability;
            float4 _Region0;
            float4 _Region1;
            float4 _Region2;
            float4 _Region3;
            float4 _Gap0;
            float4 _Gap1;
            float4 _Gap2;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.color = v.color;
                return o;
            }

            float hash12(float2 p)
            {
                float3 p3 = frac(float3(p.xyx) * 0.1031);
                p3 += dot(p3, p3.yzx + 33.33);
                return frac((p3.x + p3.y) * p3.z);
            }

            float valueNoise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);
                float a = hash12(i);
                float b = hash12(i + float2(1, 0));
                float c = hash12(i + float2(0, 1));
                float d = hash12(i + float2(1, 1));
                float2 u = f * f * (3.0 - 2.0 * f);
                return lerp(lerp(a, b, u.x), lerp(c, d, u.x), u.y);
            }

            float ellipse(float2 uv, float4 region)
            {
                float2 d = (uv - region.xy) / max(region.zw, 0.001);
                float dist = dot(d, d);
                return smoothstep(1.08, 0.62, dist);
            }

            float ellipseScaled(float2 uv, float4 region, float2 scale)
            {
                float2 radius = max(region.zw * scale, 0.001);
                float2 d = (uv - region.xy) / radius;
                float dist = dot(d, d);
                return smoothstep(1.08, 0.62, dist);
            }

            float ellipseOriented(float2 uv, float4 region, float2 scale, float angle)
            {
                float s = sin(angle);
                float c = cos(angle);
                float2 d = uv - region.xy;
                float2 rotated = float2(d.x * c - d.y * s, d.x * s + d.y * c);
                float2 radius = max(region.zw * scale, 0.001);
                float dist = dot(rotated / radius, rotated / radius);
                return smoothstep(1.08, 0.62, dist);
            }

            float compartmentShell(float2 uv, float4 region, float2 outerScale, float2 innerScale, float innerStrength)
            {
                float outer = ellipseScaled(uv, region, outerScale);
                float inner = ellipseScaled(uv, region, innerScale);
                float rim = saturate(outer - inner * innerStrength);
                return smoothstep(0.02, 0.72, rim);
            }

            float orientedCompartmentShell(float2 uv, float4 region, float2 outerScale, float2 innerScale, float innerStrength, float angle)
            {
                float outer = ellipseOriented(uv, region, outerScale, angle);
                float inner = ellipseOriented(uv, region, innerScale, angle);
                float rim = saturate(outer - inner * innerStrength);
                return smoothstep(0.02, 0.72, rim);
            }

            float lumenMask(float2 uv, float4 gap, float angle, float seedOffset)
            {
                float localNoise = valueNoise(uv * 18.0 + _Seed * 0.23 + seedOffset);
                float2 warped = uv + (localNoise - 0.5) * 0.018;
                float core = ellipseOriented(warped, gap, float2(1.12, 0.86), angle);
                float shoulder = ellipseOriented(warped, gap, float2(1.34, 1.08), angle) * 0.36;
                return saturate(core * 0.82 + shoulder);
            }

            float tubuleCompartmentMask(float2 uv)
            {
                float mask = 0;
                mask += orientedCompartmentShell(uv, _Region0, float2(1.0, 0.96), float2(0.36, 0.48), 1.08, -0.34);
                mask += orientedCompartmentShell(uv, _Region1, float2(1.04, 0.96), float2(0.38, 0.5), 1.08, 0.26);
                mask += orientedCompartmentShell(uv, _Region2, float2(1.0, 0.96), float2(0.36, 0.48), 1.08, -0.22);
                mask += orientedCompartmentShell(uv, _Region3, float2(0.96, 0.94), float2(0.34, 0.46), 1.04, 0.32);

                float cordTexture = valueNoise(uv * float2(26.0, 12.0) + _Seed * 0.27);
                float cellularRim = smoothstep(0.42, 0.84, cordTexture) * 0.08;
                return saturate(mask + cellularRim * mask);
            }

            float follicleCompartmentMask(float2 uv)
            {
                float mask = 0;
                mask += orientedCompartmentShell(uv, _Region0, float2(1.0, 0.92), float2(0.42, 0.38), 0.92, 0.18);
                mask += orientedCompartmentShell(uv, _Region1, float2(0.94, 1.06), float2(0.38, 0.44), 0.9, -0.12);
                mask += orientedCompartmentShell(uv, _Region2, float2(1.04, 0.94), float2(0.46, 0.4), 0.94, 0.08);
                mask += orientedCompartmentShell(uv, _Region3, float2(0.9, 0.98), float2(0.36, 0.42), 0.86, -0.22);

                float interstitium = (ellipseScaled(uv, _Region0, float2(1.12, 1.1)) + ellipseScaled(uv, _Region1, float2(1.1, 1.12)) + ellipseScaled(uv, _Region2, float2(1.12, 1.08))) * 0.08;
                return saturate(mask + interstitium);
            }

            float mixedCompartmentMask(float2 uv)
            {
                float tubules = orientedCompartmentShell(uv, _Region0, float2(1.08, 0.98), float2(0.44, 0.5), 1.1, -0.24)
                    + orientedCompartmentShell(uv, _Region1, float2(1.04, 0.98), float2(0.4, 0.48), 1.08, 0.22);
                float follicles = orientedCompartmentShell(uv, _Region2, float2(1.0, 0.94), float2(0.42, 0.38), 0.94, 0.12)
                    + orientedCompartmentShell(uv, _Region3, float2(0.9, 0.98), float2(0.36, 0.42), 0.88, -0.18);
                return saturate(tubules + follicles);
            }

            float unstableCompartmentMask(float2 uv)
            {
                float mask = 0;
                mask += orientedCompartmentShell(uv, _Region0, float2(0.98, 0.78), float2(0.42, 0.34), 0.92, -0.32);
                mask += orientedCompartmentShell(uv, _Region1, float2(0.9, 0.94), float2(0.38, 0.42), 0.9, 0.14);
                mask += orientedCompartmentShell(uv, _Region2, float2(1.06, 0.72), float2(0.48, 0.32), 0.98, 0.28);
                mask += orientedCompartmentShell(uv, _Region3, float2(0.72, 0.66), float2(0.3, 0.28), 0.82, -0.08);

                float fragmentation = valueNoise(uv * 16.0 + _Seed * 0.47);
                return saturate(mask * lerp(0.38, 1.05, fragmentation));
            }

            float tissueMask(float2 uv)
            {
                float n = valueNoise(uv * 8.0 + _Seed * 0.13);
                float2 warped = uv + (n - 0.5) * _Irregularity * 0.075;

                float testisMask = tubuleCompartmentMask(warped);
                float ovaryMask = follicleCompartmentMask(warped);
                float ovotestisMask = mixedCompartmentMask(warped);
                float unstableMask = unstableCompartmentMask(warped);

                float testisWeight = saturate(_TestisBias);
                float ovaryWeight = saturate(_OvaryBias);
                float mixedWeight = saturate(_Mixedness);
                float unstableWeight = saturate(_Instability);
                float totalWeight = max(0.001, testisWeight + ovaryWeight + mixedWeight + unstableWeight);
                testisWeight /= totalWeight;
                ovaryWeight /= totalWeight;
                mixedWeight /= totalWeight;
                unstableWeight /= totalWeight;

                float tissue = testisMask * testisWeight + ovaryMask * ovaryWeight + ovotestisMask * mixedWeight + unstableMask * unstableWeight;

                float genericGaps = 0;
                genericGaps += ellipse(warped, _Gap0);
                genericGaps += ellipse(warped, _Gap1);
                genericGaps += ellipse(warped, _Gap2);

                float testisGaps = 0;
                testisGaps += lumenMask(warped, _Gap0, -0.34, 3.0);
                testisGaps += lumenMask(warped, _Gap1, 0.26, 7.0);
                testisGaps += lumenMask(warped, _Gap2, -0.22, 13.0);
                float gaps = lerp(genericGaps, testisGaps, saturate(testisWeight + mixedWeight * 0.65));

                float edgeNoise = valueNoise(uv * 21.0 + _Seed * 0.31);
                float compartmentTexture = lerp(0.72, 1.14, edgeNoise);
                float softGaps = smoothstep(0.06, max(0.16, _GapSoftness), gaps);
                tissue = saturate(tissue * compartmentTexture - softGaps * 0.36);
                return smoothstep(0.07, 0.68, tissue);
            }

            float nucleiLayer(float2 uv, float mask)
            {
                float density = lerp(62.0, 118.0, saturate(_NucleiDensity));
                float2 grid = uv * density;
                float2 cell = floor(grid);
                float2 local = frac(grid);
                float rnd = hash12(cell + _Seed);
                float rnd2 = hash12(cell + _Seed + 17.7);
                float2 center = float2(hash12(cell + _Seed + 3.1), hash12(cell + _Seed + 9.4));
                float2 delta = local - center;
                delta.x *= lerp(0.75, 1.45, rnd2);
                float radius = lerp(0.095, 0.22, rnd);
                float spot = smoothstep(radius, 0.0, length(delta));

                float cluster = valueNoise(uv * 10.0 + _Seed * 0.07);
                float keepThreshold = lerp(0.34, 0.08, cluster);
                float keep = step(keepThreshold, rnd) * mask;
                return spot * keep * lerp(0.45, 1.25, cluster);
            }

            float channelField(float2 uv, float offset)
            {
                float broad = valueNoise(uv * 4.5 + _Seed * 0.19 + offset);
                float local = valueNoise(uv * 18.0 + _Seed * 0.41 + offset);
                float field = smoothstep(0.48, 0.78, broad) * 0.38 + smoothstep(0.68, 0.92, local) * 0.35;
                return saturate(field);
            }

            float markerCellLayer(float2 uv, float mask, float offset, float regionalBias)
            {
                float density = lerp(52.0, 78.0, saturate(_MarkerSpotDensity));
                float2 grid = uv * density;
                float2 cell = floor(grid);
                float2 local = frac(grid);
                float rnd = hash12(cell + _Seed + offset);
                float rnd2 = hash12(cell + _Seed + offset + 19.3);
                float2 center = float2(hash12(cell + _Seed + offset + 2.7), hash12(cell + _Seed + offset + 8.9));
                float2 delta = local - center;
                delta.x *= lerp(0.82, 1.32, rnd2);
                float radius = lerp(0.105, 0.17, rnd);
                float radial = smoothstep(radius, 0.0, length(delta));
                float cluster = valueNoise(uv * 7.0 + _Seed * 0.17 + offset);
                float keep = step(0.58, rnd) * smoothstep(0.3, 0.82, regionalBias + cluster * 0.5);
                return radial * keep * mask;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float mask = tissueMask(uv);
                float nuclei = nucleiLayer(uv, mask);
                float greenBias = channelField(uv, 11.0);
                float redBias = channelField(uv.yx + float2(0.13, -0.07), 37.0);
                float greenCells = markerCellLayer(uv, mask, 11.0, greenBias);
                float redCells = markerCellLayer(uv.yx + float2(0.13, -0.07), mask, 37.0, redBias);
                float testisBias = saturate(_TestisBias);
                float greenBroadWeight = lerp(0.42, 0.22, testisBias);
                float greenCellWeight = lerp(1.35, 1.75, testisBias);
                float redBroadWeight = lerp(0.42, 0.18, testisBias);
                float redCellWeight = lerp(1.35, 0.7, testisBias);
                float green = (greenBias * greenBroadWeight + greenCells * greenCellWeight) * mask * _Sox9Intensity;
                float red = (redBias * redBroadWeight + redCells * redCellWeight) * mask * _Foxl2Intensity;

                float tissueGlow = mask * 0.055;
                float3 color = float3(0.004, 0.006, 0.014) + tissueGlow * float3(0.18, 0.16, 0.28);
                float showMerged = 1.0 - step(0.5, _ChannelMode);
                float showSox9 = 1.0 - step(0.5, abs(_ChannelMode - 1.0));
                float showFoxl2 = 1.0 - step(0.5, abs(_ChannelMode - 2.0));
                float showDapi = 1.0 - step(0.5, abs(_ChannelMode - 3.0));
                float dapiWeight = max(showMerged, showDapi);
                float greenWeight = max(showMerged, showSox9);
                float redWeight = max(showMerged, showFoxl2);

                color += nuclei * _DapiIntensity * dapiWeight * float3(0.12, 0.24, 1.0);
                color += green * greenWeight * float3(0.04, 0.95, 0.22);
                color += red * redWeight * float3(1.0, 0.06, 0.11);
                color += (green * red) * showMerged * float3(0.55, 0.15, 0.45);
                color += (showSox9 + showFoxl2) * nuclei * 0.08 * float3(0.05, 0.08, 0.18);
                color = saturate(color);

                return fixed4(color, 1.0) * i.color;
            }
            ENDCG
        }
    }
}
