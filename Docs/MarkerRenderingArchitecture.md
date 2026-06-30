# Marker Rendering Architecture

The marker-pattern renderer is now shader-first. The goal is to avoid rebuilding dense microscopy-like images on the CPU whenever sliders change.

Target flow:

```text
SimulationParameters
↓
SexDeterminationSimulator
↓
GonadState / marker scores
↓
VisualStateDescriptor
↓
C# tissue shape parameters
↓
GPU shader procedural marker rendering
↓
RawImage / RenderTexture display
```

## Data Flow

`SexDeterminationSimulator` remains the only source of fate scoring. The marker renderer receives `GonadState` and converts it into visual parameters.

The renderer must not add new biology logic. It only maps simulation output to educational visuals.

## Continuous Visual State Mapping

The shader renderer should not depend only on `GonadFate`.

`GonadFate` is useful for interpretation labels, but the image should be driven by continuous visual descriptors derived from model scores:

```text
Simulation scores
↓
VisualStateDescriptor
↓
Shader parameters
```

Continuous descriptors should include values such as:

- testis bias;
- ovary bias;
- mixedness;
- instability;
- SOX9-like signal;
- FOXL2-like signal;
- DAPI-like density;
- elongation;
- roundness;
- fragmentation;
- lumen size and softness;
- compartment spacing;
- marker patchiness.

Small changes in sliders should produce small changes in these values. The tissue field should morph smoothly rather than flickering or replacing all geometry.

## Current Shader Classes

### MarkerPatternShaderView

Displays the shader-rendered synthetic marker panel in a UI `RawImage`.

Responsibilities:

- create the `RawImage` display object;
- create and assign the `SyntheticMarkerPattern` material;
- call `MarkerShaderParameterBinder` when simulation state changes;
- keep labels and interpretation text visible;
- fall back to the CPU renderer if shader mode is unavailable.

### TissueShapeController

Maintains stable tissue region descriptors per `GonadFate`.

Responsibilities:

- regenerate tissue descriptors only when fate changes;
- preserve stable seeds so the marker image does not flicker during slider motion;
- provide region and gap descriptors to shader parameter binding.

### TissueRegionDescriptor

Compact data structure for shader-fed tissue regions.

Responsibilities:

- store normalized center;
- store normalized radius;
- convert region data to shader vectors.

### MarkerShaderParameterBinder

Maps `GonadState` to material parameters.

Responsibilities:

- bind seed, density, DAPI-like intensity, SOX9-like intensity, FOXL2-like intensity, and irregularity;
- bind tissue regions and gap regions;
- bind continuous visual descriptor values once `VisualStateDescriptor` exists;
- keep C# data compact.

### MarkerPatternRenderMode

Selects rendering mode:

- `Shader`;
- `CpuFallback`.

## Shader

### SyntheticMarkerPattern.shader

Fragment shader used by the marker panel.

Responsibilities:

- render a dark fluorescence-style background;
- compute procedural tissue masks from C# descriptors;
- render dense blue nuclei-like dots with deterministic hash/grid logic;
- render preliminary SOX9-like and FOXL2-like channel fields;
- composite a merged fluorescence-style output.

## CPU Fallback Classes

### TissueMaskGenerator

CPU fallback generator for synthetic tissue regions.

Responsibilities:

- generate one or more irregular masks;
- leave lumen-like or background gaps;
- vary mask organization by `GonadFate`;
- expose a query such as `Contains(Vector2 normalizedPoint)`.

### NucleiFieldGenerator

Places dense nucleus-like elements inside tissue masks.

Responsibilities:

- generate hundreds or thousands of nuclei;
- vary size, elongation, rotation, and intensity;
- keep nuclei inside tissue regions;
- avoid uniform scatter by using clustered or flow-biased sampling.

### MarkerChannelGenerator

Assigns SOX9-like and FOXL2-like channel intensity to regions and nuclei.

Responsibilities:

- map `TestisLike` to green-dominant patterns;
- map `OvaryLike` to red-dominant patterns;
- map `Ovotestis` to mixed/compartmentalized regions;
- map `Unstable` to weak, patchy, inconsistent signal;
- keep marker placement synthetic and not figure-specific.

### FluorescenceCompositeRenderer

Earlier planned CPU renderer role. In the shader-first path, this responsibility moves into `SyntheticMarkerPattern.shader`.

Responsibilities:

- draw dark background;
- draw soft blue nuclei;
- draw soft green/red marker signal;
- blend channels into a merged fluorescence-style output;
- apply mild bloom-like falloff without needing post-processing.

### MarkerPatternView / MarkerPatternRenderer

Earlier CPU `Texture2D` rendering path.

Responsibilities:

- useful as fallback/debug mode;
- should not be the normal path for slider-driven updates;
- should avoid per-cell GameObject creation.

## Performance Notes

CPU `Texture2D` generation can lag because thousands of stamps require CPU loops and texture uploads. It is acceptable only as fallback.

The preferred path is:

- update material parameters from C#;
- let the fragment shader generate procedural nuclei and marker fields;
- preserve tissue descriptors across small score changes;
- regenerate major tissue descriptors only for deliberate scenario changes or major state changes;
- preserve stable seeds to avoid flicker.
