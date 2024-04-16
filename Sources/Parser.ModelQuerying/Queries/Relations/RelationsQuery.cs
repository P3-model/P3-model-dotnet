using P3Model.Parser.ModelSyntax;

namespace Parser.ModelQuerying.Queries.Relations;

public interface RelationsQuery<TRelation>
    where TRelation : Relation
{
    IReadOnlySet<TRelation> Execute(ModelGraph modelGraph);
}