using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

namespace P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;

// TODO: unique names across hierarchy
public record ProcessStep(string Name) : Element
{
    public record HasNextStep(ProcessStep Current, ProcessStep Next) : Relation;
    
    public record BelongsToDomainModule(ProcessStep Step, DomainModule DomainModule) : Relation;
}