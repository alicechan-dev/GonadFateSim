# Anatomy and Phenotype Spec

This document defines visual states that can be shown safely and honestly in the simulator.

The simulator should clearly separate:

- concepts observed in mouse-model research;
- simplified computational abstractions;
- speculative or future educational visualizations.

It should not imply full biological prediction, clinical relevance, human application, or experimental reproducibility.

## Visual State: Undifferentiated Gonad

Purpose:

- represent the early neutral starting point before the simplified model resolves into a bias.

Visual style:

- small paired schematic structures;
- neutral color palette;
- low pathway intensity;
- no clinical or procedural detail.

Labeling:

- "undifferentiated gonad";
- "early educational state";
- "pathway bias not yet resolved".

## Visual State: Testis-Like Gonad

Purpose:

- represent a testis-biased output from strong SOX9/testis-pathway activity in the simplified model.

Visual style:

- schematic cords or organized internal pattern;
- green or teal pathway highlight;
- visible but abstract organ shape.

Labeling:

- "testis-like gonad";
- "SOX9/testis pathway dominant";
- "mouse-model-inspired educational abstraction".

Avoid:

- claims that a complete testis has developed;
- fertility claims;
- clinical claims;
- human translation.

## Visual State: Ovary-Like Gonad

Purpose:

- represent an ovary-biased output from stronger WNT/beta-catenin ovarian-supporting activity.

Visual style:

- schematic follicle-like dots or soft clustered pattern;
- rose or warm pathway highlight;
- abstract organ shape.

Labeling:

- "ovary-like gonad";
- "WNT/beta-catenin pathway dominant";
- "conceptual mouse developmental biology view".

Avoid:

- claims of full ovarian function;
- reproductive or medical predictions;
- human application language.

## Visual State: Ovotestis / Mixed Gonad

Purpose:

- represent a mixed model state where testis-like and ovary-like pathway scores both remain active.

Visual style:

- split or mosaic schematic pattern;
- dual color overlay;
- visible uncertainty marker.

Labeling:

- "ovotestis / mixed state";
- "both pathway scores active";
- "model-level mixed outcome".

Avoid:

- procedural explanation;
- implying a desired or engineered result;
- direct mapping to real individual outcomes.

## Visual State: Unstable / Ambiguous State

Purpose:

- represent weak, close, or noisy pathway scores where the model does not choose a stable bias.

Visual style:

- muted colors;
- pulsing or dotted outline;
- question-mark or uncertainty icon if needed.

Labeling:

- "unstable educational state";
- "no strong pathway dominance";
- "ambiguous model output".

Avoid:

- diagnostic language;
- failure language that sounds clinical;
- overinterpretation.

## Simplified Endocrine Output

Purpose:

- show conceptual system-level output after gonad state selection.

Possible channels:

- androgen-like signal;
- estrogen-like signal;
- instability signal.

Rules:

- values are unitless;
- outputs are derived from model scores, not biological measurements;
- flow intensity is explanatory, not quantitative physiology;
- no dosing, treatment, or medical guidance.

## Stylized Whole Mouse Representation

Purpose:

- place gonad and endocrine outputs in a mouse-level visual context.

Visual style:

- simple mouse silhouette or low-detail 3D model;
- highlighted gonad region;
- optional abstract bloodstream overlay;
- no surgical, invasive, or clinical imagery.

Safe labels:

- "gonad state";
- "system-level educational context";
- "phenotype-like model output";
- "mouse developmental biology visualization".

Unsafe labels:

- "treatment outcome";
- "human prediction";
- "clinical sex phenotype";
- "how to change development".

## Scientific Conservatism

The simulator may draw inspiration from mouse developmental biology literature, but every visual should remain conservative:

- show bias rather than certainty;
- show pathway activity rather than complete mechanistic causation;
- show phenotype-like labels rather than diagnoses;
- include scope reminders in organism-level views;
- keep future maintenance or reprogramming concepts separate from Level 1 embryonic sex determination unless explicitly modeled later.
