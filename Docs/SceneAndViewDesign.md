# Scene and View Design

The simulator should support multiple views or tabs. Each view should share the same underlying simulation state while emphasizing a different layer of explanation.

## Dashboard / Control View

Purpose:

- provide direct access to model inputs;
- show the current predicted outcome;
- keep the original dashboard workflow available.

Key UI elements:

- sliders for SRY strength, SRY timing, SOX9 sensitivity, WNT/beta-catenin strength, genetic background modifier, and noise;
- preset scenario buttons;
- reset button;
- compact score bars;
- education-only scope reminder.

User interactions:

- adjust sliders;
- apply presets;
- reset;
- toggle visual layers.

Expected outputs:

- updated pathway scores;
- updated fate label;
- short explanation;
- synchronized updates in other views.

## Gene Network View

Purpose:

- explain how the simplified gene/pathway model produces scores.

Key UI elements:

- pathway graph nodes;
- arrows and inhibition links;
- signal bars;
- selected-node inspector;
- glossary tooltip panel.

User interactions:

- hover over genes/pathways for definitions;
- click a node to inspect its role;
- toggle labels and score overlays.

Expected outputs:

- visible SRY, SOX9, and WNT/beta-catenin activity;
- dominance or mixed-state indication;
- explanation of why the current outcome was selected.

## Gonad View

Purpose:

- show organ-level meaning for the current gonad fate state.

Key UI elements:

- stylized gonad diagram;
- state label;
- tissue-pattern overlays;
- uncertainty or mixed-state marker;
- small pathway score summary.

User interactions:

- click-to-inspect organ regions;
- switch between current scenario and comparison scenario;
- toggle simplified annotations.

Expected outputs:

- undifferentiated, testis-like, ovary-like, ovotestis/mixed, or unstable state;
- visual link between pathway scores and organ-level state;
- conservative educational description.

## Endocrine View

Purpose:

- show abstract system-level output from the gonad state.

Key UI elements:

- bloodstream or circulation schematic;
- androgen-like and estrogen-like unitless flow indicators;
- endocrine instability warning band;
- timeline-linked flow animation.

User interactions:

- play or pause flow animation;
- hover over output channels;
- compare current and preset scenarios.

Expected outputs:

- conceptual hormone-like output curves or bars;
- flow intensity linked to gonad state;
- reminder that outputs are unitless educational abstractions.

## Whole Mouse View

Purpose:

- place the gonad and endocrine output into a simplified mouse-level context.

Key UI elements:

- stylized mouse silhouette;
- highlighted gonad location;
- optional bloodstream overlay;
- phenotype-like educational label;
- scope warning.

User interactions:

- toggle organ and endocrine overlays;
- click highlighted systems;
- compare two scenarios side by side.

Expected outputs:

- simplified organism-level visualization;
- conservative label such as "testis-like gonad bias" or "mixed gonad state";
- no claim of full-body sex phenotype prediction.

## 3D Simulator View

Purpose:

- provide a spatial, stylized visualization of the mouse body, internal gonad region, and endocrine flow.

Key UI elements:

- transparent mouse body;
- highlighted gonad region;
- `GonadState.Fate` driven 3D gonad model;
- abstract bloodstream arcs or tubes;
- hormone-output particles;
- floating labels;
- orbit/zoom camera controls;
- layer toggles.

User interactions:

- orbit camera;
- zoom in and out;
- click gonad to inspect state;
- toggle body, gonad, bloodstream, hormones, labels, and gene network overlay;
- pause or play hormone flow;
- reset camera;
- switch between 2D dashboard and 3D simulator view.

Expected outputs:

- stylized 3D organ-level state;
- visible endocrine output as unitless particles or flow lines;
- educational labels and tooltips;
- no realistic surgery, clinical claims, or human-application framing.

## Timeline View

Purpose:

- connect parameters and outcomes to a simplified developmental sequence.

Key UI elements:

- stage markers;
- play/pause button;
- scrubber;
- current-stage details;
- pathway score traces.

User interactions:

- play, pause, step, or scrub;
- select a stage;
- inspect which parameters influence the selected stage.

Expected outputs:

- animated or stepped model progression;
- stage-specific pathway activity;
- clear link between SRY timing and downstream fate bias.

## Compare Scenarios View

Purpose:

- help users understand how different safe presets change model outputs.

Key UI elements:

- scenario slots;
- side-by-side score bars;
- side-by-side gonad visuals;
- difference summary;
- reset comparison button.

User interactions:

- choose scenarios;
- duplicate current settings into a comparison slot;
- inspect differences;
- toggle synchronized timeline playback.

Expected outputs:

- visible differences in inputs, scores, anatomy state, and endocrine abstraction;
- concise explanation of key changed variables;
- no procedural or intervention guidance.
