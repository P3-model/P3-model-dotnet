using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.ModelQuerying.Queries.Traits;

public interface TraitsQuery<TTrait>
    where TTrait : Trait
{
    IReadOnlySet<TTrait> Execute(ModelGraph modelGraph);
}