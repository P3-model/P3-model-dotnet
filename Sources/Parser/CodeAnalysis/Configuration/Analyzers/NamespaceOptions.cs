using System;
using Microsoft.CodeAnalysis;

namespace P3Model.Parser.CodeAnalysis.Configuration.Analyzers;

public record NamespaceOptions(Func<INamespaceSymbol, string> Filter);