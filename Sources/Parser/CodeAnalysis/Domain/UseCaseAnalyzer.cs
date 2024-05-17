using Humanizer;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Domain;
using P3Model.Parser.CodeAnalysis.RoslynExtensions;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain;

namespace P3Model.Parser.CodeAnalysis.Domain;

[UsedImplicitly]
public class UseCaseAnalyzer(DomainModulesHierarchyResolver modulesHierarchyResolver)
    : DomainBuildingBlockAnalyzerBase<UseCase>(modulesHierarchyResolver)
{
    protected override Type AttributeType => typeof(UseCaseAttribute);

    protected override UseCase CreateBuildingBlock(string idPartUniqueForElementType, string name) => 
        new(ElementId.Create<UseCase>(idPartUniqueForElementType), name);

    protected override void AddElementsAndRelations(UseCase useCase, DomainModule? module, ISymbol symbol,
        AttributeData buildingBlockAttribute, ModelBuilder modelBuilder)
    {
        base.AddElementsAndRelations(useCase, module, symbol, buildingBlockAttribute, modelBuilder);
        if (!buildingBlockAttribute.TryGetArgumentValue(nameof(UseCaseAttribute.Process), out string? processName))
            return;
        var process = new Process(
            ElementId.Create<Process>(processName.Dehumanize()),
            processName.Humanize(LetterCasing.Title));
        modelBuilder.Add(process);
        modelBuilder.Add(new Process.ContainsUseCase(process, useCase));
    }
}