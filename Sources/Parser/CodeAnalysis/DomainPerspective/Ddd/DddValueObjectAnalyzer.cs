using System;
using System.IO;
using P3Model.Annotations.Domain.StaticModel.DDD;
using P3Model.Parser.ModelSyntax.DomainPerspective;
using P3Model.Parser.ModelSyntax.DomainPerspective.Ddd;

namespace P3Model.Parser.CodeAnalysis.DomainPerspective.Ddd;

public class DddValueObjectAnalyzer : DddBuildingBlockAnalyzer
{
    protected override Type AttributeType => typeof(DddValueObjectAttribute);

    protected override DomainBuildingBlock CreateBuildingBlock(string name, FileInfo? descriptionFile) =>
        new DddValueObject(name, descriptionFile);
}