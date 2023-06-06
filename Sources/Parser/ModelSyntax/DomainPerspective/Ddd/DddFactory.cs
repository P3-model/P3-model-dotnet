using System.IO;

namespace P3Model.Parser.ModelSyntax.DomainPerspective.Ddd;

public record DddFactory(string Name, FileInfo? DescriptionFile) : DomainBuildingBlock(Name, DescriptionFile);