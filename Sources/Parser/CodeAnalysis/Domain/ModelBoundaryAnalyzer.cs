using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Humanizer;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Domain;
using P3Model.Parser.CodeAnalysis.RoslynExtensions;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain;
using P3Model.Parser.ModelSyntax.People;
using Serilog;

namespace P3Model.Parser.CodeAnalysis.Domain;

public class ModelBoundaryAnalyzer : FileAnalyzer, SymbolAnalyzer<IAssemblySymbol>, SymbolAnalyzer<INamespaceSymbol>
{
    public Task Analyze(FileInfo fileInfo, ModelBuilder modelBuilder)
    {
        if (fileInfo.Name.EndsWith("ModelInfo.json"))
            TryCreateModelBoundary(fileInfo, null, modelBuilder, out _);
        return Task.CompletedTask;
    }

    public void Analyze(IAssemblySymbol symbol, ModelBuilder modelBuilder)
    {
        var modelBoundary = ResolveForAssembly(symbol, null, modelBuilder);
        ResolveForDirectories(symbol.GlobalNamespace, modelBoundary, modelBuilder);
    }

    public void Analyze(INamespaceSymbol symbol, ModelBuilder modelBuilder)
    {
        var modelBoundary = ResolveForNamespaces(symbol, modelBuilder);
        modelBoundary = ResolveForAssemblies(symbol, modelBoundary, modelBuilder);
        ResolveForDirectories(symbol, modelBoundary, modelBuilder);
    }

    public bool TryResolve(ISymbol symbol, ModelBuilder modelBuilder,
        [NotNullWhen(true)] out ModelBoundary? modelBoundary)
    {
        if (TryGetNamespaceSymbol(symbol, out var namespaceSymbol))
        {
            modelBoundary = ResolveForNamespaces(namespaceSymbol, modelBuilder);
            modelBoundary = ResolveForAssemblies(namespaceSymbol, modelBoundary, modelBuilder);
            ResolveForDirectories(namespaceSymbol, modelBoundary, modelBuilder);
            return modelBoundary != null;
        }
        modelBoundary = null;
        return false;
    }

    private static bool TryGetNamespaceSymbol(ISymbol symbol, [NotNullWhen(true)] out INamespaceSymbol? namespaceSymbol)
    {
        namespaceSymbol = symbol switch
        {
            INamespaceSymbol ns => ns,
            INamedTypeSymbol ts => ts.ContainingNamespace,
            IMethodSymbol ms => ms.ContainingType.ContainingNamespace,
            _ => null
        };
        return namespaceSymbol != null;
    }

    private static ModelBoundary? ResolveForNamespaces(INamespaceSymbol symbol, ModelBuilder modelBuilder)
    {
        ModelBoundary? modelBoundary = null;
        while (!symbol.IsGlobalNamespace)
        {
            TryCreateModelBoundary(symbol, modelBuilder, modelBoundary, out modelBoundary);
            symbol = symbol.ContainingNamespace;
        }
        return modelBoundary;
    }

    // TODO: determine assembly for originally requested symbol
    private ModelBoundary? ResolveForAssemblies(INamespaceSymbol symbol, ModelBoundary? modelBoundary,
        ModelBuilder modelBuilder) => symbol
        .ConstituentNamespaces
        .Select(ns => ns.ContainingAssembly)
        .Aggregate(modelBoundary,
            (current, assemblySymbol) =>
                ResolveForAssembly(assemblySymbol, current, modelBuilder));

    private ModelBoundary? ResolveForAssembly(IAssemblySymbol symbol, ModelBoundary? modelBoundary,
        ModelBuilder modelBuilder)
    {
        TryCreateModelBoundary(symbol, modelBuilder, modelBoundary, out modelBoundary);
        return modelBoundary;
    }

    private static bool TryCreateModelBoundary(ISymbol symbol, ModelBuilder modelBuilder, 
        ModelBoundary? alreadyResolvedBoundary, [NotNullWhen(true)] out ModelBoundary? modelBoundary)
    {
        if (!symbol.TryGetAttribute(typeof(ModelBoundaryAttribute), out var attribute))
        {
            modelBoundary = alreadyResolvedBoundary;
            return false;
        }
        if (symbol.IsExplicitlyExcludedFromDomainModel())
            LogSymbolExcludedFromDomainModel(symbol);
        if (alreadyResolvedBoundary != null)
        {
            LogDuplicatedBoundaryInCodePath(symbol, alreadyResolvedBoundary);
            modelBoundary = alreadyResolvedBoundary;
            return false;
        }
        if (!attribute.TryGetConstructorArgumentValue<string>(nameof(ModelBoundaryAttribute.Name), out var name))
            name = symbol.Name;
        modelBoundary = new ModelBoundary(ElementId.Create<ModelBoundary>(name.Dehumanize()),
            name.Humanize(LetterCasing.Title));
        modelBuilder.Add(modelBoundary, symbol);
        return true;
    }

    private static ModelBoundary? ResolveForDirectories(INamespaceSymbol symbol, ModelBoundary? alreadyResolvedBoundary,
        ModelBuilder modelBuilder)
    {
        // TODO: determine assembly for originally requested symbol; check locations for assembly
        foreach (var location in symbol.ConstituentNamespaces.SelectMany(cn => cn.ContainingAssembly.Locations))
        {
            if (!TryGetDirectory(location, out var directory))
                continue;
            foreach (var file in directory.EnumerateFiles("ModelInfo.json", SearchOption.TopDirectoryOnly))
            {
                if (TryCreateModelBoundary(file, alreadyResolvedBoundary, modelBuilder, out var boundary))
                    alreadyResolvedBoundary = boundary;
            }
        }
        return alreadyResolvedBoundary;
    }

    private static bool TryGetDirectory(Location location, [NotNullWhen(true)] out DirectoryInfo? directory)
    {
        if (location.SourceTree == null)
        {
            directory = null;
            return false;
        }
        var file = new FileInfo(location.SourceTree.FilePath);
        directory = file.Directory;
        if (directory == null)
        {
            LogStrangeSourceLocation(location.SourceTree.FilePath);
            return false;
        }
        return true;
    }

    private static bool TryCreateModelBoundary(FileInfo file, ModelBoundary? alreadyResolvedBoundary, 
        ModelBuilder modelBuilder, [NotNullWhen(true)] out ModelBoundary? boundary)
    {
        var directory = file.Directory;
        if (directory is null)
        {
            LogStrangeSourceLocation(file.FullName);
            boundary = null;
            return false;
        }
        if (!TryGetModelInfo(file, out var modelInfo))
        {
            boundary = alreadyResolvedBoundary;
            return false;
        }
        if (alreadyResolvedBoundary != null)
        {
            LogDuplicatedBoundaryInCodePath(file, alreadyResolvedBoundary);
            boundary = alreadyResolvedBoundary;
            return false;
        }
        boundary = new ModelBoundary(
            ElementId.Create<ModelBoundary>(modelInfo.ModelBoundary.Dehumanize()),
            modelInfo.ModelBoundary.Humanize(LetterCasing.Title));
        modelBuilder.Add(boundary, directory);
        AddRelationToDevelopmentOwner(modelInfo, boundary, modelBuilder);
        AddRelationToBusinessOwner(modelInfo, boundary, modelBuilder);
        return true;
    }

    private static bool TryGetModelInfo(FileInfo file, [NotNullWhen(true)] out ModelBoundaryInfo? modelInfo)
    {
        using var stream = file.Open(FileMode.Open);
        try
        {
            // TODO: JsonSerializerOptions configuration
            modelInfo = JsonSerializer.Deserialize<ModelBoundaryInfo>(stream, new JsonSerializerOptions());
        }
        catch (Exception e)
        {
            LogInvalidJsonFile(file, e);
            modelInfo = null;
            return false;
        }
        if (modelInfo is null)
        {
            LogInvalidJsonFile(file);
            return false;
        }
        return true;
    }

    private static void LogDuplicatedBoundaryInCodePath(ISymbol symbol, ModelBoundary modelBoundary) => Log.Warning(
        "Model Boundary associated with Symbol: {symbol} is defined in the same code path as Model Boundary: {modelBoundary}",
        symbol.ToDisplayString(),
        modelBoundary.Id);

    private static void LogDuplicatedBoundaryInCodePath(FileInfo file, ModelBoundary modelBoundary) =>
        Log.Warning(
            "Model Boundary associated with File: {directory} is defined in the same code path as Model Boundary: {modelBoundary}",
            file.Name,
            modelBoundary.Id);

    private static void LogSymbolExcludedFromDomainModel(ISymbol symbol) => Log.Warning(
        "Symbol: {symbol} associated with Model Boundary is explicitly excluded from domain model",
        symbol.ToDisplayString());

    private static void LogInvalidJsonFile(FileInfo file) => Log.Error(
        "Invalid JSON model boundary file: {file}.",
        file.FullName);

    private static void LogInvalidJsonFile(FileInfo file, Exception exception) => Log.Error(exception,
        "Invalid JSON model boundary file: {file}.",
        file.FullName);

    private static void LogStrangeSourceLocation(string path) =>
        Log.Warning("Strange source location: {path} - resolving Model Boundary skipped", path);

    private static void AddRelationToDevelopmentOwner(ModelBoundaryInfo modelInfo, ModelBoundary modelBoundary,
        ModelBuilder modelBuilder)
    {
        if (modelInfo.DevelopmentOwner == null)
            return;
        var team = new DevelopmentTeam(
            ElementId.Create<DevelopmentTeam>(modelInfo.DevelopmentOwner.Dehumanize()),
            modelInfo.DevelopmentOwner.Humanize());
        modelBuilder.Add(team);
        modelBuilder.Add(new DevelopmentTeam.OwnsModelBoundary(team, modelBoundary));
    }

    private static void AddRelationToBusinessOwner(ModelBoundaryInfo modelInfo, ModelBoundary modelBoundary,
        ModelBuilder modelBuilder)
    {
        if (modelInfo.BusinessOwner == null)
            return;
        var organizationalUnit = new BusinessOrganizationalUnit(
            ElementId.Create<BusinessOrganizationalUnit>(modelInfo.BusinessOwner.Dehumanize()),
            modelInfo.BusinessOwner.Humanize());
        modelBuilder.Add(organizationalUnit);
        modelBuilder.Add(new BusinessOrganizationalUnit.OwnsModelBoundary(organizationalUnit, modelBoundary));
    }
}