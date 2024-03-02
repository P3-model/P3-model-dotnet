# P3 Model Annotations

**P3 Model Annotations is a set of metadata attributes** that you can use in you .net code to enrich it with information about domain model, people aspects, used patterns and more.

Working code is enough for machine that execute it but **not enough for people to understand it**.

A lot of information is already in the code but many information are missing because they are not needed for the compiler.  
Thus **we need to add metadata** about:

- business concepts not represented directly in the code (e.g. domain modules, processes, etc.)
- intent to use architecture or design patterns
- rationale of design decisions
- people responsible for maintenance and development of certain parts of the system
- etc.

We believe that enriching code with this additional aspects can help us in:

- better understanding of the design when working with code directly in IDE
- **automatically generating always up-to-date documentation** - Living Documentation
- automation architecture testing

## Usage

1. Add [P3Model.Annotations package](https://www.nuget.org/packages/P3Model.Annotations/) to each .net project you want to add metadata to.
2. Add appropriate attributed to your code. Full list of annotations with usage examples you can find [here](UsageExamples.md).

## P3 Model project

P3 Model is a tool to automatically generate documentation from your source code.  
Generated documentation is based on information already present in the code and additional metadata added with annotations and JSON or Markdown files.  
Each technology like .net or Java has its own tooling. This library is a part of .net tooling.

If you'd like to find more information about P3 Model check project's [main repository](https://github.com/P3-model/P3-model).

## Contribution

If you'd like to contribute check project's [main repository](https://github.com/P3-model/P3-model). 

## License

This work is licensed under a
[Creative Commons Attribution-ShareAlike 4.0 International License][cc-by-sa].

[![CC BY-SA 4.0][cc-by-sa-image]][cc-by-sa]

[cc-by-sa]: http://creativecommons.org/licenses/by-sa/4.0/
[cc-by-sa-image]: https://licensebuttons.net/l/by-sa/4.0/88x31.png
[cc-by-sa-shield]: https://img.shields.io/badge/License-CC%20BY--SA%204.0-lightgrey.svg
