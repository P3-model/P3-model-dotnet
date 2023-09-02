using JetBrains.Annotations;

namespace P3Model.Annotations.Technology.CleanArchitecture;

[PublicAPI]
public class EntitiesLayerAttribute : LayerAttribute
{
    public EntitiesLayerAttribute() : base("Entities") { }
}