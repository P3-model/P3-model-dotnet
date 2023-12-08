namespace P3Model.Parser.ModelSyntax.Domain.StaticModel.Ddd;

public class DddEntity : DomainBuildingBlock
{
    public DddEntity(string name) : base(name) { }
    public DddEntity(string id, string name) : base(id, name) { }
}