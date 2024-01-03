using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.ModelSyntax.Domain.DynamicModel;

public class Process(string name) : ElementBase(name)
{
    public override Perspective Perspective => Perspective.Domain;

    public record ContainsProcessStep(Process Source, ProcessStep Destination) : Relation<Process, ProcessStep>;
}