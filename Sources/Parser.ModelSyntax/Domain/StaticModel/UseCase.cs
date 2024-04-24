namespace P3Model.Parser.ModelSyntax.Domain.StaticModel;

public class UseCase(ElementId id, string name) : DomainBuildingBlock(id, name)
{
    public new class DependsOnBuildingBlock(UseCase source, DomainBuildingBlock destination) :
        DomainBuildingBlock.DependsOnBuildingBlock(source, destination),
        Relation<UseCase, DomainBuildingBlock>
    {
        Element Relation.Source => Source;

        public new UseCase Source { get; } = source;
    }
}