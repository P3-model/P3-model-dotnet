using JetBrains.Annotations;

namespace P3Model.Annotations.Technology.CleanArchitecture;

[PublicAPI]
public class AdaptersLayerAttribute : LayerAttribute
{
    public AdaptersLayerAttribute() : base("Adapters") { }
}