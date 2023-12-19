using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Basic.Reference.Assemblies;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.MSBuild;
using P3Model.Parser.CodeAnalysis.RoslynExtensions;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.OutputFormatting;
using Serilog;

namespace P3Model.Parser.CodeAnalysis;

public class RootAnalyzer
{
    private static readonly string[] SupportedFileTypes = { "json", "md" };
    private readonly string _productName;
    private readonly IReadOnlyCollection<RepositoryToAnalyze> _repositories;
    private readonly IReadOnlyCollection<FileAnalyzer> _fileAnalyzers;
    private readonly IReadOnlyCollection<SymbolAnalyzer> _symbolAnalyzers;
    private readonly IReadOnlyCollection<OutputFormatter> _outputFormatters;

    internal RootAnalyzer(string productName,
        IReadOnlyCollection<RepositoryToAnalyze> repositories,
        IReadOnlyCollection<FileAnalyzer> fileAnalyzers,
        IReadOnlyCollection<SymbolAnalyzer> symbolAnalyzers,
        IReadOnlyCollection<OutputFormatter> outputFormatters,
        LoggerConfiguration loggerConfiguration)
    {
        _repositories = repositories;
        _fileAnalyzers = fileAnalyzers;
        _symbolAnalyzers = symbolAnalyzers;
        _outputFormatters = outputFormatters;
        _productName = productName;
        Log.Logger = loggerConfiguration.CreateLogger();
    }

    public async Task Analyze()
    {
        try
        {
            var sw = Stopwatch.StartNew();
            Log.Information("Analysis started.");
            foreach (var outputFormatter in _outputFormatters)
                await outputFormatter.Clean();
            Log.Information("Previous documentation cleaned.");
            var modelBuilder = new ModelBuilder(new DocumentedSystem(_productName));
            foreach (var repository in _repositories)
                await Analyze(repository, modelBuilder);
            var model = modelBuilder.Build();
            foreach (var outputFormatter in _outputFormatters)
                await outputFormatter.Write(model);
            sw.Stop();
            Log.Information($"Analysis ended in {sw.ElapsedMilliseconds / 1000}s.");
        }
        catch (Exception e)
        {
            Log.Fatal(e, "Analysis failed");
        }
    }

    private async Task Analyze(RepositoryToAnalyze repository, ModelBuilder modelBuilder)
    {
        await AnalyzeMarkdownFiles(repository, modelBuilder);
        await foreach (var solution in GetSolutionsFor(repository))
        {
            Log.Verbose($"Analysis started for solution: {solution.FilePath}");
            var projects = solution.Projects.Where(p => !repository.ExcludedProjects.Contains(p.Name));
            await Parallel.ForEachAsync(projects,
                async (project, _) => await Analyze(project, modelBuilder));
        }
    }

    private async Task AnalyzeMarkdownFiles(RepositoryToAnalyze repository, ModelBuilder modelBuilder)
    {
        var directoryInfo = new DirectoryInfo(repository.Path);
        await Parallel.ForEachAsync(GetAllSupportedFiles(directoryInfo), async (fileInfo, _) =>
        {
            foreach (var fileAnalyzer in _fileAnalyzers)
                await fileAnalyzer.Analyze(fileInfo, modelBuilder);
        });
    }

    private static IEnumerable<FileInfo> GetAllSupportedFiles(DirectoryInfo directoryInfo) => SupportedFileTypes
        .SelectMany(fileType => directoryInfo
            .EnumerateFiles($"*.{fileType}", SearchOption.AllDirectories));

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
        workspace.WorkspaceFailed += (_, args) => Log.Error(args.Diagnostic.Message);
        return await workspace.OpenSolutionAsync(slnPath);
    }

    private async Task Analyze(Project project, ModelBuilder builder)
    {
        var compilation = await Compile(project);
        if (compilation is null)
        {
            Log.Warning($"Analysis skipped for project: {project.Name}");
            return;
        }
        Log.Verbose($"Analysis started for project: {project.Name}");
        Analyze(compilation, builder);
        await Parallel.ForEachAsync(project.Documents,
            async (document, _) => await Analyze(document, compilation, builder));
    }

    private static async Task<Compilation?> Compile(Project project)
    {
        Compilation? compilation;
        try
        {
            compilation = await project.GetCompilationAsync();
        }
        catch (Exception e)
        {
            Log.Error(e, $"Can not compile project: {project.Name}");
            return null;
        }
        if (compilation is null)
        {
            Log.Error($"Can not compile project: {project.Name}");
            return null;
        }
        var targetFramework = project.GetTargetFramework();
        var references = targetFramework switch
        {
            TargetFramework.Net60 => Net60.References.All,
            TargetFramework.Net70 => Net70.References.All,
            _ => throw new ParserError($"Compilation to target framework: {targetFramework} is not supported")
        };
        compilation = compilation.AddReferences(references);
        var errors = compilation.GetDiagnostics().Where(d => d.Severity == DiagnosticSeverity.Error).ToList();
        if (errors.Any())
        {
            Log.Error($"Can not compile project: {project.Name}");
            foreach (var error in errors)
                Log.Error(error.GetMessage());
            return null;
        }
        return compilation;
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
        Log.Verbose($"Analysis started for document: {document.FilePath}");
        syntaxWalker.Visit(rootNode);
        Log.Verbose($"Analysis finished for document: {document.FilePath}");
    }
}