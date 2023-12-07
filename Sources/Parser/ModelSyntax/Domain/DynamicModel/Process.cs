using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.ModelSyntax.Domain.DynamicModel;

public record Process(string Name) : Element
{
    public Perspective Perspective => Perspective.Domain;
    public string Id => Name;

    public record ContainsProcessStep(Process Source, ProcessStep Destination) : Relation<Process, ProcessStep>;
}