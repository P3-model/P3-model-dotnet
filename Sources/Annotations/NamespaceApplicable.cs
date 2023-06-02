using JetBrains.Annotations;

namespace P3Model.Annotations;

[PublicAPI]
public interface NamespaceApplicable
{
    public bool ApplyOnNamespace { get; }
}