using System;
using System.IO;
using JetBrains.Annotations;
using P3Model.Annotations.Domain.StaticModel.DDD;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel.Ddd;

namespace P3Model.Parser.CodeAnalysis.DomainPerspective.StaticModel.Ddd;

[UsedImplicitly]
public class DddFactoryAnalyzer : DddBuildingBlockAnalyzer
{
    protected override Type AttributeType => typeof(DddFactoryAttribute);

    protected override DomainBuildingBlock CreateBuildingBlock(string name, FileInfo? descriptionFile) =>
        new DddFactory(name, descriptionFile);
}