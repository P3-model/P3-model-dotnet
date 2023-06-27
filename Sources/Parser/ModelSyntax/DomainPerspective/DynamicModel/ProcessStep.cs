using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

namespace P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;

public record ProcessStep(HierarchyId Id) : Element
{
    public string Name => Id.Name;
    
    public record HasNextStep(ProcessStep Source, ProcessStep Destination) : Relation<ProcessStep, ProcessStep>;
    
    public record BelongsToDomainModule(ProcessStep Source, DomainModule Destination) 
        : Relation<ProcessStep, DomainModule>;
}