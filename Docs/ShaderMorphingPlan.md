# Shader Morphing Plan

The shader renderer should morph tissue and marker appearance in real time from continuous simulation-derived values.

## Architecture

```text
SimulationParameters
↓
SexDeterminationSimulator
↓
GonadState + continuous scores
↓
VisualStateDescriptor
↓
TransitionController
↓
DisplayedVisualState
↓
MarkerShaderParameterBinder
↓
Shader-generated synthetic marker pattern
```

`SexDeterminationSimulator` remains the source of model scores. The visual layer maps those scores into rendering parameters.

## C# Responsibilities

C# should:

- compute or receive continuous scores;
- create a `VisualStateDescriptor`;
- keep a `TargetVisualState` from the newest simulation output;
- keep a `DisplayedVisualState` that approaches the target over time;
- apply property-specific smoothing speeds;
- keep stable tissue descriptors and seeds;
- update shader material parameters;
- avoid CPU pixel rendering during normal interaction;
- avoid regenerating full random layouts every frame.

## Shader Responsibilities

The shader should:

- render tissue masks;
- render nuclei-like DAPI dots;
- render SOX9-like green channel;
- render FOXL2-like red channel;
- morph shape properties using continuous parameters;
- blend channels into the merged fluorescence-style view.

## Stability

Small slider changes should not cause:

```text
entire tissue shape replaced
all dots jump
all compartments randomly regenerate
```

Small slider changes should instead cause:

```text
slight elongation/rounding change
signal intensity shift
local patch growth/shrink
lumen size change
compartment spacing change
```

## Suggested Classes

### VisualStateDescriptor

Continuous visual data passed toward the shader.

Suggested fields:

```csharp
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
```

### VisualStateMapper

Converts simulation scores into visual descriptors.

Example mapping:

```text
high TestisScore → high TestisBias, high Elongation, high SOX9
high OvaryScore → high OvaryBias, high Roundness, high FOXL2
high OvotestisScore → high Mixedness
high InstabilityScore → high Fragmentation and Patchiness
```

### TissueCompartmentDescriptor

Persistent geometry descriptor for compartments.

It can hold normalized center, base radius, orientation, lumen offset, and stable seed data.

### ContinuousTissueMorphController

Maintains persistent tissue descriptors and updates continuous morph values.

Responsibilities:

- preserve stable identity of tissue compartments;
- interpolate descriptors when visual scores change;
- regenerate major geometry only for explicit scenario changes or major state changes;
- prevent flicker while sliders move.

### TransitionController

Keeps displayed visual state separate from target visual state.

Responsibilities:

- receive the newest `GonadState`;
- map it into a target `VisualStateDescriptor`;
- smooth the displayed descriptor toward the target;
- update marker signals faster than morphology;
- update architecture bias, connectedness, fragmentation, and spacing more slowly.

### DisplayedVisualState

The current visual descriptor shown to the user.

It should drive shader parameters.

### TargetVisualState

The newest simulation-derived visual descriptor.

It should not be bound directly to the shader when smooth transitions are desired.

### MarkerShaderParameterBinder

Should bind `VisualStateDescriptor` values to shader parameters rather than relying only on `GonadFate`.

## Shader Parameter Examples

Future shader parameters may include:

```text
_TestisBias
_OvaryBias
_Mixedness
_Instability
_Sox9Signal
_Foxl2Signal
_DapiDensity
_Elongation
_Roundness
_Fragmentation
_LumenSize
_LumenSoftness
_CompartmentSpacing
_MarkerPatchiness
```

The shader should read these values from `DisplayedVisualState`, not directly from a newly calculated target state.

## Stable Spatial Identity

Persistent `TissueCompartmentDescriptor` objects should keep stable IDs, centers, base radii, orientations, and seeds.

`CompartmentMorphState` can then derive current center/radius/lumen values from the displayed state without replacing the compartment list.

The simulator uses time-smoothed educational visual transitions to show relationships between states. This is not a literal real-time biological movie.

## Fallback

If shader support is limited, CPU rendering may remain as a debug or fallback path. It should not be the normal slider-driven rendering path.
