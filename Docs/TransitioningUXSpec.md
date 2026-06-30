# Transitioning UX Spec

The user should experience the synthetic marker panel as a stable visual field that changes smoothly as parameters change.

## Slider Behavior

When sliders move:

```text
output bars update immediately
3D gonad updates immediately
synthetic marker pattern smoothly changes
interpretation label updates when thresholds are crossed
```

The marker pattern should not flicker, randomize, or replace the entire tissue field during small slider movements.

## Sequential Feel

When a user drags sliders, the visual process should unfold in a readable order:

1. SOX9-like and FOXL2-like marker balance changes first;
2. local marker patchiness follows;
3. tubule-like versus follicle-like morphology changes next;
4. connectedness, fragmentation, and spacing change more slowly.

The label may update as thresholds are crossed, but the tissue image should continue moving smoothly.

## Interpretation Labels

The UI can use softer labels than only `Testis-like` and `Ovary-like`.

Suggested labels:

- Mostly testis-like;
- Leaning testis-like;
- Mixed / reprogramming-like;
- Leaning ovary-like;
- Mostly ovary-like;
- Unstable.

These labels should summarize the current state without implying real diagnosis, clinical prediction, or direct biological determinism.

## Optional Transition Meter

Future UI may include a simple meter:

```text
Testis-like ←─────●─────→ Ovary-like
```

or:

```text
SOX9-dominant  |  Mixed  |  FOXL2-dominant
```

This meter should be educational and unitless.

## Smoothness Rule

The visual should not jump randomly while dragging.

If performance requires simplification:

- update shader parameters every frame;
- avoid CPU texture regeneration;
- use stable seeds;
- only regenerate major geometry when a scenario preset is selected or when a deliberate reset occurs.

## Preset Behavior

When a user clicks a preset, a faster transition is acceptable.

Future implementation may animate preset transitions over:

```text
0.3–0.8 seconds
```

Instant replacement should be avoided when smooth interpolation is practical.

## Scientific Honesty Note

The visual transition is a teaching abstraction. Real biological development and cell fate reprogramming are not smooth UI sliders and do not occur simply because one parameter changes. The simulator uses continuous visual interpolation to help users understand relationships between signals, markers, and tissue-like patterns.

The simulator uses time-smoothed educational visual transitions to show relationships between states. This is not a literal real-time biological movie.

## Scope Label

The marker panel should continue to show:

```text
Shader-generated synthetic marker pattern. Educational abstraction, not real microscopy.
```
