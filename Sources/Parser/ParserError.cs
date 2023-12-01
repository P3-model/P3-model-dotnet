using System;

namespace P3Model.Parser;

public class ParserError : Exception
{
    public ParserError(string message) : base(message) { }
}