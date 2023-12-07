namespace P3Model.Parser.ModelSyntax.Domain.StaticModel.Ddd;

public record DddFactory(DomainModule? Module, string Name) : DomainBuildingBlock(Module, Name);