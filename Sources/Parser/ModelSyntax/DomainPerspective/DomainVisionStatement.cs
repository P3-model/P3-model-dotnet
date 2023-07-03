using System.IO;

namespace P3Model.Parser.ModelSyntax.DomainPerspective;

public record DomainVisionStatement(Product Element, FileInfo SourceFile) : Trait<Product>;