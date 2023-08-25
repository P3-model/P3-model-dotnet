namespace P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel.Ddd;

public record DddAggregate(DomainModule? Module, string Name) : DomainBuildingBlock(Module, Name);