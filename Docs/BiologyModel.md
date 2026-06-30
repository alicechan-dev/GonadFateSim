# Biology Model

This document describes the simplified biological model for Level 1: Sex Determination Simulator.

The goal is to help programmers and engineers understand the relationship:

```text
gene/pathway signal
↓
cell fate bias
↓
tissue output
↓
phenotype
```

Useful programming analogies:

- SRY = bootloader for the testis pathway;
- SOX9 = main testis fate module;
- WNT4 / RSPO1 / β-catenin = ovarian pathway module;
- DMRT1 = later testis maintenance guard, not central in Level 1;
- FOXL2 = later ovarian maintenance guard, not central in Level 1.

The Level 1 model focuses on embryonic sex determination. Later milestones may add adult gonadal fate maintenance concepts such as DMRT1, FOXL2, SOX8, and SOX9 maintenance roles, but those are not part of the first simulator calculation.

## 2.1 Core Entities

### SRY

SRY is usually associated with XY development in mice. In this educational model, SRY acts like a bootloader or feature flag that attempts to start the testis pathway.

SRY is active during a narrow embryonic developmental window. Timing matters: a strong signal outside the useful window may have less effect than a moderate signal inside the window.

SRY helps activate SOX9. If SRY is absent, weak, or delayed, the testis pathway may fail to stabilize.

### SOX9

SOX9 supports Sertoli cell fate. High SOX9 activity is associated with testis development in the simplified Level 1 model.

In the simulator, SOX9 must cross a simplified threshold before the model treats the testis pathway as stable. This threshold is an educational approximation, not a measured biological constant.

### WNT4 / RSPO1 / β-catenin

WNT4, RSPO1, and β-catenin are represented together as an ovarian-supporting pathway module.

This pathway competes with the SOX9/testis direction. If the testis pathway does not stabilize, the WNT/β-catenin direction may dominate and bias the gonad toward an ovary-like fate.

### DMRT1, FOXL2, SOX8, and Maintenance Factors

DMRT1, FOXL2, and SOX8 are important for later gonadal fate maintenance and reprogramming concepts in mouse models. In programming terms, they are closer to long-running guard processes than first-boot switches.

Level 1 mentions these factors only as future context. They should not affect the initial SRY/SOX9/WNT score unless a later milestone explicitly adds maintenance-state simulation.

### Cell Fate

For the first version, the simulator uses simple output states:

```csharp
public enum GonadFate
{
    Undetermined,
    TestisLike,
    OvaryLike,
    Ovotestis,
    Unstable
}
```

These states describe educational model outputs, not complete biological or clinical categories.

## 2.2 Main Causal Map

```text
Chromosomal setup
↓
SRY timing and strength
↓
SOX9 activation
↓
competition with WNT/β-catenin
↓
supporting cell fate bias
↓
gonad phenotype output
```

The core Level 1 path is:

```text
XY + Sry ON during critical developmental window
↓
SOX9 up
↓
Sertoli cell fate bias
↓
testis-like gonad output
```

The main alternative path is:

```text
Sry absent / weak / delayed
↓
SOX9 not sufficiently activated
↓
WNT4 / RSPO1 / β-catenin pathway dominates
↓
granulosa / ovary-like fate bias
↓
ovary-like or ovotestis output
```

## 2.3 What the Model Does NOT Claim

This model does not represent full embryonic development.

It does not model fertility, behavior, complete endocrine feedback, complete anatomy, humans, medical outcomes, or clinical decision-making.

It is not medical advice.

It is not a laboratory protocol.

It does not provide instructions for gene editing, animal experiments, surgery, injections, viral delivery, embryo manipulation, or any wet-lab procedure.
