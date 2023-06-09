using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;

namespace P3Model.Parser.ModelSyntax.Technology;

public record CodeStructure(string Name) : Element
{
    public record ImplementsProcessStep(CodeStructure CodeStructure, ProcessStep ProcessStep);
}