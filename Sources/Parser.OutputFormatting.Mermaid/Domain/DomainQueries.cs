using P3Model.Parser.ModelQuerying;
using P3Model.Parser.ModelSyntax.Domain;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.OutputFormatting.Mermaid.Domain;

public static class DomainQueries
{
    public static IReadOnlySet<DeployableUnit> GetDeployableUnitsFor(this ModelGraph modelGraph, 
        IEnumerable<DomainModule> modules) => modules
        .SelectMany(module => GetDeployableUnitsFor(modelGraph, (DomainModule)module))
        .ToHashSet();
    
    public static IReadOnlySet<DeployableUnit> GetDeployableUnitsFor(this ModelGraph modelGraph, 
        DomainModule module)
    {
        var ancestorDeployableUnit = modelGraph.Execute(query => query
            .Elements<DeployableUnit>()
            .RelatedToAny(subQuery => subQuery
                .AncestorsAndSelf<DomainModule, DomainModule.ContainsDomainModule>(module))
            .ByReverseRelation<DomainModule.IsDeployedInDeployableUnit>(filter => filter
                .MaxBy(r => r.Source.HierarchyPath.Level)));
        var descendantsDeployableUnits = modelGraph.Execute(query => query
            .Elements<DeployableUnit>()
            .RelatedToAny(subQuery => subQuery
                .Descendants<DomainModule, DomainModule.ContainsDomainModule>(module))
            .ByReverseRelation<DomainModule.IsDeployedInDeployableUnit>());
        var deployableUnits = ancestorDeployableUnit != null
            ? descendantsDeployableUnits
                .Union(new[] { ancestorDeployableUnit })
                .ToHashSet()
            : descendantsDeployableUnits;
        return deployableUnits;
    }
    
    public static IReadOnlySet<DevelopmentTeam> GetDevelopmentTeamsFor(this ModelGraph modelGraph, 
        IEnumerable<DomainModule> modules) => modules
        .SelectMany(module => GetDevelopmentTeamsFor(modelGraph, module))
        .ToHashSet();

    public static IReadOnlySet<DevelopmentTeam> GetDevelopmentTeamsFor(this ModelGraph modelGraph, DomainModule module)
    {
        var ancestorTeam = modelGraph.Execute(query => query
            .Elements<DevelopmentTeam>()
            .RelatedToAny(subQuery => subQuery
                .AncestorsAndSelf<DomainModule, DomainModule.ContainsDomainModule>(module))
            .ByRelation<DevelopmentTeam.OwnsDomainModule>(filter => filter
                .MaxBy(g => g.Destination.HierarchyPath.Level)));
        var descendantsTeams = modelGraph.Execute(query => query
            .Elements<DevelopmentTeam>()
            .RelatedToAny(subQuery => subQuery
                .Descendants<DomainModule, DomainModule.ContainsDomainModule>(module))
            .ByRelation<DevelopmentTeam.OwnsDomainModule>());
        var teams = ancestorTeam != null
            ? descendantsTeams
                .Union(new[] { ancestorTeam })
                .ToHashSet()
            : descendantsTeams;
        return teams;
    }
    
    public static IReadOnlySet<BusinessOrganizationalUnit> GetBusinessOrganizationalUnitsFor(this ModelGraph modelGraph, 
        IEnumerable<DomainModule> modules) => modules
        .SelectMany(module => GetBusinessOrganizationalUnitsFor(modelGraph, module))
        .ToHashSet();

    public static IReadOnlySet<BusinessOrganizationalUnit> GetBusinessOrganizationalUnitsFor(this ModelGraph modelGraph,
        DomainModule module)
    {
        var ancestorUnit = modelGraph.Execute(query => query
            .Elements<BusinessOrganizationalUnit>()
            .RelatedToAny(subQuery => subQuery
                .AncestorsAndSelf<DomainModule, DomainModule.ContainsDomainModule>(module))
            .ByRelation<BusinessOrganizationalUnit.OwnsDomainModule>(filter => filter
                .MaxBy(g => g.Destination.HierarchyPath.Level)));
        var descendantsUnits = modelGraph.Execute(query => query
            .Elements<BusinessOrganizationalUnit>()
            .RelatedToAny(subQuery => subQuery
                .Descendants<DomainModule, DomainModule.ContainsDomainModule>(module))
            .ByRelation<BusinessOrganizationalUnit.OwnsDomainModule>());
        var units = ancestorUnit != null
            ? descendantsUnits
                .Union(new[] { ancestorUnit })
                .ToHashSet()
            : descendantsUnits;
        return units;
    }
}