# Simulation Rules

The Level 1 model should use simple, transparent rules. It should not attempt to be a complicated biophysical model.

The goal is to help the user see how timing, thresholds, pathway competition, and uncertainty can affect a simplified developmental outcome.

These thresholds are educational approximations, not experimentally fitted biological constants.

## Core Logic

Example logic:

```text
effective_sry = SryStrength * timing_factor
sox9_score = effective_sry * Sox9Sensitivity
ovary_pathway_score = WntBetaCateninStrength

testis_score = sox9_score - ovary_pathway_score * inhibition_factor
ovary_score = ovary_pathway_score - sox9_score * inhibition_factor
instability_score = high when both scores are close or both weak
```

The scores should be clamped to a readable range, such as `0.0f` to `1.0f`, before display.

## Timing Factor

SRY must act during a critical developmental window. If SRY is too late, its ability to activate SOX9 is reduced.

Pseudo-code:

```csharp
float CalculateTimingFactor(float sryTiming)
{
    // sryTiming 0.0 = too early / absent
    // sryTiming 0.5 = optimal window
    // sryTiming 1.0 = too late
    // Return a high value near 0.5 and lower values away from it.
}
```

A simple implementation can use a triangular or bell-shaped curve centered on `0.5f`.

## Genetic Background Modifier

`GeneticBackgroundModifier` represents simplified strain or context sensitivity. It should slightly modify pathway scores without pretending to represent a full genotype.

In the current Level 1 model, `0.5` is neutral. Values below `0.5` slightly reduce the SOX9/testis-pathway side and values above `0.5` slightly increase it. The modifier should not behave like direct percentage signal loss.

Example conceptual use:

```text
sox9_score = sox9_score * GeneticBackgroundModifier
```

The exact formula can change later, but the UI should make clear that this is a simplified educational control.

## Default / Normal XY Calibration

The default and Normal XY preset should produce a deterministic testis-like outcome:

```csharp
SryStrength = 0.95f;
SryTiming = 0.50f;
Sox9Sensitivity = 0.95f;
WntBetaCateninStrength = 0.20f;
GeneticBackgroundModifier = 0.50f;
Noise = 0.0f;
```

Expected educational output:

- `Fate = GonadFate.TestisLike`;
- high testis score;
- low ovary score;
- low instability score.

## Noise

`Noise` represents uncertainty and stochastic variation. It can slightly perturb testis and ovary scores before outcome classification.

Noise should not hide the underlying model. The UI should still show the main deterministic scores or provide a way to reset noise for demonstration.

## Outcome Classification

Pseudo-logic:

```csharp
if (testisScore > 0.7f && testisScore > ovaryScore)
{
    fate = GonadFate.TestisLike;
}
else if (ovaryScore > 0.7f && ovaryScore > testisScore)
{
    fate = GonadFate.OvaryLike;
}
else if (testisScore > 0.4f && ovaryScore > 0.4f)
{
    fate = GonadFate.Ovotestis;
}
else
{
    fate = GonadFate.Unstable;
}
```

## Explanation Rules

The simulator should generate short explanations tied to the dominant model features.

Example:

```text
SRY signal was strong and occurred inside the critical window, allowing SOX9 to dominate over WNT/β-catenin.
```

Alternative example:

```text
SRY signal was absent or too weak, so the ovarian-supporting WNT/β-catenin pathway dominated.
```

Explanations should avoid clinical claims, wet-lab steps, or direct human translation.
