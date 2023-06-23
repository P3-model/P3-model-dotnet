using System.Collections.Generic;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.ModelQuerying.Queries.Relations;

public interface RelationsQuery<TRelation>
    where TRelation : Relation
{
    IReadOnlySet<TRelation> Execute(ModelGraph modelGraph);
}