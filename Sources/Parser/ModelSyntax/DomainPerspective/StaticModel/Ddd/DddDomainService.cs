namespace P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel.Ddd;

public record DddDomainService(DomainModule? Module, string Name) : DomainBuildingBlock(Module, Name);