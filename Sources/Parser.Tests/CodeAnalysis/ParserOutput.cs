using System.Collections.Concurrent;
using System.Text;
using NUnit.Framework;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.OutputFormatting;

namespace P3Model.Parser.Tests.CodeAnalysis;

public sealed class ParserOutput : OutputFormatter
{
    public static ParserOutput Instance { get; } = new();

    private static readonly ConcurrentDictionary<TargetFramework, Model> Models = new();

    public static Model GetModelFor(TargetFramework targetFramework)
    {
        if (Models.TryGetValue(targetFramework, out var model))
            return model;
        throw new InvalidOperationException($"Missing model for target framework {targetFramework}");
    }

    private ParserOutput() { }

    public static void AssertExistOnly<TElement>(params TElement[] expectedElements)
        where TElement : class, Element
    {
        foreach (var targetFramework in TargetFramework.All)
            AssertExistOnly(targetFramework, expectedElements);
    }

    public static void AssertExistOnly<TElement>(TargetFramework targetFramework, params TElement[] expectedElements)
        where TElement : class, Element
    {
        var allElements = GetModelFor(targetFramework)
            .Elements
            .OfType<TElement>()
            .ToHashSet();
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

    public Task Write(TargetFramework? defaultFramework, Model model)
    {
        if (!defaultFramework.HasValue)
            throw new InvalidOperationException("Default framework was not specified");
        if (!Models.TryAdd(defaultFramework.Value, model))
            throw new InvalidOperationException($"Duplicated model for {defaultFramework}");
        return Task.CompletedTask;
    }
}