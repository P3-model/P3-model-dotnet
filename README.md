# P3 Model tools and libraries for .net

[![CC BY-SA 4.0][cc-by-sa-shield]][cc-by-sa]

In this repository you can find all tools and libraries that facilitate use of P3 Model in .net.

P3 Model is a tool to automatically generate documentation from your source code.  
Generated documentation is based on information already present in the code and additional metadata added with annotations and JSON or Markdown files.   

If you haven't check out main [**P3 Model**](https://github.com/P3-model/P3-model) repository then we strongly encourage you to do so.

## The goal

Our goal is to provide visualization of the developed system including all important aspects:
- **domain** model (processes, modules, building blocks, etc.)
- **technology** solutions (deployable units, layers, api, types, etc.)
- **people** working on it (teams responsibilities, business ownership, etc.)

The visualization is based on parsed source code augmented with metadata (from `P3Model.Annotations` library).  
You can see the idea in this video:

Now it's only very simple, fixed set of Markdown pages with Mermaid diagrams. It's MVP.  
In the future we plan to provide dynamic visualization tool (with zooming, filtering elements and relations, _ad hoc_ diagrams, etc.). 

## Getting started

### Add metadata to your source code
1. Add [P3Model.Annotations package](https://www.nuget.org/packages/P3Model.Annotations/) to each project you want to add metadata to.
2. Add appropriate attributed to your code. Full list of annotations with usage examples you can find [here](Sources/Annotations/UsageExamples.md).

### Implement parser

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

### Run

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

### Living example

If you'd like to see P3 Model in bigger code base check [DDD Starter for .net](https://github.com/itlibrium/DDD-starter-dotnet).

## Contribution

Join us and help building P3 Model. Let's solve the problem of outdated and useless documentation together.

To start contributing you can:
- join our [discussions](https://github.com/P3-model/P3-model/discussions) in main repository
- open issue in this repository and write your proposition or question

## License

This work is licensed under a
[Creative Commons Attribution-ShareAlike 4.0 International License][cc-by-sa].

[![CC BY-SA 4.0][cc-by-sa-image]][cc-by-sa]

[cc-by-sa]: http://creativecommons.org/licenses/by-sa/4.0/
[cc-by-sa-image]: https://licensebuttons.net/l/by-sa/4.0/88x31.png
[cc-by-sa-shield]: https://img.shields.io/badge/License-CC%20BY--SA%204.0-lightgrey.svg