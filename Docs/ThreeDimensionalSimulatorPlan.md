# Three-Dimensional Simulator Plan

The 3D simulator layer should turn the current educational model into a spatial, inspectable visualization. It should show a stylized transparent mouse body, an internal gonad region, gonad-state changes, abstract bloodstream/endocrine flow, hormone-output particles, labels, and camera controls.

The 3D layer is a view only. It must not contain biological calculation logic.

```text
SimulationParameters
↓
SexDeterminationSimulator
↓
GonadState
↓
3D mouse / gonad / endocrine visualization
```

## Core Principle

`SexDeterminationSimulator` remains the source of truth for model outputs. The 3D layer receives `GonadState` and renders it.

This separation keeps the simulator inspectable:

- sliders and presets update `SimulationParameters`;
- `SexDeterminationSimulator` calculates scores and `GonadState`;
- 2D UI and 3D views subscribe to the resulting state;
- 3D objects change shape, color, particles, labels, and visibility.

The 3D layer should never introduce hidden biological thresholds, extra fate decisions, or independent outcome calculations.

## 3D Scene Concept

The first 3D simulator view should include:

- simplified transparent mouse body / silhouette;
- highlighted internal gonad region;
- 3D gonad model driven by `GonadState.Fate`;
- abstract bloodstream or endocrine flow paths;
- unitless hormone-output particles or flow lines;
- floating labels and tooltips;
- orbit and zoom camera controls;
- layer toggles for body, gonad, bloodstream, hormones, labels, and gene network overlay.

## Educational Style

The scene should feel like a scientific teaching model, not a medical simulator.

Use:

- transparent materials;
- low-poly or primitive shapes;
- simple colors;
- schematic organ forms;
- labels that say "score", "bias", and "educational abstraction";
- non-realistic flow particles.

Avoid:

- realistic surgery visuals;
- gore or realistic blood;
- clinical diagnosis language;
- human-application claims;
- wet-lab or animal-procedure framing.

## Data Flow

Runtime flow:

```text
Parameter UI / scenario preset
↓
SimulationController
↓
SexDeterminationSimulator.Simulate(...)
↓
GonadState
↓
Gonad3DView
MouseBody3DView
EndocrineFlow3DView
HormoneParticleController
LabelBillboard
```

Each view should be replaceable. For example, an early primitive-based `Gonad3DView` can later be swapped with a low-poly asset-driven view without changing the simulation rules.

## First Target

Build `Gonad3DView` first. It should:

- accept `GonadState`;
- clear or reuse existing primitive child objects;
- create the correct stylized model for `GonadState.Fate`;
- color and label the model conservatively;
- expose the current state for click-to-inspect.

Mouse body, endocrine flow, camera polish, and optional Blender assets should come after the gonad model is working.

## Implemented First Slice

The first primitive-based `Gonad3DView` implementation now exists.

Current behavior:

- `RuntimeDashboardBootstrapper` creates a 3D render panel inside the dashboard;
- a dedicated camera renders the primitive gonad model into a UI view;
- the 3D camera renders to a `RenderTexture` displayed by a UI `RawImage`;
- the 3D viewport is isolated in a dedicated `3D Gonad View` panel;
- the 3D camera uses the `Gonad3D` layer as its culling mask;
- current fate text is shown as normal UI text outside the render texture;
- slider and preset changes recalculate `GonadState`;
- `Gonad3DView.UpdateFromState(GonadState state)` rebuilds the schematic model from `GonadState.Fate`;
- the panel includes safety text for educational scope.

This implementation is intentionally limited to the gonad model. It does not include mouse body, bloodstream, hormone particles, Blender assets, or orbit controls yet.
