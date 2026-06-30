# Synthetic Microscopy Spec

The simulated marker-pattern view should evolve from sparse schematic dots into a synthetic microscopy-style educational rendering.

The real Matson 2011 reference figure is used only as visual guidance for fluorescence style, tissue density, marker localization, and dark-field composition. The simulator must not recreate the paper figure, its panel layout, its labels, or its exact image composition.

## Reference Material

Primary local reference asset:

```text
Assets/Art/ReferenceFigures/Matson2011_Figure1.png
```

Expected optional docs copy:

```text
Docs/Matson2011_Figure1.png
```

If the docs copy is absent, use the Unity asset copy as the visual reference.

Citation:

```text
Matson CK et al. (2011). DMRT1 prevents female reprogramming in the postnatal mammalian testis.
Source: https://pmc.ncbi.nlm.nih.gov/articles/PMC3150961/
```

## Style Cues To Borrow

Borrow these high-level visual qualities:

- dark fluorescence background;
- dense blue nucleus-like signal inside tissue regions;
- green SOX9-like signal localized to selected cells or regions;
- red FOXL2-like signal localized to selected cells or regions;
- merged-channel overlay rather than separate cartoon symbols;
- tissue-shaped regions with empty lumens or background gaps;
- soft glowing signal spots;
- regional organization and clustering.

Do not borrow:

- exact panel arrangement;
- labels, crop boxes, scale bars, arrows, or inset composition;
- exact cell positions;
- exact tissue morphology;
- figure-specific experimental comparisons.

## Synthetic Output

The output should look like a single synthetic educational micrograph panel:

- black or near-black background;
- one or more irregular tissue masks;
- hundreds to thousands of blue nucleus-like ellipses;
- green/red marker channels blended over the nuclei field;
- soft falloff and channel intensity variation;
- state-specific organization driven by `GonadState.Fate`.

The output should be a continuous approximation, not a fixed image per label. As scores change, the synthetic microfield should gradually adjust channel intensity, compartment shape, lumen softness, density, patchiness, and organization.

Example:

```text
SOX9-like signal decreases gradually
FOXL2-like signal increases gradually
elongated compartments become rounder
green-dominant regions become red-dominant
```

Small parameter changes should not regenerate a completely different tissue field.

The view should include a nearby label such as:

```text
Synthetic marker pattern. Educational abstraction, not a real micrograph.
```

## State Style Targets

### TestisLike

- dense nuclei within cord-like or tubular tissue regions;
- stronger green SOX9-like channel;
- weaker red FOXL2-like channel;
- green signal organized in coherent regional bands or clusters.

### OvaryLike

- dense nuclei within rounded or follicle-like tissue regions;
- stronger red FOXL2-like channel;
- weaker green SOX9-like channel;
- red signal organized around clustered regions.

### Ovotestis

- both green and red channels visible;
- partial compartmentalization, with some regions more green and others more red;
- limited overlap where educationally useful.

### Unstable

- tissue still present, not random confetti;
- weaker channel intensity;
- patchy, inconsistent, lower-confidence green/red distribution;
- more visual gaps and regional irregularity.

## Scope Boundary

The synthetic microscopy renderer is not:

- a real microscopy image;
- a reproduction of a paper figure;
- an experimental method;
- a staining protocol;
- medical or laboratory guidance;
- evidence of a biological result.

The visual transition is a teaching abstraction. Real biological development and cell fate reprogramming are not smooth UI sliders and do not occur simply because one parameter changes.
