namespace P3Model.Parser.ModelSyntax.Domain;

public class ModelBoundary(ElementId id, string name) : ElementBase(id, name)
{
    public override Perspective Perspective => Perspective.Domain;
    
    public class ContainsDomainModule(ModelBoundary source, DomainModule destination) 
        : RelationBase<ModelBoundary, DomainModule>(source, destination);

    public class ContainsBuildingBlock(ModelBoundary source, DomainBuildingBlock destination) 
        : RelationBase<ModelBoundary, DomainBuildingBlock>(source, destination);
}