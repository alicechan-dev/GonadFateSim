# 3D Asset Pipeline

The 3D simulator should start with Unity primitives and procedural C# objects. Do not start with realistic models.

The first goal is to prove that `GonadState.Fate` can drive clear, safe, stylized 3D educational states.

## Phase 1: Unity Primitives and Procedural Objects

Use:

- spheres;
- capsules;
- cylinders;
- cubes where useful for labels or markers;
- line renderers;
- particle systems;
- transparent materials.

Recommended first assets:

- primitive transparent mouse body;
- primitive gonad states;
- simple highlight ring around gonad region;
- endocrine flow lines;
- small hormone particles;
- floating text labels.

Benefits:

- fast iteration;
- no external art dependency;
- low risk of over-realism;
- easy to regenerate from `GonadState`;
- easy to tune for readability.

## Phase 2: Procedural Mesh Generation in Unity

After primitive prototypes work, optional procedural meshes can improve shape quality.

Possible procedural assets:

- smoother mouse silhouette mesh;
- stylized gonad mesh variants;
- tube mesh for flow paths;
- label anchor rings;
- pulsing outline mesh for unstable state.

Procedural meshes should remain schematic and low-detail.

## Phase 3: Blender-Generated Low-Poly Models

Blender Python scripts may be used later to create:

- low-poly mouse body;
- low-poly gonad variants;
- simplified internal organ markers;
- abstract flow tubes or arcs.

Rules:

- keep meshes stylized and non-clinical;
- avoid anatomical precision that could suggest surgical training;
- export clean, simple assets;
- preserve separate materials for body, organ, highlight, and flow.

## Phase 4: Optional Polished Imported Assets

Polished assets may be considered only after the primitive and low-poly versions are useful.

Imported assets should:

- improve readability;
- keep the educational style;
- remain non-realistic;
- avoid human anatomy;
- avoid medical or procedural imagery.

## Suggested Folders

```text
Assets/
  Art/
    Materials/
      ThreeD/
    Sprites/
      Labels/
  Models/
    Mouse/
    Organs/
    Flow/
  Prefabs/
    ThreeD/
      Mouse/
      Gonad/
      Endocrine/
      Labels/
  Scripts/
    Visualization/
      ThreeD/
```

## Material Guidelines

Use:

- transparent body materials;
- teal/green for testis-like state;
- rose/amber for ovary-like state;
- split colors for mixed state;
- yellow-gray for unstable state;
- soft emissive flow lines if helpful.

Avoid:

- red realistic blood materials;
- wet tissue shaders;
- gore-like materials;
- high-detail anatomical textures.
