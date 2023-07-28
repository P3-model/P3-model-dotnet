using System;

namespace P3Model.Parser.CodeAnalysis.DomainPerspective;

public class ModelBoundaryInfo
{
    public string ModelBoundary { get; }
    public string? BusinessOwner { get; init; }
    public string? DevelopmentOwner { get; init; }

    public ModelBoundaryInfo(string modelBoundary) =>
        ModelBoundary = modelBoundary ?? throw new ArgumentNullException(nameof(modelBoundary));
}