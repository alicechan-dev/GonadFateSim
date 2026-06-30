# Mouse Sex Determination Educational Simulator

## Short Description

Mouse Sex Determination Educational Simulator is a Unity application concept where the user can adjust SRY, SOX9, WNT/β-catenin, and related parameters, then see a simplified probabilistic developmental outcome:

- testis-like gonad;
- ovary-like gonad;
- ovotestis / mixed outcome;
- unstable / failed developmental state.

The first level focuses on how an embryonic mouse gonad can move toward a testis-like or ovary-like developmental path.

The project is evolving from a dashboard into a multi-view educational simulator. The dashboard remains the control surface, but the simulator should also show gene network activity, stylized gonad anatomy, abstract endocrine flow, a simplified whole-mouse context, developmental timeline playback, and scenario comparison.

```text
gene/pathway ON
↓
cell fate bias
↓
tissue output
↓
phenotype
```

## Main Principle

This is not a precise biophysical simulator.

It is an educational abstraction designed to show:

- timing;
- thresholds;
- gene interaction;
- pathway competition;
- probabilistic outcomes.

The model is intended to help programmers, engineers, students, and readers of scientific papers reason about biological systems as interacting modules. In this analogy, genes are modules or functions, enhancer/promoter behavior is similar to configuration or feature flags, pathways are pipelines, cell fate is runtime state, and phenotype is an output build.

## Visual Simulator Docs

The richer simulator design is split across these planning documents:

- [VisualSimulatorVision.md](VisualSimulatorVision.md) — evolution from dashboard to layered simulator.
- [SceneAndViewDesign.md](SceneAndViewDesign.md) — dashboard, network, gonad, endocrine, whole mouse, timeline, and comparison views.
- [AnatomyAndPhenotypeSpec.md](AnatomyAndPhenotypeSpec.md) — safe visual states and conservative phenotype language.
- [AssetPipeline.md](AssetPipeline.md) — 2D, procedural, and optional 3D asset plan.
- [InteractionModel.md](InteractionModel.md) — sliders, presets, tooltips, toggles, compare mode, inspection, and timeline controls.
- [FutureVisualizationRoadmap.md](FutureVisualizationRoadmap.md) — phased delivery plan for the visual simulator.

## Scientific Background

The project is inspired by mouse developmental biology research on sex determination, gonadal fate maintenance, and somatic sex reprogramming.

For the current reference list and how those papers map to simulator concepts, see [ScientificBackground.md](ScientificBackground.md).

## Scope Limitations

This simulator is for education only.
It does not provide wet-lab protocols, medical advice, gene editing instructions, or human application guidance.
Mouse developmental biology findings must not be treated as directly applicable to humans.

The simulator does not teach CRISPR, viral delivery, injections, surgery, animal handling, dosing, embryo manipulation, or any laboratory workflow.
