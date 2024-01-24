using System.Collections.Concurrent;
using System.Text;
using JetBrains.Annotations;
using NUnit.Framework;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.OutputFormatting;

namespace P3Model.Parser.Tests.CodeAnalysis;

public sealed class ParserOutput : OutputFormatter
{
    public static ParserOutput Instance { get; } = new();

    private static readonly ConcurrentDictionary<TargetFramework, Model> Models = new();
    
    private ParserOutput() { }

    [PublicAPI]
    public static void AssertModelContainsOnlyElements<TElement>(params TElement[] expectedElements)
        where TElement : class, Element
    {
        foreach (var targetFramework in TargetFramework.All)
            AssertModelContainsOnlyElements(targetFramework, expectedElements);
    }

    [PublicAPI]
    public static void AssertModelContainsOnlyElements<TElement>(TargetFramework targetFramework,
        params TElement[] expectedElements)
        where TElement : class, Element
    {
        var equalityComparer = new ElementBase.DataEqualityComparer();
        var allElements = GetModelFor(targetFramework)
            .Elements
            .OfType<TElement>()
            .ToHashSet(equalityComparer);
        var missingElements = expectedElements
            .Where(expectedElement => !allElements.Contains(expectedElement))
            .ToList();
        var unexpectedElements = allElements
            .Except(expectedElements, equalityComparer)
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

    [PublicAPI]
    public static void AssertModelContainsOnlyRelations<TRelation>(params TRelation[] expectedRelations)
        where TRelation : class, Relation
    {
        foreach (var targetFramework in TargetFramework.All)
            AssertModelContainsOnlyRelations(targetFramework, expectedRelations);
    }

    [PublicAPI]
    public static void AssertModelContainsOnlyRelations<TRelation>(TargetFramework targetFramework,
        params TRelation[] expectedRelations)
        where TRelation : class, Relation
    {
        var allRelations = GetModelFor(targetFramework)
            .Relations
            .OfType<TRelation>()
            .ToHashSet();
        var missingRelations = expectedRelations
            .Where(expectedRelation => !allRelations.Contains(expectedRelation))
            .ToList();
        var unexpectedRelations = allRelations
            .Except(expectedRelations)
            .ToList();
        if (!missingRelations.Any() && !unexpectedRelations.Any())
            return;
        var message = CreateFailMassage(missingRelations, unexpectedRelations);
        Assert.Fail(message);
    }
    
    private static string CreateFailMassage(IReadOnlyCollection<Relation> missingRelations,
        IReadOnlyCollection<Relation> unexpectedRelations)
    {
        var messageBuilder = new StringBuilder();
        messageBuilder.AppendLine("Model has incorrect relations.");
        if (missingRelations.Any())
        {
            messageBuilder.AppendLine();
            messageBuilder.AppendLine("Missing relations:");
            foreach (var relation in missingRelations)
                messageBuilder.AppendLine(relation.ToString());
        }
        if (unexpectedRelations.Any())
        {
            messageBuilder.AppendLine();
            messageBuilder.AppendLine("Unexpected relations:");
            foreach (var relation in unexpectedRelations)
                messageBuilder.AppendLine(relation.ToString());
        }
        return messageBuilder.ToString();
    }

    private static Model GetModelFor(TargetFramework targetFramework)
    {
        if (Models.TryGetValue(targetFramework, out var model))
            return model;
        throw new InvalidOperationException($"Missing model for target framework {targetFramework}");
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