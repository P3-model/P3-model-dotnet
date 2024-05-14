# P3 Model Code Analysis Abstractions

**P3 Model Code Analysis Abstractions is a set of base abstractions to build custom code analyzers for P3 Model Parser.**

## Usage

1. Create a new .net class library project.
2. Add [P3Model.Parser.CodeAnalysis.Abstractions package](https://www.nuget.org/packages/P3Model.Parser.CodeAnalysis.Abstractions/).
3. Implement `SymbolAnalyzer`, `OperationAnalyzer` and/or `FileAnalyzer` interface with your custom logic.
4. Add your custom analyzer to `P3Model.Parser` through `P3.Analyzers` method.

For more information about P3 Model .net implementation check [P3 Model .net repository](https://github.com/P3-model/P3-model-dotnet).

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
