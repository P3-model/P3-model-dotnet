using System;

namespace P3Model.Parser.CodeAnalysis.DomainPerspective.StaticModel;

public class DomainModuleInfo
{
    public string DomainModule { get; }
    public string? BusinessOwner { get; init; }
    public string? DevelopmentOwner { get; init; }

    public DomainModuleInfo(string domainModule) =>
        DomainModule = domainModule ?? throw new ArgumentNullException(nameof(domainModule));
}