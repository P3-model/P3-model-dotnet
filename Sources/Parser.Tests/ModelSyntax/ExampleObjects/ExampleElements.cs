using System.Collections;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain;
using P3Model.Parser.ModelSyntax.Domain.Ddd;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;
using P3Model.Parser.ModelSyntax.Technology.CSharp;

namespace P3Model.Parser.Tests.ModelSyntax.ExampleObjects;

public class ExampleElements : IEnumerable<Element>
{
    private readonly Dictionary<Type, Element> _elements = new();

    private static readonly DomainModule DomainModule = new(
        ElementId.Create<DomainModule>("ExampleModuleA.ModuleB.ModuleC"),
        HierarchyPath.FromParts("ExampleModuleA", "ModuleB", "ModuleC"));

    public static readonly ExampleElements All =
    [
        new Actor(ElementId.Create<Actor>("ExampleActor"), "ExampleActor"),
        new BusinessOrganizationalUnit(
            ElementId.Create<BusinessOrganizationalUnit>("ExampleBusinessOrganizationalUnit"),
            "ExampleBusinessOrganizationalUnit"),
        new CSharpProject(
            ElementId.Create<CSharpProject>("ExampleCSharpProject"),
            "ExampleCSharpProject",
            "ExampleDirectory/ExampleCSharpProject"),
        new CSharpNamespace(
            ElementId.Create<CSharpNamespace>("ExampleParentCSharpNamespace.ExampleCSharpNamespace"),
            HierarchyPath.FromParts("ExampleParentCSharpNamespace", "ExampleCSharpNamespace"), 
            "ExampleDirectory/ExampleCSharpNamespace"),
        new CSharpType(
            ElementId.Create<CSharpType>("ExampleCSharpNamespace.ExampleCSharpType"),
            "ExampleCSharpType",
            "ExampleDirectory/ExampleCSharpType"),
        new Database(
            ElementId.Create<Database>("ExampleDatabase"), 
            "ExampleDatabase"),
        new DatabaseCluster(
            ElementId.Create<DatabaseCluster>("ExampleDatabaseCluster"), 
            "ExampleDatabaseCluster"),
        new DddAggregate(
            ElementId.Create<DddAggregate>("ExampleModuleA.ExampleDddAggregate"), 
            "ExampleDddAggregate"),
        new DddDomainService(
            ElementId.Create<DddDomainService>("ExampleModuleA.ExampleDddDomainService"),
            "ExampleDddDomainService"),
        new DddEntity(
            ElementId.Create<DddEntity>("ExampleModuleA.ExampleDddEntity"), 
            "ExampleDddEntity"),
        new DddFactory(
            ElementId.Create<DddFactory>("ExampleModuleA.ExampleDddFactory"), 
            "ExampleDddFactory"),
        new DddRepository(
            ElementId.Create<DddRepository>("ExampleModuleA.ExampleDddRepository"),
            "ExampleDddRepository"),
        new DddValueObject(
            ElementId.Create<DddValueObject>("ExampleModuleA.ExampleDddValueObject"),
            "ExampleDddValueObject"),
        new DeployableUnit(
            ElementId.Create<DeployableUnit>("ExampleDeployableUnit"), 
            "ExampleDeployableUnit"),
        new DevelopmentTeam(
            ElementId.Create<DevelopmentTeam>("ExampleDevelopmentTeam"), 
            "ExampleDevelopmentTeam"),
        new DomainBuildingBlock(
            ElementId.Create<DomainBuildingBlock>("ExampleModuleA.ExampleBuildingBlock"),
            "ExampleBuildingBlock"),
        DomainModule,
        new ExternalSystem(
            ElementId.Create<ExternalSystem>("ExampleExternalSystem"), 
            "ExampleExternalSystem"),
        new ExternalSystemIntegration(
            ElementId.Create<ExternalSystemIntegration>("ExampleModuleA.ExternalSystemIntegration"),
            "ExternalSystemIntegration"),
        new Layer(
            ElementId.Create<Layer>("ExampleLayer"), 
            "ExampleLayer"),
        new Process(
            ElementId.Create<Process>("ExampleProcessX"), 
            "ExampleProcessX"),
        new UseCase(
            ElementId.Create<UseCase>("ExampleModuleA.UseCaseA"), 
            "UseCaseA"),
        new Tier(
            ElementId.Create<Tier>("ExampleTier"), 
            "ExampleTier")
    ];

    private void Add<TElement>(TElement element)
        where TElement : Element
        => _elements.Add(typeof(TElement), element);

    public Element ForType(Type type)
    {
        if (!_elements.TryGetValue(type, out var element))
            throw new InvalidOperationException($"Missing example for {type.Name}");
        return element;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<Element> GetEnumerator() => _elements.Values.GetEnumerator();
}