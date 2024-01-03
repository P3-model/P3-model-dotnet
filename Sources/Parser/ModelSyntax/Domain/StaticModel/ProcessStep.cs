namespace P3Model.Parser.ModelSyntax.Domain.StaticModel;

public class ProcessStep(string idPartUniqueForElementType, string name)
    : DomainBuildingBlock(idPartUniqueForElementType, name)
{
    public new record DependsOnBuildingBlock(ProcessStep Source, DomainBuildingBlock Destination) : 
        DomainBuildingBlock.DependsOnBuildingBlock(Source, Destination), 
        Relation<ProcessStep, DomainBuildingBlock>
    {
        Element Relation.Source => Source;

        public new ProcessStep Source { get; } = Source;
    }
    
    public record HasNextStep(ProcessStep Source, ProcessStep Destination) : Relation<ProcessStep, ProcessStep>;
}