using System;
using System.IO;
using JetBrains.Annotations;
using P3Model.Annotations.Domain.StaticModel.DDD;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel.Ddd;

namespace P3Model.Parser.CodeAnalysis.DomainPerspective.StaticModel.Ddd;

[UsedImplicitly]
public class DddAggregateAnalyzer : DddBuildingBlockAnalyzer
{
    protected override Type AttributeType => typeof(DddAggregateAttribute);

    protected override DomainBuildingBlock CreateBuildingBlock(string name, FileInfo? descriptionFile) =>
        new DddAggregate(name, descriptionFile);
}