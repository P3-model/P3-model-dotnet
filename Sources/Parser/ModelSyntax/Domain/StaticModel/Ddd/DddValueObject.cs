namespace P3Model.Parser.ModelSyntax.Domain.StaticModel.Ddd;

public class DddValueObject : DomainBuildingBlock
{
    public DddValueObject(string name) : base(name) { }
    public DddValueObject(string id, string name) : base(id, name) { }
}