using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

namespace P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;

// TODO: unique names across hierarchy
public record ProcessStep(string Name) : Element
{
    public record HasNextStep(ProcessStep Source, ProcessStep Destination) : Relation<ProcessStep, ProcessStep>;
    
    public record BelongsToDomainModule(ProcessStep Source, DomainModule Destination) 
        : Relation<ProcessStep, DomainModule>;
}