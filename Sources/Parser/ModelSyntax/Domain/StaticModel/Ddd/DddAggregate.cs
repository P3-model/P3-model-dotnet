namespace P3Model.Parser.ModelSyntax.Domain.StaticModel.Ddd;

public class DddAggregate : DomainBuildingBlock
{
    public DddAggregate(string name) : base(name) { }
    public DddAggregate(string id, string name) : base(id, name) { }
}