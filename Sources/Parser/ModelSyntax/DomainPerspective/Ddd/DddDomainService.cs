using System.IO;

namespace P3Model.Parser.ModelSyntax.DomainPerspective.Ddd;

public record DddDomainService(string Name, FileInfo? DescriptionFile) : BuildingBlock(Name, DescriptionFile);