# Reference Figure Integration

The simulator supports viewing real scientific reference figures as image assets. A reference figure is source material, not a procedural simulation.

## Purpose

The reference figure panel should let users inspect a real paper figure while keeping clear attribution and scope boundaries.

The panel should show:

- figure image asset;
- title;
- caption;
- source link;
- zoom and pan controls;
- close button;
- scope note.

## Runtime Implementation

`ReferenceFigurePanel` controls the modal.

Loading model:

- the real figure was manually downloaded as `Docs/nihms313068f1.jpg`;
- the project copy is stored/imported as `Assets/Art/ReferenceFigures/Matson2011_Figure1.png`;
- the figure PNG is stored at `Assets/Art/ReferenceFigures/Matson2011_Figure1.png`;
- the modal uses a Unity UI `RawImage`, which is appropriate for large scientific figures;
- `ReferenceFigurePanel` loads the local PNG bytes at runtime and assigns the resulting `Texture2D` to the `RawImage`;
- title, caption, citation text, guide text, and source URL are metadata fields;
- the source link opens with `Application.OpenURL`;
- `FigureZoomPanView` provides scroll-wheel zoom and drag panning for the figure image.

The C# viewer is responsible for modal behavior and metadata display. It must not procedurally recreate the exact paper figure.

## Modal Layout

The modal uses a simple two-column layout:

- left side: the real reference figure image;
- right side: short caption, citation text, source link, and a concise reading guide;
- controls: `Open Source Link`, `How to Read This Figure`, and `Close`.

If the PNG is missing, the figure area must show:

```text
Reference figure image not found. Expected path: Assets/Art/ReferenceFigures/Matson2011_Figure1.png
```

The source link, citation, and reading guide should remain visible even when the local image is missing.

Default citation:

```text
Matson CK et al. (2011). DMRT1 prevents female reprogramming in the postnatal mammalian testis.
```

Default source URL:

```text
https://pmc.ncbi.nlm.nih.gov/articles/PMC3150961/
```

## Required Distinction

The real figure is reference material. It should not be recreated procedurally.

The dynamic marker-pattern panel is a separate educational abstraction generated in C#.

## License and Repository Caution

Real paper figures are reference material. Before committing them to a public repository, verify the article/figure license and attribution requirements.

If redistribution is not allowed, keep `Matson2011_Figure1.png` local and commit only:

- the source link;
- citation text;
- instructions for manually adding the figure at `Assets/Art/ReferenceFigures/Matson2011_Figure1.png`.

## Scope Text

Use labels such as:

```text
Real paper figure shown as reference material.
Mouse-model literature only.
Not medical or laboratory guidance.
```

Do not present the figure as a diagnostic tool, protocol, or human-application guide.
