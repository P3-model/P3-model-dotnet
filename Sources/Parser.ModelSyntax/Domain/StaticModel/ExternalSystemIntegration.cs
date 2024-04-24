using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.ModelSyntax.Domain.StaticModel;

public class ExternalSystemIntegration(ElementId id, string name) : DomainBuildingBlock(id, name)
{
    public class Integrates(ExternalSystemIntegration source, ExternalSystem destination) 
        : RelationBase<ExternalSystemIntegration, ExternalSystem>(source, destination);
}