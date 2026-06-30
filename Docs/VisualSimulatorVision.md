# Visual Simulator Vision

The project should evolve from a parameter dashboard into a multi-view educational simulator. The dashboard remains useful as the control surface, but it should become one layer in a larger visual explanation of mouse developmental biology concepts.

The goal is to let users move from abstract controls to visible biological meaning:

```text
gene/pathway controls
↓
pathway network activity
↓
gonad anatomy state
↓
endocrine / bloodstream output
↓
stylized whole-mouse context
↓
development timeline and scenario comparison
```

This is still an educational computational model. It does not reproduce full mouse development, does not provide experimental methods, and does not support clinical or human-application decisions.

## Layer 1: Controls

The control layer contains sliders, presets, reset controls, layer toggles, and comparison setup. It should make model inputs transparent and reversible.

Controls should use normalized educational values such as `0.0` to `1.0`. They should not use dosages, delivery routes, gene editing settings, surgical steps, or laboratory planning language.

## Layer 2: Pathway Graph

The pathway graph shows simplified SRY, SOX9, and WNT/beta-catenin relationships.

It should visualize:

- node activity;
- pathway dominance;
- mutual inhibition;
- instability or mixed-state bias;
- links between model parameters and pathway scores.

This layer explains the calculation, not a complete gene regulatory network.

## Layer 3: Gonad Anatomy

The gonad anatomy layer shows a stylized organ-level state:

- undifferentiated gonad;
- testis-like gonad;
- ovary-like gonad;
- ovotestis / mixed gonad;
- unstable / ambiguous state.

Visuals should be diagrammatic rather than realistic clinical imagery. They should teach pattern and state, not anatomy for intervention.

## Layer 4: Endocrine / Bloodstream Flow

The endocrine layer shows abstract hormone-like output as unitless signals moving through a simplified bloodstream view.

It may show:

- relative androgen-like output;
- relative estrogen-like output;
- instability warnings;
- endocrine balance as colored flow intensity.

Hormone values should remain conceptual and unitless unless a later safe educational scaling system is documented.

## Layer 5: Whole Mouse View

The whole mouse view should provide a simplified organism-level context. It may show a stylized mouse silhouette with highlighted internal systems, but it should avoid overclaiming full-body sex phenotype.

This view should communicate:

- gonad state as one developmental component;
- endocrine signals as abstract systemic outputs;
- phenotype-like educational labels rather than biological diagnoses.

This view may be implemented as a 2D schematic first and later as a stylized 3D simulator view. The 3D version should use a transparent low-poly or primitive-based mouse body, highlighted gonad region, abstract endocrine flow, and floating labels.

For the 3D direction, see [ThreeDimensionalSimulatorPlan.md](ThreeDimensionalSimulatorPlan.md), [ThreeDAnatomySpec.md](ThreeDAnatomySpec.md), [ThreeDAssetPipeline.md](ThreeDAssetPipeline.md), and [CameraAndInteractionSpec.md](CameraAndInteractionSpec.md).

## Layer 6: Timeline

The timeline layer lets users play, pause, scrub, or step through simplified developmental stages:

- early gonad;
- SRY timing window;
- SOX9 stabilization;
- pathway competition;
- gonad fate output;
- endocrine/system-level visualization.

The timeline should show model state transitions, not real embryological timing protocols.

## Layer 7: Comparison Mode

Comparison mode lets users place two or more safe educational scenarios side by side.

Examples:

- Normal XY vs delayed SRY;
- XX baseline vs high SOX9 concept;
- high WNT vs mixed outcome;
- low-noise vs high-noise conceptual uncertainty.

Comparison should help users reason about model behavior. It must not imply experimental planning or intervention design.

## Optional 3D Visualization Layer

The 3D layer should be a view fed by `GonadState`, not a separate simulation engine.

It should show:

- simplified transparent mouse body / silhouette;
- highlighted internal gonad region;
- 3D gonad model driven by `GonadState.Fate`;
- simplified bloodstream / endocrine flow;
- hormone-output particles or flow lines;
- floating labels and tooltips;
- camera orbit and zoom controls.

The first 3D implementation should use Unity primitives and procedural C# objects. Realistic anatomy should not be the starting point.
