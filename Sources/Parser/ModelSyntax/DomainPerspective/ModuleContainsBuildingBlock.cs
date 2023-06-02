namespace P3Model.Parser.ModelSyntax.DomainPerspective;

public record ModuleContainsBuildingBlock(DomainModule DomainModule, BuildingBlock BuildingBlock) : Relation;