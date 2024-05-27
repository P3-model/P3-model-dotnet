using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.Tests.CodeAnalysis.Technology;

public static class DatabaseInstances
{
    public static readonly Database MainDatabase = new(ElementId.Create<Database>("main"),  "Main");
}