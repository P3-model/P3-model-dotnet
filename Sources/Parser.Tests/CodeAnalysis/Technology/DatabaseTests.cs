using NUnit.Framework;
using static P3Model.Parser.Tests.CodeAnalysis.Technology.DatabaseInstances;

namespace P3Model.Parser.Tests.CodeAnalysis.Technology;

[TestFixture]
public class DatabaseTests
{
    [Test]
    public void AllDatabasesArePresent() => ParserOutput.AssertModelContainsOnlyElements(
        MainDatabase
    );
}