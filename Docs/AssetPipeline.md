# Asset Pipeline

The simulator should begin with simple, readable visuals and only move toward 3D assets when the interaction model proves useful.

The first visual priority is clarity, not realism.

## Suggested Folders

```text
Assets/
  Art/
    Sprites/
      Mouse/
      Gonads/
      Endocrine/
  Models/
    Mouse/
    Organs/
  Materials/
    Educational/
  Prefabs/
    Visualization/
  Scripts/
    Visualization/
```

## Phase 1: 2D Schematics

Use Unity UI, sprites, simple generated art, and procedural C# shapes.

Suitable assets:

- mouse silhouette;
- gonad state icons;
- pathway graph nodes;
- bloodstream flow arrows;
- timeline markers;
- score bars and overlays.

Benefits:

- fast iteration;
- easy to keep abstract;
- avoids misleading biological realism;
- works well for educational labels and overlays.

## Phase 2: Optional Low-Poly 3D Models

If the 2D simulator works well, add optional low-poly assets.

Suitable assets:

- simplified mouse body;
- stylized paired gonads;
- abstract endocrine flow tubes or particles;
- simple organ-location markers.

Rules:

- keep models non-clinical and non-procedural;
- avoid surgical or anatomical training detail;
- use low-detail geometry to communicate system relationships.

## Blender-Generated Assets

Blender scripts may be used later to generate simple models such as:

- mouse silhouette mesh;
- generic organ markers;
- abstract flow paths;
- low-poly gonad shapes.

Blender generation should focus on educational visualization assets, not biological precision or intervention planning.

## Procedural C# Visuals

Procedural Unity visuals are appropriate for:

- signal bars;
- arrows;
- timelines;
- node graphs;
- pulsing uncertainty outlines;
- endocrine flow particles;
- comparison overlays.

Procedural elements should derive from `GonadState`, pathway scores, and scenario definitions.

## Asset Naming

Use descriptive educational names:

- `Gonad_Undifferentiated_Schematic`;
- `Gonad_TestisLike_Schematic`;
- `Gonad_OvaryLike_Schematic`;
- `Mouse_Silhouette_Educational`;
- `Flow_AndrogenLike_Unitless`;
- `Flow_EstrogenLike_Unitless`.

Avoid names that imply clinical use, intervention, or real-world procedure.

## Import Guidelines

Recommended settings for Phase 1 sprites:

- transparent background;
- readable at dashboard scale;
- high contrast on dark and light UI backgrounds;
- simple shapes;
- no realistic medical imagery.

Recommended settings for Phase 2 models:

- low-poly;
- neutral materials;
- separate visual layers for body, organ highlight, and flow;
- clear scale conventions inside Unity.
