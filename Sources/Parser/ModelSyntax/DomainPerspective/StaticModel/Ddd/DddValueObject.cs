namespace P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel.Ddd;

public record DddValueObject(DomainModule? Module, string Name) : DomainBuildingBlock(Module, Name);