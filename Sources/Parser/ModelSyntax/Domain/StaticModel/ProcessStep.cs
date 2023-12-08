namespace P3Model.Parser.ModelSyntax.Domain.StaticModel;

public class ProcessStep : DomainBuildingBlock
{
    public ProcessStep(string name) : base(name) { }
    public ProcessStep(string id, string name) : base(id, name) { }

    public new record DependsOnBuildingBlock(ProcessStep Source, DomainBuildingBlock Destination) : 
        DomainBuildingBlock.DependsOnBuildingBlock(Source, Destination), 
        Relation<ProcessStep, DomainBuildingBlock>
    {
        Element Relation.Source => Source;

        public new ProcessStep Source { get; } = Source;
    }
    
    public record HasNextStep(ProcessStep Source, ProcessStep Destination) : Relation<ProcessStep, ProcessStep>;
}