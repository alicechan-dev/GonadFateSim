# Marker Shader Rendering Plan

The marker-pattern renderer is moving from CPU `Texture2D` pixel generation to a shader-driven synthetic microscopy panel.

## Why Change

The CPU renderer drew many soft nuclei and channel marks into a `Texture2D` with `SetPixel` loops and `Texture2D.Apply`. That worked visually, but slider changes could lag because each simulation update forced the CPU to rebuild thousands of pixels and upload the texture again.

The shader approach keeps the expensive dense visual detail on the GPU. C# updates compact parameters, while the fragment shader computes tissue masks, nuclei-like dots, and fluorescence channels per pixel.

## Target Pipeline

```text
SimulationParameters
↓
SexDeterminationSimulator
↓
GonadState / marker scores
↓
C# tissue shape descriptors and material parameters
↓
RawImage material shader
↓
GPU procedural marker rendering
```

## First Prototype

Implemented prototype path:

- `SyntheticMarkerPattern.shader`;
- `MarkerPatternShaderView`;
- `TissueShapeController`;
- `TissueRegionDescriptor`;
- `MarkerShaderParameterBinder`;
- `MarkerPatternRenderMode`.

The first shader prototype renders:

- dark fluorescence background;
- procedural tissue masks;
- dense blue DAPI/nuclei-like dots;
- preliminary SOX9-like green and FOXL2-like red channels;
- soft fluorescence-style blending.

## C# Responsibilities

C# should:

- receive `GonadState`;
- keep stable random seeds per `GonadFate`;
- maintain tissue region descriptors;
- bind SOX9-like, FOXL2-like, DAPI-like, density, seed, and irregularity parameters;
- update material parameters when sliders or presets change;
- avoid per-dot GameObjects;
- avoid CPU `SetPixel` loops during normal shader rendering.

## Shader Responsibilities

The shader should:

- compute procedural tissue masks;
- compute dense nuclei-like dots inside tissue masks;
- compute localized SOX9-like and FOXL2-like fluorescence fields;
- composite a dark-background merged fluorescence output;
- use deterministic hash/noise so the image does not flicker.

## Fallback

The older CPU `MarkerPatternRenderer` remains as a fallback for environments where the shader is missing or unsupported.

Fallback behavior:

- `MarkerPatternRenderMode.CpuFallback`;
- generated `Texture2D`;
- slower but still functional;
- useful for debugging or platforms with shader compatibility issues.

## Reference Boundary

`Assets/Art/ReferenceFigures/Matson2011_Figure1.png` guides visual style only:

- dark fluorescence background;
- dense nuclei;
- localized red/green channel feel;
- tissue gaps;
- soft merged signal.

The shader must not copy the paper figure, panel layout, labels, exact tissue shapes, exact cell positions, scale bars, or annotations.

