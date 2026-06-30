# Scientific Background

This project is inspired by mouse developmental biology research on sex determination, gonadal fate maintenance, and somatic sex reprogramming.

The simulator is an educational abstraction. It does not reproduce full embryonic development, does not provide wet-lab protocols, and must not be used for medical or human application decisions.

## Conceptual Model

The project visualizes high-level relationships such as:

```text
gene/pathway activity
↓
cell fate bias
↓
tissue-level output
↓
phenotype-like educational result
```

These links are simplified for teaching. They should not be read as full mechanistic coverage, quantitative biological prediction, or a direct map to human biology.

## Key References

### SRY and SOX9 in Mammalian Testis Determination

Reference:

```text
Koopman, P. (1999). Sry and Sox9: mammalian testis-determining genes.
```

Link:

- PMC: https://pmc.ncbi.nlm.nih.gov/articles/PMC11146764/

Simulator relevance:

- supports the Level 1 focus on SRY as an upstream trigger for testis-pathway development;
- supports the simplified analogy of SRY as a bootloader and SOX9 as a central testis fate module;
- motivates the educational SRY → SOX9 → Sertoli/testis-like path.

### DMRT1 Maintains Testis Fate After Birth

Reference:

```text
Matson et al. (2011). DMRT1 prevents female reprogramming in the postnatal mammalian testis.
```

Links:

- PMC: https://pmc.ncbi.nlm.nih.gov/articles/PMC3150961/
- PubMed: https://pubmed.ncbi.nlm.nih.gov/21775990/

Simulator relevance:

- supports future maintenance-state modeling after initial sex determination;
- motivates DMRT1 as a later testis maintenance guard;
- belongs mainly to Milestone 2, not the Level 1 embryonic SRY/SOX9/WNT competition model.

### FOXL2 Maintains Ovarian Fate in Adult Mice

Reference:

```text
Uhlenhaut et al. (2009). Somatic Sex Reprogramming of Adult Ovaries to Testes by FOXL2 Ablation.
```

Links:

- PubMed: https://pubmed.ncbi.nlm.nih.gov/20005806/
- Journal: https://www.cell.com/fulltext/S0092-8674(09)01433-0

Simulator relevance:

- supports future ovarian maintenance-state modeling;
- motivates FOXL2 as a later ovarian maintenance guard;
- should be represented conceptually, without experimental procedures or clinical claims.

### SOX9 and SOX8 Protect Adult Testis Identity

Reference:

```text
Barrionuevo et al. (2016). Sox9 and Sox8 protect the adult testis from male-to-female genetic reprogramming and complete degeneration.
```

Links:

- PMC: https://pmc.ncbi.nlm.nih.gov/articles/PMC4945155/
- PubMed: https://pubmed.ncbi.nlm.nih.gov/27328324/

Simulator relevance:

- supports future modeling of adult testis identity maintenance;
- motivates SOX8/SOX9 as maintenance factors beyond initial embryonic pathway selection;
- belongs mainly to Milestone 2.

### FOXL2 and Ovarian Function / Steroid Metabolism

Reference:

```text
Caburet et al. (2012). The transcription factor FOXL2: at the crossroads of ovarian physiology and pathology.
```

Link:

- PubMed: https://pubmed.ncbi.nlm.nih.gov/21763750/

Simulator relevance:

- supports future endocrine and ovarian-function context;
- may inform later conceptual dashboards for ovarian pathway maintenance and steroid metabolism;
- should not be used to imply clinical translation.

## Mouse Model Scope

Mouse models are useful for understanding developmental biology, but they are not direct human therapy models.

In this simulator:

- mouse findings are translated into abstract educational variables;
- scores are unitless and simplified;
- thresholds are teaching tools, not fitted constants;
- outputs are phenotype-like labels, not biological diagnoses.

## Safety Boundary

This background document is for citation and orientation only.

It does not provide:

- wet-lab protocols;
- gene editing instructions;
- CRISPR guide design;
- viral delivery methods;
- injection schedules;
- animal handling instructions;
- surgical methods;
- medical advice;
- human application guidance.

