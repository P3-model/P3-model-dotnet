using System.IO;

namespace P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel.Ddd;

public record DddRepository(string Name, FileInfo? DescriptionFile) : DomainBuildingBlock(Name, DescriptionFile);