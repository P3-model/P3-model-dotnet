using System.Collections.Immutable;

namespace P3Model.Parser.ModelSyntax;

public record Model(ImmutableArray<Element> Elements,
    ImmutableArray<Relation> Relations);