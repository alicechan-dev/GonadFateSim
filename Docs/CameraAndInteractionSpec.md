# Camera and Interaction Spec

The 3D simulator should be inspectable without feeling like a medical planning tool. Camera and interaction features should support educational exploration of the model state.

## Camera Orbit

Users should be able to orbit around the mouse or gonad model.

Controls:

- drag to orbit;
- optional right-drag or middle-drag to pan;
- smooth damping;
- clamp vertical angle to avoid disorienting flips;
- orbit target can switch between whole mouse and gonad region.

## Zoom

Users should be able to zoom in and out.

Controls:

- mouse wheel or pinch gesture;
- min and max distance limits;
- reset camera button;
- optional focus-on-gonad button.

Zoom should help inspect labels and states, not expose realistic anatomy.

## Click-to-Inspect Gonad

Clicking the 3D gonad should open or update an inspector panel.

Inspector content:

- current `GonadState.Fate`;
- pathway scores;
- short explanation;
- safe visual-state definition;
- scope reminder.

## Layer Toggles

Users should be able to toggle:

- body;
- gonad;
- bloodstream;
- hormones;
- labels;
- gene network overlay.

Layer toggles should change visibility only. They should not change the simulation output.

## Hormone Flow Playback

Controls:

- pause hormone flow;
- play hormone flow;
- reset particles;
- optionally slow down or speed up visual playback.

Playback controls affect visualization only. They should not change `GonadState` or model scores.

## Labels and Tooltips

Floating labels should face the camera.

Useful labels:

- "Transparent mouse body";
- "Highlighted gonad region";
- "Estrogen-like output score";
- "Testosterone-like output score";
- "Educational abstraction, not measured concentration";
- "Unitless endocrine output".

Tooltips should be concise and conservative.

## 2D / 3D View Switching

The simulator should allow switching between:

- 2D dashboard view;
- 3D simulator view;
- split dashboard + 3D view if screen space allows.

Switching views should preserve the same current scenario and `GonadState`.

## Reset Camera

Reset should restore:

- default orbit angle;
- default zoom distance;
- default target;
- default layer visibility if requested.

Reset should not change model parameters unless the user selects a full scenario reset.
