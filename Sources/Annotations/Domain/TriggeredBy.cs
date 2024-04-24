using JetBrains.Annotations;

namespace P3Model.Annotations.Domain;

[PublicAPI]
public enum TriggeredBy
{
    Unspecified,
    Command,
    Query,
    Event
}