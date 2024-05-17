using NUnit.Framework;
using P3Model.Parser.ModelSyntax.Domain;
using static P3Model.Parser.Tests.CodeAnalysis.DomainPerspective.DomainBuildingBlockInstances;
using static P3Model.Parser.Tests.CodeAnalysis.DomainPerspective.ProcessInstances;

namespace P3Model.Parser.Tests.CodeAnalysis.DomainPerspective;

[TestFixture]
public class ProcessTests
{
    [Test]
    public void AllProcessesArePresent() => ParserOutput.AssertModelContainsOnlyElements(
        SampleProcess);

    [Test]
    public void AllContainsUseCaseRelationsArePresent() => ParserOutput.AssertModelContainsOnlyRelations(
        new Process.ContainsUseCase(SampleProcess, SampleUseCase));
}