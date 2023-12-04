using System.Text;
using NUnit.Framework;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.OutputFormatting;

namespace P3Model.Parser.Tests.CodeAnalysis;

public class ParserOutput : OutputFormatter
{
    private static readonly HashSet<Element> CheckedElements = new();
    private static Model? _model;
    private static Model Model => _model ?? throw new InvalidOperationException("Parser has not finished yet");

    public static void AssertExists(params Element[] elements)
    {
        var missingElements = new List<Element>();
        foreach (var element in elements)
        {
            if (Model.Elements.Contains(element))
                CheckedElements.Add(element);
            else
                missingElements.Add(element);
        }
        if (!missingElements.Any()) 
            return;
        var message = CreateFailMassage("Missing elements:", missingElements);
        Assert.Fail(message);

    }

    public static void AssertAllElementsAreChecked()
    {
        var unexpectedElements = Model.Elements.Where(element => !CheckedElements.Contains(element)).ToList();
        if (!unexpectedElements.Any())
            return;
        var message = CreateFailMassage("Unexpected elements:", unexpectedElements);
        Assert.Fail(message);
    }

    private static string CreateFailMassage(string header, IEnumerable<Element> elements)
    {
        var messageBuilder = new StringBuilder();
        messageBuilder.AppendLine(header);
        foreach (var element in elements)
            messageBuilder.AppendLine(element.ToString());
        return messageBuilder.ToString();
    }

    public Task Clean() => Task.CompletedTask;

    public Task Write(Model model)
    {
        _model = model;
        return Task.CompletedTask;
    }
}