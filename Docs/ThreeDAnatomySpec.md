# 3D Anatomy Spec

The 3D anatomy layer must be stylized, schematic, and conservative. It should visualize model states without implying medical accuracy, surgical training, experimental reproducibility, or human application.

All 3D anatomy states are educational views of `GonadState.Fate`.

## Current Primitive Implementation

The first implementation uses Unity primitives only:

- capsules and spheres for neutral and lobe-like states;
- spheres for follicle-like educational markers;
- a split primitive composition for mixed state;
- a muted irregular primitive composition for unstable state;
- a dedicated orthographic camera;
- a `RenderTexture` shown through a UI `RawImage`;
- a dedicated `Gonad3D` layer so the 3D camera does not render dashboard UI;
- normal UI text outside the viewport for the current state label.

These primitives are placeholders for educational visualization. They are not realistic anatomy.

## GonadFate: Undetermined

Use when the timeline or visual state is before a clear fate output, or when a neutral placeholder is needed.

Visual:

- small neutral oval organ;
- smooth primitive shape;
- low saturation gray or blue-gray material;
- minimal internal structure.

Label:

- "Undetermined gonad state";
- "Early educational visualization";
- "Pathway bias not resolved".

## GonadFate: TestisLike

Visual:

- two smooth lobes;
- optional cord-like capsule or cylinder forms;
- teal or green educational highlight;
- symmetrical or paired internal forms.

Label:

- "Testis-like gonad state";
- "SOX9/testis pathway dominant";
- "Educational model output".

Avoid:

- claims of complete testis anatomy;
- fertility or function claims;
- clinical or human translation language.

## GonadFate: OvaryLike

Visual:

- rounded organ body;
- small follicle-like spheres on or inside the organ;
- rose, amber, or magenta educational highlight;
- soft clustered forms.

Label:

- "Ovary-like gonad state";
- "WNT/beta-catenin pathway dominant";
- "Educational model output".

Avoid:

- claims of complete ovarian function;
- reproductive prediction;
- realistic follicle anatomy.

## GonadFate: Ovotestis

Visual:

- mixed split organ;
- one side testis-like with lobes or cords;
- one side ovary-like with small follicle-like spheres;
- dual-color material or split highlight;
- visible mixed-state label.

Label:

- "Ovotestis / mixed model state";
- "Both pathway scores remain active";
- "Conceptual mouse-model-inspired visualization".

Avoid:

- implying an engineered target;
- suggesting experimental methods;
- describing a real individual outcome.

## GonadFate: Unstable

Visual:

- neutral irregular organ;
- uneven or pulsing outline;
- muted warning color such as yellow-gray;
- small warning-style label or icon.

Label:

- "Unstable educational state";
- "No strong pathway dominance";
- "Ambiguous model output".

Avoid:

- diagnostic wording;
- clinical failure language;
- procedural interpretation.

## Mouse Body

The mouse body should be a simplified transparent low-poly or primitive-based silhouette.

Suggested primitive approach:

- capsule or ellipsoid torso;
- small sphere head;
- tapered capsule tail;
- simple limb markers if useful;
- transparent material around 15-35% opacity;
- highlighted internal gonad region.

The body should provide spatial context only. It should not be realistic anatomy.

## Bloodstream

Bloodstream visualization should be abstract.

Allowed visuals:

- transparent tubes;
- arcs;
- line renderers;
- glowing paths;
- simple flow direction arrows.

Not allowed:

- realistic blood;
- gore;
- vascular surgery detail;
- injection or delivery-route implication.

## Hormone Particles

Hormone particles should be small moving particles or dots along flow lines.

Labels:

- "Estrogen-like output score";
- "Testosterone-like output score";
- "Educational abstraction, not measured concentration";
- "Unitless endocrine output".

Particles should communicate relative output intensity only. They should not imply real hormone concentrations, treatment targets, or medical measurements.
