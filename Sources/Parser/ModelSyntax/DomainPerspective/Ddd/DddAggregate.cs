using System.IO;

namespace P3Model.Parser.ModelSyntax.DomainPerspective.Ddd;

public record DddAggregate(string Name, FileInfo? DescriptionFile) : DomainBuildingBlock(Name, DescriptionFile);