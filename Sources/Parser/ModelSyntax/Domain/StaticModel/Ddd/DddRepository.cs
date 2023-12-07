namespace P3Model.Parser.ModelSyntax.Domain.StaticModel.Ddd;

public record DddRepository(DomainModule? Module, string Name) : DomainBuildingBlock(Module, Name);