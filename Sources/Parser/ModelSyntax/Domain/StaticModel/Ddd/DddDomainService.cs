namespace P3Model.Parser.ModelSyntax.Domain.StaticModel.Ddd;

public record DddDomainService(DomainModule? Module, string Name) : DomainBuildingBlock(Module, Name);