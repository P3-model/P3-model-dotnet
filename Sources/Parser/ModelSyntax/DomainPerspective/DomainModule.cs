using System.Linq;

namespace P3Model.Parser.ModelSyntax.DomainPerspective;

public record DomainModule(string ModulesHierarchy) : Element
{
    public string Name => ModulesHierarchy[(ModulesHierarchy.LastIndexOf('.') + 1 )..];

    public int Level => ModulesHierarchy.Count(c => c == '.');

    public record ContainsDomainModule(DomainModule Parent, DomainModule Child) : Relation;
}