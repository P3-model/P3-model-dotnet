using System.Text;
using NUnit.Framework;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.OutputFormatting;

namespace P3Model.Parser.Tests.CodeAnalysis;

public class ParserOutput : OutputFormatter
{
    private static Model? _model;
    private static Model Model => _model ?? throw new InvalidOperationException("Parser has not finished yet");

    public static void AssertExistOnly<TElement>(params TElement[] expectedElements)
        where TElement : class, Element
    {
        var allElements = Model.Elements.OfType<TElement>().ToHashSet();
        var missingElements = expectedElements
            .Where(expectedElement => !allElements.Contains(expectedElement))
            .ToList();
        var unexpectedElements = allElements
            .Except(expectedElements)
            .ToList();
        if (!missingElements.Any() && !unexpectedElements.Any())
            return;
        var message = CreateFailMassage(missingElements, unexpectedElements);
        Assert.Fail(message);
    }

    private static string CreateFailMassage(IReadOnlyCollection<Element> missingElements,
        IReadOnlyCollection<Element> unexpectedElements)
    {
        var messageBuilder = new StringBuilder();
        messageBuilder.AppendLine("Model has incorrect elements.");
        if (missingElements.Any())
        {
            messageBuilder.AppendLine();
            messageBuilder.AppendLine("Missing elements:");
            foreach (var element in missingElements)
                messageBuilder.AppendLine(element.ToString());
        }
        if (unexpectedElements.Any())
        {
            messageBuilder.AppendLine();
            messageBuilder.AppendLine("Unexpected elements:");
            foreach (var element in unexpectedElements)
                messageBuilder.AppendLine(element.ToString());
        }
        return messageBuilder.ToString();
    }

    public Task Clean() => Task.CompletedTask;

    public Task Write(Model model)
    {
        _model = model;
        return Task.CompletedTask;
    }
}