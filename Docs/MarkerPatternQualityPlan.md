# Marker Pattern Quality Plan

The goal is a high-quality synthetic educational micrograph, not a sparse marker schematic.

## Quality Targets

The generated marker panel should show:

- hundreds to thousands of nucleus-like elements;
- high-density tissue regions;
- dark fluorescence background;
- soft channel blending;
- empty background or lumen-like gaps;
- regional clustering instead of uniform random scatter;
- clear state-specific visual differences.
- smooth changes when parameters move between states.

## Minimum Acceptable Upgrade

The first renderer upgrade should:

- render through a shader material on a UI `RawImage`;
- expose nuclei density and seed as material parameters;
- compute tissue masks procedurally;
- render blue nuclei-like marks inside tissue masks;
- keep the output stable while sliders move;
- preserve the current legend and interpretation line.

## Better Target

A stronger renderer should:

- use shader hash/noise or Voronoi-like pseudo-cell placement;
- vary nucleus scale, spacing, and brightness in the fragment shader;
- use multiple C# tissue descriptors with gap descriptors;
- create channel regions using shader noise fields;
- create compartmentalized `Ovotestis` patterns through parameterized channel weighting;
- create weak patchy `Unstable` patterns through lower density and higher irregularity.
- map continuous simulation scores into visual descriptors;
- morph geometry and channel weights rather than swapping fixed images;
- keep compartment identity stable during slider motion.

## Performance Quality Target

Slider motion should update material parameters smoothly.

Avoid:

- CPU `SetPixel` loops during normal interaction;
- `Texture2D.Apply` on every slider tick;
- thousands of UI GameObjects;
- regenerated random layouts during tiny slider changes;
- complete tissue replacement while the user drags a slider;
- visible flicker when continuous scores change.

Accept:

- CPU fallback mode for debugging or unsupported shader environments;
- regenerating compact tissue descriptors when `GonadFate` changes.

## Visual QA Checklist

Before considering the renderer done, verify:

- the output does not look like random confetti;
- the tissue has coherent shape and density;
- blue nuclei are visible but not uniformly scattered everywhere;
- green and red signals follow state-specific regional patterns;
- TestisLike, OvaryLike, Ovotestis, and Unstable are visually distinct;
- intermediate score values visibly produce intermediate marker patterns;
- small slider changes produce small visual changes;
- presets do not permanently destabilize the random seed or tissue layout;
- the label says the output is synthetic and educational;
- no wet-lab or medical interpretation is implied.

## Reference Use Checklist

The Matson 2011 figure may guide:

- fluorescence channel feel;
- dark background;
- dense nuclear texture;
- tissue-region organization;
- soft signal blending.

It must not be used to duplicate:

- panel grids;
- exact microscopy crops;
- arrows, insets, labels, or scale bars;
- exact cell positions;
- paper-specific experimental layout.
