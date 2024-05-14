# P3 Model Querying

**P3 Model Querying provides searching capabilities that can be used to build custom output formatters for P3 Model Parser.**

## Usage

1. Create a new .net class library project.
2. Add [P3Model.Parser.ModelQuerying package](https://www.nuget.org/packages/P3Model.Parser.ModelQuerying/).
3. Add [P3Model.Parser.OutputFormatting.Abstractions package](https://www.nuget.org/packages/P3Model.Parser.OutputFormatting.Abstractions/).
4. Implement `OutputFormatter` interface with your custom logic. Use `ModelGraph` class to query P3 Model.
5. Add your custom output formatter to `P3Model.Parser` through `P3.OutputFormat` method.

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
