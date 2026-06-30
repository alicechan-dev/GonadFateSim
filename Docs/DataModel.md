# Data Model

The simulator should be data-driven where practical. Genes, pathways, and example scenarios should be represented with `ScriptableObject` assets so designers can tune values without rewriting C# code.

## GeneDefinition

Suggested fields:

```csharp
string Id;
string DisplayName;
string ShortDescription;
string ProgrammerAnalogy;
float DefaultActivity;
float MinActivity;
float MaxActivity;
```

Example:

```text
Id: SRY
DisplayName: SRY
ProgrammerAnalogy: bootloader / feature flag for testis pathway
```

Example:

```text
Id: SOX9
DisplayName: SOX9
ProgrammerAnalogy: main module that stabilizes Sertoli/testis fate
```

Example:

```text
Id: WNT_BETA_CATENIN
DisplayName: WNT/β-catenin
ProgrammerAnalogy: competing ovarian pathway pipeline
```

## PathwayDefinition

Suggested fields:

```csharp
string Id;
string DisplayName;
string ShortDescription;
string ProgrammerAnalogy;
Color DisplayColor;
List<GeneDefinition> RelatedGenes;
```

Initial pathway definitions:

- SOX9/Testis pathway;
- WNT/β-catenin/Ovary pathway;
- Instability / mixed-state pathway.

## ScenarioDefinition

Scenarios should be presets that write values into `SimulationParameters`.

Suggested fields:

```csharp
string Id;
string DisplayName;
string ShortDescription;
SimulationParameters Parameters;
```

First version scenarios:

- Normal XY;
- XY Sry knockout-like;
- XY delayed Sry;
- XX baseline;
- XX with Sox9 overactivation concept;
- High WNT/β-catenin;
- Mixed / ovotestis-prone scenario.

Use "-like" for knockout scenarios because this is an educational abstraction, not a lab model.

## Data Flow

```text
ScenarioDefinition
↓
SimulationParameters
↓
SexDeterminationSimulator
↓
GonadState
↓
UI and visualization
```

ScriptableObject assets should define safe educational presets only. They should not include laboratory procedures, experimental recipes, dosage values, injection routes, or gene editing instructions.

