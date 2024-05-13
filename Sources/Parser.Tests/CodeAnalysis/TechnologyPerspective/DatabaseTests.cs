using NUnit.Framework;
using static P3Model.Parser.Tests.CodeAnalysis.TechnologyPerspective.DatabaseInstances;

namespace P3Model.Parser.Tests.CodeAnalysis.TechnologyPerspective;

[TestFixture]
public class DatabaseTests
{
    [Test]
    public void AllDatabasesArePresent() => ParserOutput.AssertModelContainsOnlyElements(
        MainDatabase
    );
}