namespace P3Model.Parser.ModelSyntax.Domain;

public class UseCase(ElementId id, string name) : DomainBuildingBlock(id, name)
{
    public new class DependsOnBuildingBlock(UseCase source, DomainBuildingBlock destination) :
        DomainBuildingBlock.DependsOnBuildingBlock(source, destination),
        Relation<UseCase, DomainBuildingBlock>
    {
        Element Relation.Source => Source;

        public new UseCase Source { get; } = source;
    }

    public class UsesUseCase(UseCase source, UseCase destination) : RelationBase<UseCase, UseCase>(source, destination);
}