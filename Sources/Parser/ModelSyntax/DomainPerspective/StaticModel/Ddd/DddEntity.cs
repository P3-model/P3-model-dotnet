namespace P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel.Ddd;

public record DddEntity(DomainModule? Module, string Name) : DomainBuildingBlock(Module, Name);