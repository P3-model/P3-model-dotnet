namespace P3Model.Parser.ModelSyntax.Domain.StaticModel.Ddd;

public class DddDomainService : DomainBuildingBlock
{
    public DddDomainService(string name) : base(name) { }
    public DddDomainService(string id, string name) : base(id, name) { }
}