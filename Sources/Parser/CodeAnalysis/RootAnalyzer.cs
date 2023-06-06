using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Basic.Reference.Assemblies;
using Microsoft.Build.Locator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using P3Model.Annotations.Domain.StaticModel.DDD;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.OutputFormatting;

namespace P3Model.Parser.CodeAnalysis;

public record RepositoryToAnalyze(string Path, IReadOnlyCollection<string> SlnPaths);

public class RootAnalyzer
{
    private readonly IReadOnlyCollection<RepositoryToAnalyze> _repositories;
    private readonly IReadOnlyCollection<FileAnalyzer> _fileAnalyzers;
    private readonly IReadOnlyCollection<SymbolAnalyzer> _symbolAnalyzers;
    private readonly IReadOnlyCollection<OutputFormatter> _outputFormatters;

    internal RootAnalyzer(IReadOnlyCollection<RepositoryToAnalyze> repositories,
        IReadOnlyCollection<FileAnalyzer> fileAnalyzers, 
        IReadOnlyCollection<SymbolAnalyzer> symbolAnalyzers, 
        IReadOnlyCollection<OutputFormatter> outputFormatters)
    {
        _repositories = repositories;
        _fileAnalyzers = fileAnalyzers;
        _symbolAnalyzers = symbolAnalyzers;
        _outputFormatters = outputFormatters;
    }

    public async Task Analyze()
    {
        var sw = Stopwatch.StartNew();
        MSBuildLocator.RegisterDefaults();
        var modelBuilder = new ModelBuilder();
        foreach (var repository in _repositories) 
            await Analyze(repository, modelBuilder);
        var model = modelBuilder.Build();
        foreach (var outputFormatter in _outputFormatters)
            await outputFormatter.Write(model);
        sw.Stop();
        Console.WriteLine($"Total: {sw.ElapsedMilliseconds / 1000}s.");
    }

    private async Task Analyze(RepositoryToAnalyze repository, ModelBuilder modelBuilder)
    {
        await AnalyzeMarkdownFiles(repository, modelBuilder);
        await foreach (var solution in GetSolutionsFor(repository))
            await Parallel.ForEachAsync(solution.Projects,
                async (project, _) => await Analyze(project, modelBuilder));
    }

    private async Task AnalyzeMarkdownFiles(RepositoryToAnalyze repository, ModelBuilder modelBuilder)
    {
        var directoryInfo = new DirectoryInfo(repository.Path);
        await Parallel.ForEachAsync(directoryInfo.EnumerateFiles("*.md", SearchOption.AllDirectories), 
            async (fileInfo, _) =>
            {
                foreach (var fileAnalyzer in _fileAnalyzers) 
                    await fileAnalyzer.Analyze(fileInfo, modelBuilder);
            });
    }
    
    private static async IAsyncEnumerable<Solution> GetSolutionsFor(RepositoryToAnalyze repository)
    {
        if (repository.SlnPaths.Count == 0)
        {
            var directoryInfo = new DirectoryInfo(repository.Path);
            foreach (var fileInfo in directoryInfo.EnumerateFiles("*.sln", SearchOption.AllDirectories))
                yield return await Load(fileInfo.FullName);
        }
        else
        {
            foreach (var slnPath in repository.SlnPaths)
                yield return await Load(slnPath);
        }
    }

    private static async Task<Solution> Load(string slnPath)
    {
        var workspace = MSBuildWorkspace.Create();
        workspace.SkipUnrecognizedProjects = true;
        workspace.WorkspaceFailed += (_, args) =>
        {
            if (args.Diagnostic.Kind == WorkspaceDiagnosticKind.Failure)
                Console.Error.WriteLine(args.Diagnostic.Message);
        };
        return await workspace.OpenSolutionAsync(slnPath);
    }

    private static async Task<Compilation> Compile(Project project)
    {
        var compilation = await project.GetCompilationAsync();
        if (compilation is null)
            throw new InvalidOperationException($"Can not compile project: {project.Name}");
        compilation = compilation
            .AddReferences(Net60.References.All)
            .AddReferences(MetadataReference.CreateFromFile(typeof(DddAggregateAttribute).Assembly.Location));
        return compilation;
    }

    private async Task Analyze(Project project, ModelBuilder builder)
    {
        var compilation = await Compile(project);
        Analyze(compilation, builder);
        await Parallel.ForEachAsync(project.Documents,
            async (document, _) => await Analyze(document, compilation, builder));
    }

    private void Analyze(Compilation compilation, ModelBuilder builder)
    {
        var assemblySymbol = compilation.Assembly;
        foreach (var symbolAnalyzer in _symbolAnalyzers.OfType<SymbolAnalyzer<IAssemblySymbol>>())
            symbolAnalyzer.Analyze(assemblySymbol, builder);
    }

    private async Task Analyze(Document document, Compilation compilation, ModelBuilder modelBuilder)
    {
        var syntaxTree = await document.GetSyntaxTreeAsync();
        if (syntaxTree is null)
            throw new InvalidOperationException();
        var semanticModel = compilation.GetSemanticModel(syntaxTree);
        var rootNode = await syntaxTree.GetRootAsync();
        var syntaxWalker = new SyntaxWalker(_symbolAnalyzers, semanticModel, modelBuilder);
        syntaxWalker.Visit(rootNode);
    }
}