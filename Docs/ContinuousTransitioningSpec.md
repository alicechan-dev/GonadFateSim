# Continuous Transitioning Spec

The marker renderer should evolve from discrete visual states into a continuous educational visual state-space.

Current simulator labels such as `TestisLike`, `OvaryLike`, `Ovotestis`, and `Unstable` remain useful for interpretation. The visual renderer, however, should not simply swap between hardcoded images. It should respond to continuous scores from the simulation.

Target flow:

```text
parameter changes
↓
continuous biological scores
↓
continuous visual descriptors
↓
shader-driven marker pattern morphs smoothly
```

Avoid this:

```text
Testis-like image
↓
Mixed image
↓
Ovary-like image
```

## Core Principle

Small parameter changes should produce small visual changes.

Large parameter changes should produce larger visible reorganization.

The marker panel should feel like a stable synthetic tissue field whose properties shift as the model scores shift.

## Example Transitions

As parameters change:

- SOX9-like signal decreases gradually;
- FOXL2-like signal increases gradually;
- elongated tubule-like regions slowly become rounder;
- separated compartments slowly become more clustered;
- green-dominant fields gradually become red-dominant;
- lumen-like gaps soften, shrink, or reorganize gradually;
- unstable states become patchier and less organized.

## Flicker Avoidance

Do not regenerate a completely different random tissue field on every tiny slider change.

The tissue should morph, not flicker.

Stable seeds, persistent compartment descriptors, and continuous shader parameters should preserve visual identity during slider motion.

## Displayed Versus Target State

The renderer should keep two visual states:

```text
TargetVisualState = latest simulation-derived descriptor
DisplayedVisualState = what the user currently sees
```

When parameters change, the target state may move immediately, but the displayed state should approach it over time.

This prevents threshold crossing from instantly replacing the tissue architecture.

## Sequential Transition Order

Transitions should feel ordered:

1. marker intensities change first;
2. local patchiness changes;
3. compartment shape changes;
4. spacing, connectedness, fragmentation, and architecture bias change last.

This creates a visible process:

```text
green/red balance shifts
↓
local signal patchiness changes
↓
compartments slowly reshape
↓
overall architecture reorganizes
```

## Property Speeds

Fast properties:

- SOX9-like signal;
- FOXL2-like signal;
- marker patchiness;
- DAPI density.

Medium properties:

- elongation;
- roundness;
- lumen size;
- lumen softness.

Slow properties:

- testis bias;
- ovary bias;
- mixedness;
- instability;
- connectedness;
- fragmentation;
- compartment spacing.

## Labels Versus Visuals

The UI may still display a discrete or semi-discrete interpretation label, such as:

- Mostly testis-like;
- Leaning testis-like;
- Mixed / reprogramming-like;
- Leaning ovary-like;
- Mostly ovary-like;
- Unstable.

The marker renderer should be driven by continuous values underneath those labels.

## Scientific Honesty

The visual transition is a teaching abstraction. Real biological development and cell fate reprogramming are not smooth UI sliders and do not occur simply because one parameter changes. The simulator uses continuous visual interpolation to help users understand relationships between signals, markers, and tissue-like patterns.

The simulator uses time-smoothed educational visual transitions to show relationships between states. This is not a literal real-time biological movie.

## Scope

This remains:

- educational;
- synthetic;
- mouse-model inspired;
- not real microscopy;
- not medical guidance;
- not a wet-lab protocol;
- not human application guidance.

Do not add CRISPR, animal procedure, surgery, injection, or medical content.
