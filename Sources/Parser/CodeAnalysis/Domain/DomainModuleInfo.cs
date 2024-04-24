using System;

namespace P3Model.Parser.CodeAnalysis.Domain;

public class DomainModuleInfo(string domainModule)
{
    public string DomainModule { get; } = domainModule ?? throw new ArgumentNullException(nameof(domainModule));
    public string? BusinessOwner { get; init; }
    public string? DevelopmentOwner { get; init; }
}