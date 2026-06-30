# Safety and Scope

This project is an educational computational model.
It does not provide laboratory protocols.
It does not provide instructions for gene editing.
It does not include CRISPR guide design, viral delivery, injection schedules, animal handling instructions, surgical methods, or human application guidance.

The simulator is designed to explain ideas from mouse developmental biology at a conceptual level:

```text
gene/pathway ON
↓
cell fate bias
↓
tissue output
↓
phenotype
```

It is not a wet-lab protocol, medical tool, clinical planner, or self-experimentation guide.

## Visual Simulator Scope

The simulator may include visual layers for:

- gene and pathway activity;
- schematic gonad anatomy;
- abstract endocrine / bloodstream flow;
- stylized whole mouse context;
- stylized 3D transparent mouse and gonad views;
- developmental timeline playback;
- scenario comparison.
- real reference figure viewing with attribution;
- simulated marker-pattern schematics.

These visuals are educational abstractions. They are not anatomical training tools, medical predictions, experimental plans, or human-application guides.

## Reference Figure and Marker Pattern Boundary

The simulator may show real paper figures as reference material when an image asset, title/caption, and source link are provided.

The simulator may also generate dynamic marker-pattern schematics with:

- blue DAPI-like nuclei;
- green SOX9-positive schematic dots;
- red FOXL2-positive schematic dots.

The real reference figure and simulated marker view must remain clearly distinct.

The marker simulation is not a real micrograph, not a staining protocol, not evidence of a biological result, and not medical or laboratory guidance.

Continuous marker-pattern transitions are also educational abstractions. They do not represent real-time biological tissue transformation, actual development, clinical transition, or laboratory reprogramming.

Scientific honesty note:

```text
The visual transition is a teaching abstraction. Real biological development and cell fate reprogramming are not smooth UI sliders and do not occur simply because one parameter changes. The simulator uses continuous visual interpolation to help users understand relationships between signals, markers, and tissue-like patterns.
```

Marker hover inspection is also synthetic. Hover tooltips may describe local SOX9-like dominant, FOXL2-like dominant, mixed, or nuclei-rich low-marker zones, but these are generated display interpretations only.

Hover inspection must not be described as:

- real microscopy analysis;
- cell-type identification;
- diagnostic interpretation;
- evidence of a biological result;
- laboratory or medical guidance.

Use conservative wording such as "suggests", "marker-dominant", "local identity interpretation", and "simulated interpretation, not real microscopy."

## 3D Visualization Boundary

The 3D simulator may use:

- transparent primitive mouse body shapes;
- stylized 3D gonad states;
- abstract tubes, arcs, line renderers, or particles for endocrine flow;
- floating labels and tooltips;
- orbit and zoom camera controls.

The 3D simulator must not include:

- realistic surgery visuals;
- gore or realistic blood;
- injection methods or delivery routes;
- animal procedure instructions;
- clinical claims;
- human transition claims;
- CRISPR or gene editing instructions;
- realistic anatomy intended for intervention planning.

The 3D anatomy should remain schematic, low-detail, and conservative.

## Observed in Mouse Models

Examples of concepts observed in mouse model literature and suitable for conceptual educational modeling:

- SRY/SOX9 pathway effects;
- DMRT1 deletion causing granulosa-like reprogramming in mouse testis;
- FOXL2 ablation causing Sertoli-like reprogramming in mouse ovary;
- SOX9/SOX8 roles in protecting adult testis identity in mouse models;
- AR signaling affecting mouse external genitalia development.

These topics should be discussed as biological concepts, not as instructions for reproducing experiments.

The reference list in [ScientificBackground.md](ScientificBackground.md) is included to support educational context, not to provide experimental guidance.

## Hypothetical / Not Clinical

Examples that must remain hypothetical and non-clinical:

- converting testis-like tissue into stable ovary-like estrogen-producing tissue;
- using this for humans;
- creating safe endocrine grafts through reprogramming.

The simulator may represent these as abstract risks or future concept dashboards, but it must not present them as actionable plans.

## Not Supported

This project does not support:

- DIY application;
- self-experimentation;
- medical decisions;
- direct mouse-to-human translation;
- clinical planning;
- laboratory execution;
- gene editing design.

It also does not support:

- realistic surgical visualization;
- injection or delivery planning;
- realistic anatomy training;
- realistic blood or gore visualization;
- clinical phenotype prediction;
- human transition or treatment planning;
- reproductive prediction;
- using comparison mode to choose interventions.

## Risk List

Risks in real biological systems include:

- tumor risk;
- mosaicism;
- off-target effects;
- immune response;
- vascularization failure;
- endocrine instability;
- developmental timing mismatch;
- species differences.

These risks are listed to prevent overinterpretation. They are not presented with mitigation steps, protocols, or implementation guidance.

## Wording Rules

Use educational language:

- "testis-like";
- "ovary-like";
- "bias";
- "score";
- "pathway activity";
- "conceptual";
- "educational abstraction".
- "stylized mouse model";
- "unitless endocrine output";
- "phenotype-like model output".

Avoid language that implies clinical use, guaranteed outcomes, direct human relevance, or actionable intervention.
