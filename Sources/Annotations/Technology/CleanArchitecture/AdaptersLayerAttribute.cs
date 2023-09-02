using JetBrains.Annotations;

namespace P3Model.Annotations.Technology.CleanArchitecture;

[PublicAPI]
public class AdaptersLayerAttribute : LayerAttribute
{
    public new const string Name = "Adapters";
    
    public AdaptersLayerAttribute() : base(Name) { }
}