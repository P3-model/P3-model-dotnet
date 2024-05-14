using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.ModelQuerying.Queries.Relations;

public class GetRelations<TRelation> : RelationsQuery<TRelation>
    where TRelation : Relation
{
    private readonly Func<TRelation, bool>? _predicate;

    public GetRelations(Func<TRelation, bool>? predicate) => _predicate = predicate;

    public IReadOnlySet<TRelation> Execute(ModelGraph modelGraph) => _predicate is null
        ? modelGraph
            .RelationsOfType<TRelation>()
            .ToHashSet()
        : modelGraph
            .RelationsOfType<TRelation>()
            .Where(r => _predicate(r))
            .ToHashSet();
}