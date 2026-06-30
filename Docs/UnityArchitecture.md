# Unity Architecture

Level 1 began as a small, data-driven Unity scene. The architecture should now support a richer multi-view educational simulator while keeping the calculation transparent and inspectable.

The simulator should separate:

- model calculation;
- view state;
- UI controls;
- pathway visualization;
- anatomy visualization;
- endocrine visualization;
- 3D visualization;
- timeline playback;
- scenario comparison;
- asset loading.

## Suggested Folder Structure

```text
Assets/
  Scripts/
    Simulation/
      SexDeterminationSimulator.cs
      GeneSignal.cs
      PathwayState.cs
      GonadState.cs
      SimulationParameters.cs
    UI/
      ParameterSliderView.cs
      SimulationResultView.cs
      TimelineView.cs
      GeneNodeView.cs
      TooltipGlossaryView.cs
      ScenarioComparisonView.cs
    ReferenceFigures/
      ReferenceFigurePanel.cs
      FigureZoomPanView.cs
      FigureGuidePanel.cs
    MarkerSimulation/
      VisualStateDescriptor.cs
      VisualStateMapper.cs
      DisplayedVisualState.cs
      TargetVisualState.cs
      TransitionController.cs
      TissueCompartmentDescriptor.cs
      CompartmentMorphState.cs
      ContinuousTissueMorphController.cs
      MarkerPatternShaderView.cs
      MarkerShaderParameterBinder.cs
      MarkerPatternHoverInspector.cs
      HoverZoneInterpreter.cs
      HoverOutlineView.cs
      MarkerTooltipView.cs
  Editor/
    ReferenceFigureTextureImporter.cs
    MarkerSimulation/
      MarkerPatternView.cs
      MarkerPatternGenerator.cs
    Visualization/
      PathwayGraphView.cs
      GonadPhenotypeView.cs
      GonadVisualController.cs
      EndocrineFlowView.cs
      MousePhenotypeView.cs
      SignalBarView.cs
      ThreeD/
        MouseBody3DView.cs
        Gonad3DView.cs
        Gonad3DFactory.cs
        EndocrineFlow3DView.cs
        HormoneParticleController.cs
        CameraOrbitController.cs
        LabelBillboard.cs
        AnatomyLayerToggle.cs
        ThreeDSceneBootstrapper.cs
    Timeline/
      TimelinePlaybackController.cs
      DevelopmentStageDefinition.cs
    Data/
      GeneDefinition.cs
      PathwayDefinition.cs
      ScenarioDefinition.cs
      VisualStateDefinition.cs
    Services/
      VisualAssetLoader.cs
      ScenarioComparisonService.cs
  Art/
    Sprites/
      Mouse/
      Gonads/
      Endocrine/
  Models/
    Mouse/
    Organs/
  ScriptableObjects/
    Genes/
    Pathways/
    Scenarios/
    VisualStates/
  Scenes/
    SexDeterminationScene.unity
  Docs/
```

## Core Classes

### SexDeterminationSimulator

Main calculation class.

Responsibilities:

- receives `SimulationParameters`;
- calculates pathway scores;
- calculates probabilistic outcomes;
- returns `GonadState`;
- keeps the model readable and inspectable for educational use.

This class should not know about sliders, buttons, scene objects, or Unity UI layout.

### SimulationParameters

Container for user-controlled parameters:

```csharp
public float SryStrength;
public float SryTiming;
public float Sox9Sensitivity;
public float WntBetaCateninStrength;
public float GeneticBackgroundModifier;
public float Noise;
```

Suggested value range for each field is `0.0f` to `1.0f` in Level 1, unless a later design needs a wider range.

### GeneSignal

Represents gene or signal activity over time:

```csharp
public string Id;
public string DisplayName;
public float CurrentLevel;
public AnimationCurve ActivityOverTime;
```

Use this for visual displays such as timeline curves, signal bars, or graph node brightness.

### PathwayState

Represents the current state of a biological pathway:

```csharp
public string PathwayName;
public float ActivityScore;
public bool IsDominant;
```

Examples of pathway names:

- `SOX9/Testis`;
- `WNT/β-catenin/Ovary`;
- `Instability`.

### GonadState

Represents the simulation output:

```csharp
public GonadFate Fate;
public float TestisScore;
public float OvaryScore;
public float OvotestisScore;
public float InstabilityScore;
public string Explanation;
```

The `Explanation` field should be short, human-readable, and suitable for display in the right panel.

### ParameterSliderView

Connects a UI slider to one field in `SimulationParameters`.

Responsibilities:

- display the current value;
- update the parameter when the user moves the slider;
- notify the scene controller that simulation output should refresh.

### SimulationResultView

Displays `GonadState`.

Responsibilities:

- show predicted outcome;
- show testis, ovary, ovotestis, and instability scores;
- show a short explanation.

### TimelineView

Displays the simplified developmental sequence:

```text
Early gonad
↓
SRY window
↓
SOX9 stabilization
↓
Pathway competition
↓
Fate output
```

### GeneNodeView

Displays one gene or pathway node in the graph. Activity can be represented by brightness, size, fill amount, or outline strength.

### PathwayGraphView

Displays the relationship:

```text
SRY → SOX9 → Testis-like fate
WNT/β-catenin → Ovary-like fate
SOX9 ┤ WNT/β-catenin
WNT/β-catenin ┤ SOX9
```

Line thickness or brightness should reflect the current pathway score.

### GonadPhenotypeView

Displays the output phenotype in a simple non-realistic educational visualization. It should avoid clinical or procedural imagery.

### SignalBarView

Displays normalized activity scores for individual genes or pathways.

## Visual Systems

### GonadVisualController

Displays the organ-level gonad state using safe schematic visuals.

Responsibilities:

- receives `GonadState`;
- selects undifferentiated, testis-like, ovary-like, ovotestis/mixed, or unstable visual state;
- updates color overlays, labels, and uncertainty markers;
- exposes click-to-inspect details for the current organ state.

This class should not calculate biological outcomes. It should only render the already-computed educational state.

### EndocrineFlowView

Displays abstract endocrine or bloodstream flow.

Responsibilities:

- maps model scores to unitless androgen-like, estrogen-like, and instability flow intensity;
- animates arrows, particles, or bars;
- labels outputs as conceptual and unitless;
- avoids medical measurement or treatment language.

### MousePhenotypeView

Displays a stylized whole-mouse context.

Responsibilities:

- shows a simplified mouse silhouette or low-detail model;
- highlights the gonad region;
- optionally overlays abstract bloodstream flow;
- presents phenotype-like educational labels without claiming full-body prediction.

### TimelinePlaybackController

Controls developmental timeline playback.

Responsibilities:

- play, pause, step, scrub, and reset timeline state;
- expose current conceptual stage;
- synchronize pathway graph, gonad visual, endocrine flow, and comparison views;
- avoid real protocol timing or experimental scheduling.

### ScenarioComparisonView

Displays two or more safe educational scenarios side by side.

Responsibilities:

- show scenario inputs, scores, gonad visuals, and endocrine abstractions;
- synchronize timeline playback across scenarios;
- summarize major differences;
- avoid ranking scenarios as better, worse, or actionable.

### TooltipGlossaryView

Shows short definitions and safety reminders.

Responsibilities:

- explain genes, pathways, visual states, and score meanings;
- label all endocrine outputs as unitless abstractions;
- provide scope warnings for mouse-model and whole-organism views.

### VisualAssetLoader

Loads sprites, materials, prefabs, or optional models for visualization layers.

Responsibilities:

- keep asset references centralized;
- support 2D schematic assets first;
- allow optional low-poly 3D assets later;
- keep fallback procedural visuals available when art assets are missing.

## Reference Figure Systems

### ReferenceFigurePanel

Displays a real scientific reference figure as a Unity image asset.

Responsibilities:

- load the local reference figure texture from `Assets/Art/ReferenceFigures/Matson2011_Figure1.png`;
- show the reference figure in the modal `RawImage`;
- show title, caption, citation text, source link, and concise reading guide;
- support close behavior;
- expose source URL opening;
- label the figure as reference material.

This class should not generate simulated marker patterns.

### ReferenceFigureTextureImporter

Editor-only importer that applies stable texture import settings for the Matson 2011 reference PNG.

Responsibilities:

- detect `Assets/Art/ReferenceFigures/Matson2011_Figure1.png`;
- keep the texture import suitable for a UI `RawImage`;
- keep mipmaps disabled for a crisp reference figure;
- leave biological interpretation and viewer behavior to runtime scripts.

### FigureZoomPanView

Provides zoom and pan behavior for the displayed reference figure.

Responsibilities:

- handle scroll-wheel zoom;
- handle drag panning;
- reset view when the modal opens.

### FigureGuidePanel

Displays the marker reading guide.

Responsibilities:

- explain SOX9 green, FOXL2 red, and DAPI blue;
- explain row/column comparison;
- explain green-dominant, red-dominant, and mixed pattern interpretation;
- include the educational/non-medical scope boundary.

## Marker Simulation Systems

### VisualStateDescriptor

Continuous visual state generated from simulation scores.

Responsibilities:

- store testis bias, ovary bias, mixedness, and instability;
- store SOX9-like, FOXL2-like, and DAPI-like signal values;
- store shape descriptors such as elongation, roundness, connectedness, fragmentation, lumen size, lumen softness, compartment spacing, and marker patchiness;
- provide shader-friendly values that can morph smoothly between named outcomes.

This class is a rendering descriptor, not a biological measurement.

### VisualStateMapper

Maps `GonadState` scores to `VisualStateDescriptor`.

Responsibilities:

- convert high `TestisScore` into higher testis bias, elongation, SOX9-like signal, and lumen structure;
- convert high `OvaryScore` into higher ovary bias, roundness, and FOXL2-like signal;
- convert high `OvotestisScore` into mixedness and partial compartmentalization;
- convert high `InstabilityScore` into fragmentation, patchiness, and weaker organization;
- keep mapping continuous so small score changes produce small visual changes.

### TissueCompartmentDescriptor

Persistent compartment geometry descriptor.

Responsibilities:

- store normalized center, base radius, orientation, lumen offset, and stable seed data;
- provide stable compartment identity while visual parameters morph;
- avoid replacing the entire tissue field during tiny slider changes.

### CompartmentMorphState

Runtime geometry state derived from a persistent compartment and the displayed visual descriptor.

Responsibilities:

- hold current center, radius, and lumen scale;
- allow shape changes without changing stable compartment identity;
- support gradual morphing of local tissue architecture.

### DisplayedVisualState

The visual descriptor currently shown to the user.

Responsibilities:

- hold the smoothed descriptor that drives shader parameters;
- update gradually toward `TargetVisualState`;
- prevent abrupt visual replacement when interpretation labels change.

### TargetVisualState

The newest visual descriptor produced by `VisualStateMapper`.

Responsibilities:

- represent the latest simulation-derived target;
- remain separate from displayed state so visual transitions can unfold over time.

### TransitionController

Moves `DisplayedVisualState` toward `TargetVisualState`.

Responsibilities:

- smooth SOX9-like, FOXL2-like, DAPI-like, and patchiness values quickly;
- smooth elongation, roundness, lumen size, and lumen softness at medium speed;
- smooth connectedness, fragmentation, spacing, and architecture bias slowly;
- preserve the sense of a sequential educational transition.

### ContinuousTissueMorphController

Maintains persistent tissue descriptors and applies continuous morph values.

Responsibilities:

- preserve stable tissue structure while sliders move;
- interpolate geometry and shader parameters;
- regenerate major layout only for intentional scenario changes or reset;
- prevent flicker.

### MarkerPatternShaderView

Shader-first marker-pattern view.

Responsibilities:

- create the marker `RawImage`;
- assign the `SyntheticMarkerPattern` shader material;
- update shader parameters from `GonadState`;
- keep legend and interpretation text visible;
- use CPU fallback only when shader mode is unavailable.

### MarkerPatternHoverInspector

Pointer inspection layer for the synthetic marker view.

Responsibilities:

- listen for pointer enter, move, and exit events on the marker `RawImage`;
- convert pointer position into normalized marker-image coordinates;
- request a conservative hover-zone classification;
- show and hide the soft outline and tooltip views;
- keep hover behavior attached to the displayed synthetic image rather than the reference figure.

### HoverZoneInterpreter

Classifies the local synthetic marker zone.

Responsibilities:

- estimate local SOX9-like, FOXL2-like, and DAPI-like values from pointer UV and the current displayed `VisualStateDescriptor`;
- classify zones as SOX9-like dominant, FOXL2-like dominant, mixed marker, or nuclei-rich low-marker;
- return conservative tooltip wording;
- avoid claims of real microscopy analysis or definitive cell-type identification.

Current implementation estimates local values from the smoothed visual descriptor. A future shader-generated zone map or ID buffer can replace this approximation.

### HoverOutlineView

Displays the local inspection highlight.

Responsibilities:

- draw a soft curved outline around the hovered area;
- color the outline by hover category;
- avoid harsh rectangular selection boxes;
- hide when the pointer exits the marker view.

### MarkerTooltipView

Displays the hover interpretation panel.

Responsibilities:

- follow the pointer without leaving the visible modal/panel bounds;
- show zone name, interpretation, unitless marker readouts, and scope note;
- use conservative wording such as "suggests" and "simulated interpretation";
- hide when the pointer exits the marker view.

### TissueShapeController

Maintains stable tissue-region descriptors for shader rendering.

Responsibilities:

- regenerate descriptors when `GonadFate` changes;
- preserve stable random seeds per fate;
- provide tissue and gap regions to the shader binder.

### TissueRegionDescriptor

Compact normalized tissue region data.

Responsibilities:

- store region center and radius;
- convert to shader vectors.

### MarkerShaderParameterBinder

Binds model output to shader material properties.

Responsibilities:

- bind seed, density, irregularity, DAPI-like intensity, SOX9-like intensity, and FOXL2-like intensity;
- bind tissue regions and gap regions;
- bind `VisualStateDescriptor` values as continuous shader parameters;
- prefer displayed visual state over direct target state when smooth transitions are active;
- avoid per-dot CPU rendering.

### MarkerPatternRenderMode

Selects marker rendering mode:

- shader mode for normal use;
- CPU fallback for debugging or unsupported shader environments.

### SyntheticMarkerPattern Shader

Located at:

```text
Assets/Shaders/SyntheticMarkerPattern.shader
```

Responsibilities:

- render dark fluorescence background;
- render procedural tissue masks;
- render dense nuclei-like blue dots inside masks;
- render preliminary SOX9-like and FOXL2-like channels;
- composite a soft merged fluorescence-style output.

### MarkerPatternGenerator

Current lightweight generator for schematic marker elements from `GonadState.Fate`.

Responsibilities:

- produce blue DAPI-like nuclei;
- produce green SOX9-positive dots;
- produce red FOXL2-positive dots;
- make TestisLike green-dominant, OvaryLike red-dominant, Ovotestis mixed, and Unstable weak/mixed.

This class is now legacy/fallback context. It must not recreate a real microscopy figure.

### MarkerPatternView

Earlier CPU display path for generated marker textures.

Responsibilities:

- clear previous marker objects;
- create small colored UI dots;
- update whenever simulation state changes;
- label the output as simulated and educational.

Current/fallback responsibility:

- display a generated `Texture2D` or `RenderTexture` in a `RawImage`;
- keep marker legend and interpretation text;
- avoid per-cell GameObject creation for dense microscopy-style output.

### TissueMaskGenerator

Future system that creates irregular synthetic tissue regions.

Responsibilities:

- constrain nuclei and marker placement to tissue masks;
- create empty/background gaps;
- vary structural feel by `GonadFate`;
- avoid copying real paper figure tissue shapes.

### NucleiFieldGenerator

Future system that generates dense DAPI-like nuclei.

Responsibilities:

- place hundreds to thousands of nuclei inside masks;
- vary nucleus size, shape, orientation, and intensity;
- use regional density and clustering.

### MarkerChannelGenerator

Future system that generates SOX9-like and FOXL2-like channel fields.

Responsibilities:

- create green-dominant TestisLike regions;
- create red-dominant OvaryLike regions;
- create mixed or compartmentalized Ovotestis regions;
- create weak patchy Unstable regions.

### FluorescenceCompositeRenderer

Future system that renders the synthetic micrograph.

Responsibilities:

- composite blue, green, and red channels over a dark background;
- draw soft spots and blended regions;
- output a `Texture2D` or `RenderTexture`;
- keep the output educational and synthetic.

## 3D Visualization Systems

The 3D systems are view components. They receive `GonadState` and related display data from the simulation/controller layer. They must not contain independent biological calculation logic.

### MouseBody3DView

Displays a simplified transparent mouse body.

Responsibilities:

- create or show a transparent primitive/low-poly mouse silhouette;
- expose a highlighted internal gonad region;
- provide anchor points for labels, flow paths, and camera focus;
- keep the body stylized and non-clinical.

### Gonad3DView

Displays the current 3D gonad model.

Responsibilities:

- receive `GonadState`;
- request a model from `Gonad3DFactory`;
- update material, label, and inspectable state;
- support click-to-inspect behavior;
- render `TestisLike`, `OvaryLike`, `Ovotestis`, and `Unstable` states conservatively.

Current first-slice implementation:

- exposes `UpdateFromState(GonadState state)`;
- clears previous primitive child objects;
- rebuilds the schematic model from `GonadState.Fate`;
- assigns primitive children to the `Gonad3D` layer;
- is rendered into a dedicated `3D Gonad View` panel by an orthographic camera, `RenderTexture`, and UI `RawImage`;
- leaves current-state text in regular UI outside the render texture.

### Gonad3DFactory

Builds stylized gonad forms from Unity primitives or later asset prefabs.

Responsibilities:

- create a neutral oval for undetermined state;
- create two lobes or cord-like shapes for testis-like state;
- create a rounded organ with follicle-like spheres for ovary-like state;
- create split mixed geometry for ovotestis state;
- create a neutral irregular warning-labeled organ for unstable state;
- avoid realistic surgical or clinical anatomy.

The current implementation uses only Unity primitives and runtime materials.

### 3D Viewport Isolation

The first 3D viewport uses:

- a dedicated 3D scene root;
- `Gonad3D` layer assignment for primitive model objects;
- a dedicated orthographic camera with culling mask set to `Gonad3D`;
- `RenderTexture` output;
- UI `RawImage` display;
- UI labels outside the render texture.

This prevents the 3D camera from rendering dashboard text or unrelated scene objects.

### EndocrineFlow3DView

Displays abstract bloodstream/endocrine flow paths.

Responsibilities:

- create tubes, arcs, or line renderers;
- map unitless output scores to visual intensity;
- coordinate with `HormoneParticleController`;
- label output as educational and not measured concentration.

### HormoneParticleController

Controls moving hormone-output particles.

Responsibilities:

- spawn and animate small particles along flow lines;
- represent estrogen-like and testosterone-like output scores;
- pause, play, and reset visual flow;
- avoid implying real hormone concentration or treatment targets.

### CameraOrbitController

Controls the 3D simulator camera.

Responsibilities:

- orbit around mouse or gonad target;
- zoom in and out;
- clamp camera distance and vertical angle;
- reset camera;
- optionally focus on the gonad region.

### LabelBillboard

Keeps floating labels facing the camera.

Responsibilities:

- orient labels toward the active camera;
- anchor labels to body, gonad, flow, and particle regions;
- support tooltip or inspector triggers.

### AnatomyLayerToggle

Controls visibility of 3D visualization layers.

Responsibilities:

- toggle body, gonad, bloodstream, hormones, labels, and gene network overlay;
- preserve simulation state;
- broadcast visibility changes to relevant view components.

### ThreeDSceneBootstrapper

Creates or wires the 3D simulator view.

Responsibilities:

- instantiate primitive prototype objects for Phase 1;
- connect 3D views to `SimulationController`;
- create default materials and labels;
- provide safe fallback visuals if imported assets are missing.
