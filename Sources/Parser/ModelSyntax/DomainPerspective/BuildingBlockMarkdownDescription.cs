using System.IO;

namespace P3Model.Parser.ModelSyntax.DomainPerspective;

public record BuildingBlockMarkdownDescription(BuildingBlock Element, FileInfo FileInfo) : Trait<BuildingBlock>;