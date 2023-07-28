using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Humanizer;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Domain.StaticModel;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.People;

namespace P3Model.Parser.CodeAnalysis.DomainPerspective;

// TODO: Model Boundary at namespace level
[UsedImplicitly]
public class ModelBoundaryAnalyzer : SymbolAnalyzer<IAssemblySymbol>, FileAnalyzer
{
    public void Analyze(IAssemblySymbol symbol, ModelBuilder modelBuilder)
    {
        if (!symbol.TryGetAttribute(typeof(ModelBoundaryAttribute), out var attribute))
            return;
        var name = attribute.ConstructorArguments[0].Value?.ToString() ?? symbol.Name.Humanize(LetterCasing.Title);
        var modelBoundary = new ModelBoundary(name);
        modelBuilder.Add(modelBoundary, symbol);
        modelBuilder.Add(elements => GetRelations(modelBoundary, symbol, elements));
    }

    private static IEnumerable<Relation> GetRelations(ModelBoundary modelBoundary, IAssemblySymbol assemblySymbol,
        ElementsProvider elements) => elements
        .Where(elementInfo => elementInfo.Element is DomainModule or DomainBuildingBlock or ProcessStep)
        .Where(elementInfo => elementInfo.Symbols
            .Any(symbol => symbol.IsFrom(assemblySymbol)))
        .Select(elementInfo => CreateRelationFor(elementInfo, modelBoundary));

    public async Task Analyze(FileInfo fileInfo, ModelBuilder modelBuilder)
    {
        if (fileInfo.Directory is null)
            throw new InvalidOperationException();
        if (!fileInfo.Name.EndsWith("ModelBoundaryInfo.json"))
            return;
        await using var stream = fileInfo.Open(FileMode.Open);
        // TODO: JsonSerializerOptions configuration
        var modelBoundaryInfo = await JsonSerializer.DeserializeAsync<ModelBoundaryInfo>(stream,
            new JsonSerializerOptions());
        // TODO: warning logging
        if (modelBoundaryInfo is null)
            return;
        // TODO: Rethink adding same element from different analyzers (relations and symbols are no problem, but element's data are).
        var modelBoundary = new ModelBoundary(modelBoundaryInfo.ModelBoundary);
        modelBuilder.Add(modelBoundary, fileInfo.Directory);
        modelBuilder.Add(elements => GetRelations(modelBoundary, fileInfo.Directory, elements));
        if (modelBoundaryInfo.DevelopmentOwner != null)
        {
            var team = new DevelopmentTeam(modelBoundaryInfo.DevelopmentOwner);
            modelBuilder.Add(team);
            modelBuilder.Add(new DevelopmentTeam.OwnsModelBoundary(team, modelBoundary));
        }
        if (modelBoundaryInfo.BusinessOwner != null)
        {
            var organizationalUnit = new BusinessOrganizationalUnit(modelBoundaryInfo.BusinessOwner);
            modelBuilder.Add(organizationalUnit);
            modelBuilder.Add(new BusinessOrganizationalUnit.OwnsModelBoundary(organizationalUnit, modelBoundary));
        }
    }

    private static IEnumerable<Relation> GetRelations(ModelBoundary modelBoundary, DirectoryInfo directory,
        ElementsProvider elements) => elements
        .Where(elementInfo => elementInfo.Element is DomainModule or DomainBuildingBlock or ProcessStep)
        .Where(elementInfo => elementInfo.Symbols
            .Any(symbol => symbol.SourcesAreIn(directory)))
        .Select(elementInfo => CreateRelationFor(elementInfo, modelBoundary));

    private static Relation CreateRelationFor(ElementInfo elementInfo, ModelBoundary modelBoundary) =>
        elementInfo.Element switch
        {
            DomainModule domainModule => new ModelBoundary.ContainsDomainModule(modelBoundary,
                domainModule),
            DomainBuildingBlock buildingBlock => new ModelBoundary.ContainsBuildingBlock(modelBoundary,
                buildingBlock),
            ProcessStep processStep => new ModelBoundary.ContainsProcessStep(modelBoundary, processStep),
            _ => throw new ArgumentOutOfRangeException(nameof(elementInfo), elementInfo, null)
        };
}