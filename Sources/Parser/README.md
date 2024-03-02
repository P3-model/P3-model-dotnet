# P3 Model Parser

P3 Model Parser is a tool that generate documentation from C# source code enriched with metadata like: annotations and JSON or Markdown files.

## Usage

1. Create console application.
2. Add [P3Model.Parser package](https://www.nuget.org/packages/P3Model.Parser/).
3. Configure Parser in `Program.cs` like that:
    ```csharp
    await P3
        .Product(product => product
            .UseName("YOUR_SYSTEM_NAME"))
        .Repositories(repositories => repositories
            .Use("REPOSITORY_PATH"))
        .Analyzers(analyzers => analyzers
            .UseDefaults(options => options
                .TreatNamespacesAsDomainModules(namespaces => namespaces
                    .RemoveNamespacePart("PART_TO_REMOVE"))))
        .OutputFormat(formatters => formatters
            .UseMermaid(options => options
                .Directory("MERMAID_OUTPUT_PATH")
                .UseDefaultPages())
            .UseJson(options => options
                .File("JSON_OUTPUT_PATH")))
        .LogLevel(LogEventLevel.Verbose)
        .Analyze();
    ```
4. Run 
   - Run parser from command line or your IDE.
   - Integrate Parser with your CI pipeline to update documentation on every commit.  
     Here is example with GitHub Action:
     ```yaml
      name: Generate P3 documentation
   
      on:
        push:
          branches:
            - main
   
      jobs:
        generate-p3-docs:
          runs-on: ubuntu-latest
       
          permissions:      
            contents: write
   
          steps:
            - name: Checkout repository
              uses: actions/checkout@v4
   
            - name: Restore packages
              run: dotnet restore
   
            - name: Run P3 Parser
              run: dotnet run --project Docs/P3/DocsGenerator/DocsGenerator.csproj
   
            - name: Add, Commit & Push changes
              uses: stefanzweifel/git-auto-commit-action@v5
              with:
                commit_message: "P3 docs updated"
        ```
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
