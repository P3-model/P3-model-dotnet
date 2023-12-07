namespace P3Model.Parser.ModelSyntax.Domain.StaticModel.Ddd;

public record DddAggregate(DomainModule? Module, string Name) : DomainBuildingBlock(Module, Name);