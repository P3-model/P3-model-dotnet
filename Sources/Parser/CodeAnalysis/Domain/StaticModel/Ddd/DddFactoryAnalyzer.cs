using System;
using JetBrains.Annotations;
using P3Model.Annotations.Domain.StaticModel.DDD;
using P3Model.Parser.ModelSyntax.Domain.StaticModel;
using P3Model.Parser.ModelSyntax.Domain.StaticModel.Ddd;

namespace P3Model.Parser.CodeAnalysis.Domain.StaticModel.Ddd;

[UsedImplicitly]
public class DddFactoryAnalyzer : DomainBuildingBlockAnalyzerBase
{
    protected override Type AttributeType => typeof(DddFactoryAttribute);

    public DddFactoryAnalyzer(DomainModuleFinder moduleFinder) : base(moduleFinder) { }

    protected override DomainBuildingBlock CreateBuildingBlock(DomainModule? module, string name) => 
        new DddFactory(module, name);
}