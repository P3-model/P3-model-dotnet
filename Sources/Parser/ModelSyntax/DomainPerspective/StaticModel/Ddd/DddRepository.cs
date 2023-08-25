namespace P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel.Ddd;

public record DddRepository(DomainModule? Module, string Name) : DomainBuildingBlock(Module, Name);