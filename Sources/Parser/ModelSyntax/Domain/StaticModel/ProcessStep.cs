namespace P3Model.Parser.ModelSyntax.Domain.StaticModel;

public class ProcessStep(string idPartUniqueForElementType, string name)
    : DomainBuildingBlock(idPartUniqueForElementType, name)
{
    public new class DependsOnBuildingBlock(ProcessStep source, DomainBuildingBlock destination) : 
        DomainBuildingBlock.DependsOnBuildingBlock(source, destination), 
        Relation<ProcessStep, DomainBuildingBlock>
    {
        Element Relation.Source => Source;

        public new ProcessStep Source { get; } = source;
    }
    
    public class HasNextStep(ProcessStep source, ProcessStep destination) 
        : RelationBase<ProcessStep, ProcessStep>(source, destination);
}