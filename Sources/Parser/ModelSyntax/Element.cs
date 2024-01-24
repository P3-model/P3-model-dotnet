using System;

namespace P3Model.Parser.ModelSyntax;

public interface Element : IEquatable<Element>
{
    Perspective Perspective { get; }
    string Id { get; }
    string Name { get; }
    
    bool DataEquals(Element? element);
}