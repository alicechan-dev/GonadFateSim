# Roadmap

This roadmap keeps the project focused on educational simulation. Each milestone should preserve the same safety boundary: no wet-lab protocols, no gene editing instructions, no clinical recommendations, and no direct human application guidance.

## Phase 1 — Current Dashboard

Includes:

- SRY;
- SOX9;
- WNT/β-catenin;
- simple pathway competition;
- Unity UI sliders;
- simple output.

Goal:

```text
SRY timing/strength
↓
SOX9 activation
↓
pathway competition
↓
gonad fate output
```

## Phase 2 — 3D Gonad Model Using Unity Primitives

Includes:

- `Gonad3DView`;
- `Gonad3DFactory`;
- neutral, testis-like, ovary-like, ovotestis/mixed, and unstable primitive shapes;
- connection to `GonadState.Fate`;
- conservative floating labels.

Goal:

```text
GonadState.Fate
↓
stylized primitive gonad model
```

Status:

- first primitive-based `Gonad3DView` implementation exists;
- runtime dashboard now includes a 3D gonad render panel;
- full mouse body, bloodstream, hormone particles, and orbit controls remain future work.

## Phase 3 — Transparent Mouse Body and Highlighted Gonad Region

Includes:

- `MouseBody3DView`;
- transparent low-poly or primitive mouse body;
- highlighted internal gonad region;
- camera focus target for gonad inspection;
- body/gonad layer toggles.

Goal:

```text
3D mouse context
↓
highlighted gonad state
```

## Phase 4 — Bloodstream and Hormone Flow Visualization

Includes:

- `EndocrineFlow3DView`;
- `HormoneParticleController`;
- abstract flow paths;
- estrogen-like and testosterone-like output particles;
- unitless score labels.

Goal:

```text
gonad state
↓
abstract endocrine flow
```

## Phase 5 — Camera Orbit, Layer Toggles, Labels, and Tooltips

Includes:

- `CameraOrbitController`;
- `AnatomyLayerToggle`;
- `LabelBillboard`;
- floating tooltips;
- reset camera;
- switch between 2D dashboard and 3D simulator view.

Goal:

```text
inspectable 3D educational model
↓
clear labels and safe interaction
```

## Phase 6 — Optional Blender / Low-Poly Assets

Includes:

- optional Blender-generated low-poly mouse body;
- optional low-poly gonad forms;
- polished imported materials;
- richer but still schematic animation.

3D asset polish should come only after primitive prototypes are useful and safe.

## Later Milestone — Visual Simulator Foundation

Includes:

- layer toggle foundation;
- improved result explanation;
- tooltip glossary;
- scene/view navigation;
- runtime dashboard cleanup;
- safe visual state definitions.

Goal:

```text
dashboard controls
↓
pathway graph
↓
view switching
↓
visual state foundation
```

## Later Milestone — Shader-Driven Synthetic Marker Rendering

Includes:

- `MarkerPatternShaderView`;
- `TissueShapeController`;
- `TissueRegionDescriptor`;
- `MarkerShaderParameterBinder`;
- `MarkerPatternRenderMode`;
- `SyntheticMarkerPattern.shader`;
- CPU renderer fallback for unsupported shader environments.

Goal:

```text
GonadState
↓
C# tissue descriptors and channel weights
↓
GPU procedural marker rendering
↓
smooth synthetic microscopy panel
```

Status:

- first shader prototype exists;
- RawImage shader rendering is now the preferred marker-pattern path;
- CPU `Texture2D` generation remains as fallback/debug support.

## Later Milestone — Continuous Visual Transitioning

Includes:

- `VisualStateDescriptor`;
- `VisualStateMapper`;
- continuous shader parameters for SOX9-like, FOXL2-like, DAPI-like, shape, lumen, spacing, and patchiness values;
- stable tissue descriptors that morph instead of flicker;
- leaning/mixed interpretation labels;
- optional smooth preset transitions.

Goal:

```text
Simulation scores
↓
continuous visual descriptors
↓
shader parameters
↓
smooth marker-pattern morphing
```

Tasks:

- map simulation scores to visual descriptors;
- drive shader parameters continuously;
- smooth transitions while sliders move;
- avoid visual flicker and random replacement;
- add labels such as Mostly testis-like, Leaning testis-like, Mixed / reprogramming-like, Leaning ovary-like, Mostly ovary-like, and Unstable.

The transition visualization is an educational state-space abstraction. It does not represent real-time biological tissue transformation.

## Later Milestone — 2D Gonad Anatomy and Endocrine Flow

Includes:

- stylized gonad anatomy visualization;
- undifferentiated, testis-like, ovary-like, ovotestis/mixed, and unstable visual states;
- simple endocrine arrows or flow;
- unitless androgen-like and estrogen-like output indicators;
- click-to-inspect organ explanations.

Goal:

```text
pathway scores
↓
organ-level state
↓
abstract endocrine output
```

## Later Milestone — Whole Mouse and Scenario Comparison

Includes:

- stylized whole mouse silhouette;
- highlighted gonad region;
- simplified bloodstream overlay;
- side-by-side scenario comparison;
- synchronized timeline playback.

This milestone should avoid overclaiming full-body sex phenotype. It should show educational context, not biological diagnosis.

## Later Milestone — Gonadal Maintenance / Reprogramming

Includes:

- DMRT1;
- FOXL2;
- SOX8/SOX9;
- Sertoli ↔ granulosa-like state;
- adult tissue fate maintenance.

This milestone should describe maintenance and reprogramming conceptually. It should not provide experimental methods.

## Later Milestone — External Genitalia Development

Includes:

- genital tubercle;
- androgen receptor;
- androgen level;
- SHH;
- FGF;
- WNT;
- GLI2/GLI3;
- masculinization score.

The model should remain a simplified educational visualization of signaling relationships.

## Later Milestone — Expanded Endocrine Output

Includes:

- Leydig-like cells;
- theca-like cells;
- granulosa-like cells;
- CYP19A1/aromatase;
- estradiol/testosterone output curves;
- vascularization factor;
- instability warnings.

Hormone curves should be abstract and unitless unless later documentation defines a safe educational scaling system.

## Later Milestone — Tissue Engineering Concept Dashboard

This milestone must be concept-only and not clinical.

Includes:

- vascularization risk;
- immune response;
- tumor risk;
- mosaicism;
- endocrine instability;
- species translation penalty;
- clear warning that this is not a clinical planner.

The dashboard must not include procedural steps, implementation routes, surgical planning, dosing, delivery methods, or self-experimentation guidance.
