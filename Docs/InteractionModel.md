# Interaction Model

The simulator should feel interactive and inspectable. Every interaction should help users understand the model, not imply real-world intervention.

## Sliders

Sliders control normalized model parameters:

- SRY strength;
- SRY timing;
- SOX9 sensitivity;
- WNT/beta-catenin strength;
- genetic background modifier;
- noise.

Slider changes should update all visible layers immediately.

## Preset Scenario Buttons

Preset buttons apply safe educational scenarios.

Examples:

- Normal XY;
- Sry knockout-like;
- Delayed Sry;
- High WNT;
- Mixed outcome;
- Reset.

Use "-like" and "concept" wording where appropriate. Presets should not represent lab protocols or intervention recipes.

## Hover Tooltips

Tooltips should define genes, pathways, scores, visual states, and safety boundaries.

Examples:

- "SRY: simplified upstream signal in the Level 1 model.";
- "SOX9 score: unitless testis-pathway activity.";
- "WNT/beta-catenin score: unitless ovarian-supporting pathway activity.";
- "Endocrine flow: conceptual unitless output, not a biological measurement."

## Marker Hover Inspection

The synthetic marker-pattern view supports local hover inspection.

When the pointer hovers over the generated marker image:

- a soft curved outline highlights the local inspected zone;
- a tooltip follows the pointer;
- the tooltip reports a conservative local interpretation;
- the tooltip hides when the pointer exits the image.

Supported hover categories:

- SOX9-like dominant zone;
- FOXL2-like dominant zone;
- mixed marker zone;
- nuclei-rich low-marker zone.

Hover wording should use "suggests", "marker-dominant", and "local identity interpretation". It must not imply that the simulator can definitively identify real cell types from real microscopy.

For details, see [MarkerHoverInspectionSpec.md](MarkerHoverInspectionSpec.md).

## Layer Toggles

Layer toggles let users show or hide:

- controls;
- gene network;
- gonad anatomy;
- endocrine flow;
- whole mouse view;
- timeline;
- comparison overlay.

Toggles should not change the simulation result. They should only change what is visible.

## Compare Mode

Compare mode should support two or more scenario slots.

Useful interactions:

- copy current settings into a slot;
- choose a preset for another slot;
- lock timeline playback across slots;
- show difference summaries.

Compare mode should explain model behavior, not rank scenarios as better or worse.

## Click-to-Inspect Organ

Users should be able to click a gonad visual, mouse silhouette, pathway node, or endocrine flow channel.

Inspection can show:

- current state;
- related score;
- short explanation;
- glossary definition;
- scope note if the topic is easily overinterpreted.

## Timeline Playback

Timeline controls:

- play;
- pause;
- step forward;
- step backward;
- scrub;
- reset to beginning.

Timeline playback should animate conceptual state transitions, not real developmental timing protocols.

## Reset

Reset should restore the default educational scenario and clear comparison state if requested.

Reset must be easy to find, because users should be encouraged to explore without fear of breaking the model.

## Interaction Language

Use:

- "adjust";
- "inspect";
- "compare";
- "visualize";
- "bias";
- "score";
- "educational state".

Avoid:

- "treat";
- "edit an organism";
- "apply to human";
- "protocol";
- "dose";
- "inject";
- "surgically modify".
