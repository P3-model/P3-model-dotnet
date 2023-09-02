using JetBrains.Annotations;

namespace P3Model.Annotations.Technology.CleanArchitecture;

[PublicAPI]
public class UseCasesLayerAttribute : LayerAttribute
{
    public UseCasesLayerAttribute() : base("UseCases") { }
}