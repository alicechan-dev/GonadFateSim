# Visual State Space Model

The marker renderer should use continuous visual variables instead of relying only on `GonadFate`.

`GonadFate` is still useful for readable interpretation. The renderer should also receive a continuous descriptor that can represent in-between states.

## Continuous Variables

Suggested visual state fields:

```text
testisBias: 0..1
ovaryBias: 0..1
mixedness: 0..1
instability: 0..1

sox9Signal: 0..1
foxl2Signal: 0..1
dapiDensity: 0..1

elongation: 0..1
roundness: 0..1
connectedness: 0..1
fragmentation: 0..1
lumenSize: 0..1
lumenSoftness: 0..1
compartmentSpacing: 0..1
markerPatchiness: 0..1
```

These values should be continuous even when the UI label is discrete.

Example:

```text
testisBias = 0.65
ovaryBias = 0.25
mixedness = 0.35
```

This should produce a visible in-between state, not a hard swap to a fixed testis-like or mixed image.

## High Testis Bias

When `testisBias` is high:

- compartments become more elongated;
- tubule-like or cord-like structures are emphasized;
- SOX9-like green signal is strong;
- FOXL2-like red signal is low;
- compartments are separated or semi-separated;
- oval lumen-like gaps may be visible;
- green marker-positive spots appear along tissue compartments.

## High Ovary Bias

When `ovaryBias` is high:

- compartments become rounder;
- follicle-like clustered structures are emphasized;
- FOXL2-like red signal is strong;
- SOX9-like green signal is low;
- rounded tissue regions are more clustered.

## High Mixedness

When `mixedness` is high:

- some structures remain tubule-like;
- some structures become rounded or follicle-like;
- both SOX9-like and FOXL2-like signals are visible;
- red and green channels may partially compartmentalize;
- overlap should remain educational and synthetic.

## High Instability

When `instability` is high:

- tissue is patchier;
- edges become more irregular;
- channel patterns are weaker or inconsistent;
- compartments are less organized;
- DAPI-like density may become less uniform.

## Visual Variables Are Not Biology

These variables are rendering descriptors. They do not represent measured biological constants, cell counts, real concentration, real development time, or clinical state.
