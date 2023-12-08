namespace P3Model.Parser.ModelSyntax.Domain.StaticModel.Ddd;

public class DddRepository : DomainBuildingBlock
{
    public DddRepository(string name) : base(name) { }
    public DddRepository(string id, string name) : base(id, name) { }
}