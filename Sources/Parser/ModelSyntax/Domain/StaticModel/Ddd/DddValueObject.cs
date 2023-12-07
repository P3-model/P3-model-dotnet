namespace P3Model.Parser.ModelSyntax.Domain.StaticModel.Ddd;

public record DddValueObject(DomainModule? Module, string Name) : DomainBuildingBlock(Module, Name);