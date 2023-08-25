using System.Collections;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel.Ddd;
using P3Model.Parser.ModelSyntax.People;
using P3Model.Parser.ModelSyntax.Technology;

namespace Parser.Tests.ModelSyntax.ExampleObjects;

public class ExampleElements : IEnumerable<Element>
{
    private readonly Dictionary<Type, Element> _elements = new();

    private static readonly DomainModule DomainModule =
        new(HierarchyId.FromParts("ExampleModuleA", "ModuleB", "ModuleC"));

    public static readonly ExampleElements All = new()
    {
        new Actor("ExampleActor"),
        new BusinessOrganizationalUnit("ExampleBusinessOrganizationalUnit"),
        new CodeStructure("ExampleCodeStructure"),
        new DddAggregate(DomainModule, "ExampleDddAggregate"),
        new DddDomainService(DomainModule, "ExampleDddDomainService"),
        new DddEntity(DomainModule, "ExampleDddEntity"),
        new DddFactory(DomainModule, "ExampleDddFactory"),
        new DddRepository(DomainModule, "ExampleDddRepository"),
        new DddValueObject(DomainModule, "ExampleDddValueObject"),
        new DeployableUnit("ExampleDeployableUnit"),
        new DevelopmentTeam("ExampleDevelopmentTeam"),
        new DomainBuildingBlock(DomainModule, "ExampleBuildingBlock"),
        DomainModule,
        new ExternalSoftwareSystem("ExampleExternalSystem"),
        new Process(HierarchyId.FromParts("ExampleProcessX", "ProcessY")),
        new ProcessStep(DomainModule, "StepA"),
        new Tier("ExampleTier")
    };
    
    private void Add<TElement>(TElement element)
        where TElement : class, Element
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