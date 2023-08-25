namespace P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel.Ddd;

public record DddFactory(DomainModule? Module, string Name) : DomainBuildingBlock(Module, Name);