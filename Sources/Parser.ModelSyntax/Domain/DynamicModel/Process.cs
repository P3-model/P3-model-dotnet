using P3Model.Parser.ModelSyntax.Domain.StaticModel;

namespace P3Model.Parser.ModelSyntax.Domain.DynamicModel;

public class Process(ElementId id, string name) : ElementBase(id, name)
{
    public override Perspective Perspective => Perspective.Domain;

    public class ContainsUseCase(Process source, UseCase destination) 
        : RelationBase<Process, UseCase>(source, destination);
}