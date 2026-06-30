# Marker Pattern Simulation Spec

The simulated marker-pattern view is an educational synthetic microscopy-style rendering generated in Unity/C#. It is inspired by marker-reading concepts and by the visual style of the local Matson 2011 reference figure, but it is not a recreation of a real microscopy figure.

The real paper figure is reference material. The simulator output is a synthetic educational abstraction.

## Visual Elements

The upgraded view should show:

- dark fluorescence-style background;
- irregular tissue masks with empty/background gaps;
- dense blue nucleus-like signal for DAPI-style cell locations;
- green SOX9-like marker regions;
- red FOXL2-like marker regions;
- merged fluorescence-style overlay;
- soft, microscopy-like spots rather than hard-edged cartoon dots.

The long-term renderer should generate a `Texture2D` or `RenderTexture`, not thousands of individual UI objects.

## State Mapping

The view responds to `GonadState.Fate`:

- `TestisLike` -> more green SOX9-positive clusters and fewer red FOXL2-positive clusters;
- `OvaryLike` -> more red FOXL2-positive clusters and fewer green SOX9-positive clusters;
- `Ovotestis` -> mixed green and red regions with partial compartmentalization or intermixing;
- `Unstable` -> weaker, patchier, inconsistent tissue-based signal;
- `Undetermined` -> low, balanced signal.

## Renderer Pipeline

Target architecture:

```text
SimulationState
↓
TissueMaskGenerator
↓
NucleiFieldGenerator
↓
MarkerChannelGenerator
↓
FluorescenceCompositeRenderer
↓
Texture2D / RenderTexture
```

Deterministic generation keeps the pattern stable while a scenario is selected. Regeneration should happen when simulation output changes.

The renderer should produce an image that looks tissue-based and channel-composited, not like random confetti.

## Hover Inspection

The marker view includes a synthetic local interpretation overlay.

The first implementation samples the pointer location over the marker `RawImage`, estimates local SOX9-like, FOXL2-like, and DAPI-like signal from the current displayed visual state, and classifies the local area as:

- SOX9-like dominant;
- FOXL2-like dominant;
- mixed marker;
- nuclei-rich low-marker.

The tooltip must describe these as simulated marker-dominant zones. It should not claim real microscopy analysis or definitive biological cell typing.

Future versions may add a shader-generated zone map or ID buffer for more precise tissue-region outlines.

## Scope Boundary

The simulated marker pattern is:

- an educational abstraction;
- generated from model output;
- not a real micrograph;
- not a staining protocol;
- not medical or laboratory guidance.

It must not be used as evidence of a biological result.
