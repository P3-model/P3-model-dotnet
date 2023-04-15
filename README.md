# P3 Model Annotations for .net

[![CC BY-SA 4.0][cc-by-sa-shield]][cc-by-sa]

**P3 Model Annotations is a set of metadata attributes** that you can use in you .net code to enrich it with information about domain model, people aspects, used patterns and more.

This library is a part of [**P3 Model**](https://github.com/P3-model/P3-model) - a tool to navigate throughout complex systems from all important perspectives.

## Introduction

Working code is enough for machine that execute it but not enough for people to understand it.

A lot of information is already in the code base but many informations are missing because they are not needed for the compiler. Thus **we need to add metadata** about:

- business concepts not represented directly in the code (e.g. domain model boundaries, processes, etc.)
- intent to use architecture or design patterns
- rationale of design decisions
- people responsible for maintenance and development of certain parts of the system
- etc.

We believe that enriching code with this additional aspects can help us in:

- better understanding of the design when working with code directly in IDE
- **automatically generating always up-to-date documentation** - Living Documentation
- automation architecture testing

If you haven't check out main [**P3 Model**](https://github.com/P3-model/P3-model) repository then we strongly encourage you to do it.

## Usage

To use P3 Model Annotations simply add NuGet package to your C# project and start annotating your code.

Project with examples is coming soon so stay tuned.

## Contribution

Join us and help building P3 Model. Let's solve the problem of outdated and useless documentation together.

To start contributing or discussing just open GH issue in this repository and write your proposition or question.

## License

This work is licensed under a
[Creative Commons Attribution-ShareAlike 4.0 International License][cc-by-sa].

[![CC BY-SA 4.0][cc-by-sa-image]][cc-by-sa]

[cc-by-sa]: http://creativecommons.org/licenses/by-sa/4.0/
[cc-by-sa-image]: https://licensebuttons.net/l/by-sa/4.0/88x31.png
[cc-by-sa-shield]: https://img.shields.io/badge/License-CC%20BY--SA%204.0-lightgrey.svg