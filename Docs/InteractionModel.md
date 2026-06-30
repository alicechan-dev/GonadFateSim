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

Reset should restore the default / Normal XY-style educational scenario and clear comparison state if requested.

Reset must be easy to find, because users should be encouraged to explore without fear of breaking the model.

Reset must not quit the app, remove reference/source links, or close documentation panels unexpectedly.

## Global Exit and Modal Handling

The simulator should provide an explicit `Exit` button and keyboard escape behavior.

`Esc` priority:

```text
Esc pressed
↓
if any modal is open:
    close topmost modal
else:
    show Exit Simulator? confirmation dialog
```

The exit dialog should include:

- title: `Exit Simulator?`;
- message: `Are you sure you want to exit? No simulation data is saved.`;
- buttons: `Cancel` and `Exit`.

In standalone builds, confirmed exit calls `Application.Quit()`. In the Unity Editor, confirmed exit should stop Play Mode if possible.

All modals should register with a common modal manager so Reference Figure, Figure Reading Guide, Synthetic Marker Pattern, Help/About, and Exit confirmation close consistently.

## Help / About

The Help/About panel should be accessible from the main dashboard and explain:

- this is an educational Unity simulator for mouse developmental biology;
- gene/pathway activity, gonadal fate, and marker-pattern interpretation are simplified visual relationships;
- the app is not a medical tool;
- the app is not a laboratory protocol;
- the app does not provide gene editing, CRISPR, animal procedure, surgery, injection, or human-application guidance;
- synthetic marker patterns are educational abstractions, not real microscopy;
- mouse-model findings must not be treated as direct human clinical guidance.

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
