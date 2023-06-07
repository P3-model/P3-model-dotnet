using System.IO;

namespace P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel.Ddd;

public record DddDomainService(string Name, FileInfo? DescriptionFile) : DomainBuildingBlock(Name, DescriptionFile);