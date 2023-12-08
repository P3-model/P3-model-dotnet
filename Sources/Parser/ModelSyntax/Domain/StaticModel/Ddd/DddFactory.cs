namespace P3Model.Parser.ModelSyntax.Domain.StaticModel.Ddd;

public class DddFactory : DomainBuildingBlock
{
    public DddFactory(string name) : base(name) { }
    public DddFactory(string id, string name) : base(id, name) { }
}