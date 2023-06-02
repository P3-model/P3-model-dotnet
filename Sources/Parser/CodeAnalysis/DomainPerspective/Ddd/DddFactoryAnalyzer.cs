using System;
using System.IO;
using P3Model.Annotations.Domain.StaticModel.DDD;
using P3Model.Parser.ModelSyntax.DomainPerspective;
using P3Model.Parser.ModelSyntax.DomainPerspective.Ddd;

namespace P3Model.Parser.CodeAnalysis.DomainPerspective.Ddd;

public class DddFactoryAnalyzer : DddBuildingBlockAnalyzer
{
    protected override Type AttributeType => typeof(DddFactoryAttribute);

    protected override BuildingBlock CreateBuildingBlock(string name, FileInfo? descriptionFile) =>
        new DddFactory(name, descriptionFile);
}