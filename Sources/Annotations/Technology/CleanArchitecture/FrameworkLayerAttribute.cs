using JetBrains.Annotations;

namespace P3Model.Annotations.Technology.CleanArchitecture;

[PublicAPI]
public class FrameworkLayerAttribute : LayerAttribute
{
    public FrameworkLayerAttribute() : base("Framework") { }
}