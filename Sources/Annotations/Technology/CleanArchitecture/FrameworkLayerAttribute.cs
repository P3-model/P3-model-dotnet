using JetBrains.Annotations;

namespace P3Model.Annotations.Technology.CleanArchitecture;

[PublicAPI]
public class FrameworkLayerAttribute : LayerAttribute
{
    public new const string Name = "Framework";

    public FrameworkLayerAttribute() : base(Name) { }
}