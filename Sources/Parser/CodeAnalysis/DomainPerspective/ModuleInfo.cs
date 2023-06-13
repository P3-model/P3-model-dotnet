using System;

namespace P3Model.Parser.CodeAnalysis.DomainPerspective;

public class ModuleInfo
{
    public string Module { get; }
    public string? BusinessOwner { get; init; }
    public string? DevelopmentOwner { get; init; }
    
    public ModuleInfo(string module) => Module = module ?? throw new ArgumentNullException(nameof(module));
}