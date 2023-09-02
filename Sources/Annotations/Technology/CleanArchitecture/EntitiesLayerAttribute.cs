using JetBrains.Annotations;

namespace P3Model.Annotations.Technology.CleanArchitecture;

[PublicAPI]
public class EntitiesLayerAttribute : LayerAttribute
{
    public new const string Name = "Entities";
    
    public EntitiesLayerAttribute() : base(Name) { }
}