using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.OutputFormatting;
using Serilog;

namespace P3Model.Parser.CodeAnalysis;

public class RootAnalyzer
{
    private static readonly string[] SupportedFileTypes = ["json", "md"];
    private readonly string _productName;
    private readonly IReadOnlyCollection<RepositoryToAnalyze> _repositories;
    private readonly IReadOnlyCollection<FileAnalyzer> _fileAnalyzers;
    private readonly IReadOnlyCollection<SymbolAnalyzer> _symbolAnalyzers;
    private readonly IReadOnlyCollection<OperationAnalyzer> _operationAnalyzers;
    private readonly IReadOnlyCollection<OutputFormatter> _outputFormatters;

    internal RootAnalyzer(string productName,
        IReadOnlyCollection<RepositoryToAnalyze> repositories,
        IReadOnlyCollection<FileAnalyzer> fileAnalyzers,
        IReadOnlyCollection<SymbolAnalyzer> symbolAnalyzers,
        IReadOnlyCollection<OperationAnalyzer> operationAnalyzers,
        IReadOnlyCollection<OutputFormatter> outputFormatters, 
        LoggerConfiguration loggerConfiguration)
    {
        _repositories = repositories;
        _fileAnalyzers = fileAnalyzers;
        _symbolAnalyzers = symbolAnalyzers;
        _operationAnalyzers = operationAnalyzers;
        _outputFormatters = outputFormatters;
        _productName = productName;
        Log.Logger = loggerConfiguration.CreateLogger();
    }

    public async Task Analyze(TargetFramework? defaultFramework = null)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            Log.Information("Analysis started.");
            foreach (var outputFormatter in _outputFormatters)
                await outputFormatter.Clean();
            Log.Information("Previous documentation cleaned.");
            var modelBuilder = new DefaultModelBuilder(new DocumentedSystem(_productName));
            foreach (var repository in _repositories)
                await Analyze(repository, defaultFramework, modelBuilder);
            var model = modelBuilder.Build();
            foreach (var outputFormatter in _outputFormatters)
                await outputFormatter.Write(model);
            stopwatch.Stop();
            Log.Information($"Analysis finished in {stopwatch.ElapsedMilliseconds / 1000}s.");
        }
        catch (Exception e)
        {
            Log.Fatal(e, "Analysis failed");
        }
    }

    private async Task Analyze(RepositoryToAnalyze repository, TargetFramework? defaultFramework, 
        ModelBuilder modelBuilder)
    {
        Log.Verbose($"Analysis started for repository: {repository.Directory.FullName}");
        await AnalyzeFiles(repository, modelBuilder);
        var projects = repository.GetProjects(defaultFramework);
        await Parallel.ForEachAsync(projects,
            async (project, _) => await Analyze(project, modelBuilder));
        Log.Verbose($"Analysis finished for repository: {repository.Directory.FullName}");
    }

    private Task AnalyzeFiles(RepositoryToAnalyze repository, ModelBuilder modelBuilder) =>
        Parallel.ForEachAsync(GetAllSupportedFiles(repository.Directory), async (fileInfo, _) =>
        {
            Log.Verbose($"Analysis started for file: {fileInfo.FullName}");
            foreach (var fileAnalyzer in _fileAnalyzers)
                await fileAnalyzer.Analyze(fileInfo, modelBuilder);
            Log.Verbose($"Analysis finished for file: {fileInfo.FullName}");
        });

    private static IEnumerable<FileInfo> GetAllSupportedFiles(DirectoryInfo directoryInfo) => SupportedFileTypes
        .SelectMany(fileType => directoryInfo
            .EnumerateFiles($"*.{fileType}", SearchOption.AllDirectories));

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
        Log.Verbose($"Analysis finished for project: {project.Name}");
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
        var syntaxWalker = new SyntaxWalker(_symbolAnalyzers, _operationAnalyzers, semanticModel, modelBuilder);
        Log.Verbose($"Analysis started for document: {document.FilePath}");
        syntaxWalker.Visit(rootNode);
        Log.Verbose($"Analysis finished for document: {document.FilePath}");
    }
}