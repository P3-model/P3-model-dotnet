using System.Collections;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain.DynamicModel;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;
using P3Model.Parser.ModelSyntax.Domain.StaticModel.Ddd;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;
using P3Model.Parser.ModelSyntax.Technology.CSharp;

namespace P3Model.Parser.Tests.ModelSyntax.ExampleObjects;

public class ExampleElements : IEnumerable<ElementBase>
{
    private readonly Dictionary<Type, ElementBase> _elements = new();

    private static readonly DomainModule DomainModule =
        new(HierarchyId.FromParts("ExampleModuleA", "ModuleB", "ModuleC"));

    public static readonly ExampleElements All = new()
    {
        new Actor("ExampleActor"),
        new BusinessOrganizationalUnit("ExampleBusinessOrganizationalUnit"),
        new CSharpProject("ExampleCSharpProject", "ExampleDirectory/ExampleCSharpProject"),
        new CSharpNamespace(HierarchyId.FromParts("ExampleParentCSharpNamespace", "ExampleCSharpNamespace"),
                "ExampleDirectory/ExampleCSharpNamespace"),
        new CSharpType("ExampleCSharpNamespace.ExampleCSharpType",
            "ExampleCSharpType",
            "ExampleDirectory/ExampleCSharpType"),
        new Database("ExampleDatabase"),
        new DatabaseCluster("ExampleDatabaseCluster"),
        new DddAggregate("ExampleModuleA.ExampleDddAggregate", "ExampleDddAggregate"),
        new DddDomainService("ExampleModuleA.ExampleDddDomainService", "ExampleDddDomainService"),
        new DddEntity("ExampleModuleA.ExampleDddEntity", "ExampleDddEntity"),
        new DddFactory("ExampleModuleA.ExampleDddFactory", "ExampleDddFactory"),
        new DddRepository("ExampleModuleA.ExampleDddRepository", "ExampleDddRepository"),
        new DddValueObject("ExampleModuleA.ExampleDddValueObject", "ExampleDddValueObject"),
        new DeployableUnit("ExampleDeployableUnit"),
        new DevelopmentTeam("ExampleDevelopmentTeam"),
        new DomainBuildingBlock("ExampleModuleA.ExampleBuildingBlock", "ExampleBuildingBlock"),
        DomainModule,
        new ExternalSystem("ExampleExternalSystem"),
        new ExternalSystemIntegration("ExampleModuleA.ExternalSystemIntegration", "ExternalSystemIntegration"),
        new Layer("ExampleLayer"),
        new Process("ExampleProcessX"),
        new ProcessStep("ExampleModuleA.StepA", "StepA"),
        new Tier("ExampleTier")
    };

    private void Add<TElement>(TElement element)
        where TElement : ElementBase
        => _elements.Add(typeof(TElement), element);

    public ElementBase ForType(Type type)
    {
        if (!_elements.TryGetValue(type, out var element))
            throw new InvalidOperationException($"Missing example for {type.Name}");
        return element;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<ElementBase> GetEnumerator() => _elements.Values.GetEnumerator();
}