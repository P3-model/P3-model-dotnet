namespace P3Model.Parser.ModelSyntax.Domain.StaticModel.Ddd;

public record DddEntity(DomainModule? Module, string Name) : DomainBuildingBlock(Module, Name);