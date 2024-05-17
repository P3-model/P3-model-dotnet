using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Domain;

namespace P3Model.Parser.Tests.CodeAnalysis.DomainPerspective;

public static class ProcessInstances
{
    public static readonly Process SampleProcess = new(
        ElementId.Create<Process>("SampleProcess"),
        "Sample Process");
}