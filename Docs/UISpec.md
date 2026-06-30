# UI Spec

## Scene: SexDeterminationScene

The first Unity scene can begin as a dashboard, but it should evolve into a tabbed or multi-view simulator. The dashboard remains one view among several:

- Dashboard / Control View;
- Gene Network View;
- Gonad View;
- Endocrine View;
- Whole Mouse View;
- Timeline View;
- Compare Scenarios View.

For details, see [SceneAndViewDesign.md](SceneAndViewDesign.md).

## Dashboard Layout

The dashboard view should be split into four zones:

- left panel: parameters;
- center panel: pathway graph;
- right panel: output;
- bottom panel: timeline.

The UI should feel like a simulator control surface, not a medical tool.

## Left Panel: Parameters

Sliders:

- SRY strength;
- SRY timing;
- SOX9 sensitivity;
- WNT/β-catenin strength;
- Genetic background modifier;
- Noise.

Scenario buttons:

- Normal XY;
- Sry knockout-like;
- Delayed Sry;
- High WNT;
- Mixed outcome;
- Reset.

Each slider should show a normalized value from `0.0` to `1.0`. Scenario buttons should apply safe educational presets only.

## Center Panel: Pathway Graph

Visual graph:

```text
SRY → SOX9 → Testis-like fate
WNT/β-catenin → Ovary-like fate
SOX9 ┤ WNT/β-catenin
WNT/β-catenin ┤ SOX9
```

Line thickness or brightness should represent activity score.

Nodes should show readable labels and normalized activity. The graph should update immediately when parameters change.

## Right Panel: Output

Show:

```text
Predicted outcome: Testis-like / Ovary-like / Ovotestis / Unstable
Testis score
Ovary score
Instability score
Short explanation
```

Example explanation:

```text
SRY signal was strong and occurred inside the critical window, allowing SOX9 to dominate over WNT/β-catenin.
```

Alternative explanation:

```text
SRY signal was absent or too weak, so the ovarian-supporting WNT/β-catenin pathway dominated.
```

The right panel should also include a small education-only reminder:

```text
Educational abstraction. Not a medical or laboratory tool.
```

## Bottom Panel: Timeline

Show a simplified developmental timeline:

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

The current step can be highlighted based on the selected parameter focus. For example, changing SRY timing can highlight the SRY window.

## Interaction Notes

The UI should respond instantly to parameter changes. No user action should imply real-world intervention, treatment, editing, or experiment planning.

Labels should use model language such as "pathway strength", "timing", "score", "bias", and "educational outcome".

## Layer Toggles

The simulator should include toggles for:

- controls;
- pathway graph;
- gonad anatomy;
- endocrine flow;
- whole mouse context;
- timeline;
- comparison overlay.

Toggles change visibility only. They should not change model output.

## Multi-View Navigation

Navigation may use tabs, segmented buttons, or a side rail. The first screen should remain usable as a simulator, not a landing page.

Each view should keep the current scenario synchronized. A change in one view should update all other views.

## 3D Gonad View Panel

The first 3D slice should appear as a dedicated panel titled:

```text
3D Gonad View
```

Subtitle:

```text
Stylized mouse-model educational schematic
```

The panel should contain:

- a UI `RawImage` showing the `RenderTexture` from the dedicated 3D gonad camera;
- normal UI text such as `Current 3D state: Ovary-like`;
- safety text: `Not medical or laboratory guidance.`

The 3D camera should render only the `Gonad3D` layer. It should not render dashboard UI text or unrelated scene objects.

## Scenario Comparison

Compare mode should allow at least two scenario slots.

Each slot should show:

- selected preset or current custom settings;
- pathway scores;
- gonad visual state;
- abstract endocrine output;
- concise difference summary.

Comparison should explain model behavior and must not imply experiment planning.

## Reference Figure and Marker Pattern

The dashboard should include:

- `View Reference Figure` button;
- `How to Read This Figure` button;
- `Simulated Marker Pattern` panel.

The reference figure button opens a modal with a real figure image asset loaded from `Assets/Art/ReferenceFigures/Matson2011_Figure1.png`. The source image was manually downloaded as `Docs/nihms313068f1.jpg` and copied/imported into the Unity art folder. The modal should use a clean two-column layout:

- left side: the actual figure image in a zoomable/pannable UI `RawImage`;
- right side: short caption, citation text, source link, and a concise reading guide;
- buttons: `Open Source Link`, `How to Read This Figure`, and `Close`.

The reference figure is asset-based source material. It must not be procedurally recreated in C#.

If the PNG is missing, the figure area should show this message instead of a blank placeholder:

```text
Reference figure image not found. Expected path: Assets/Art/ReferenceFigures/Matson2011_Figure1.png
```

Before committing real paper figures to a public repository, verify the article or figure license and attribution requirements. If redistribution is not allowed, keep the image local and provide only the source link plus manual placement instructions.

The reading guide button opens a modal explaining:

- SOX9 = green;
- FOXL2 = red;
- DAPI = blue;
- how to compare rows and columns;
- how to interpret green-dominant, red-dominant, or mixed patterns.

The simulated marker pattern is generated in C# from `GonadState.Fate`. It should show blue nucleus-like dots plus green SOX9 and red FOXL2 marker dots. It must be labeled as an educational abstraction, not a real micrograph.
