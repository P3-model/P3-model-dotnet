using System.IO;

namespace P3Model.Parser.ModelSyntax.DomainPerspective.Ddd;

public record DddValueObject(string Name, FileInfo? DescriptionFile) : DomainBuildingBlock(Name, DescriptionFile);