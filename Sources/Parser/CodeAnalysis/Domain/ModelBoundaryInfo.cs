namespace P3Model.Parser.CodeAnalysis.Domain;

public class ModelBoundaryInfo(string modelBoundary)
{
    public string ModelBoundary { get; } = modelBoundary ?? throw new ArgumentNullException(nameof(modelBoundary));
    public string? BusinessOwner { get; init; }
    public string? DevelopmentOwner { get; init; }
}