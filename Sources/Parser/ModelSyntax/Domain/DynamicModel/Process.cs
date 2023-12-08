using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.ModelSyntax.Domain.DynamicModel;

public class Process : ElementBase
{
    public override Perspective Perspective => Perspective.Domain;

    public Process(string name) : base(name) { }
    public Process(string id, string name) : base(id, name) { }

    public record ContainsProcessStep(Process Source, ProcessStep Destination) : Relation<Process, ProcessStep>;
}