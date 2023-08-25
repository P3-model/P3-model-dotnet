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

    public static readonly ExampleElements All = new()
    {
        new Actor("ExampleActor"),
        new BusinessOrganizationalUnit("ExampleBusinessOrganizationalUnit"),
        new CodeStructure("ExampleCodeStructure"),
        new DddAggregate("ExampleDddAggregate"),
        new DddDomainService("ExampleDddDomainService"),
        new DddEntity("ExampleDddEntity"),
        new DddFactory("ExampleDddFactory"),
        new DddRepository("ExampleDddRepository"),
        new DddValueObject("ExampleDddValueObject"),
        new DeployableUnit("ExampleDeployableUnit"),
        new DevelopmentTeam("ExampleDevelopmentTeam"),
        new DomainBuildingBlock("ExampleBuildingBlock"),
        new DomainModule(HierarchyId.FromParts("ExampleModuleA", "ModuleB", "ModuleF")),
        new ExternalSoftwareSystem("ExampleExternalSystem"),
        new Process(HierarchyId.FromParts("ExampleProcessX", "ProcessY")),
        new ProcessStep(HierarchyId.FromParts("ExampleProcessX", "ProcessY", "StepA")),
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