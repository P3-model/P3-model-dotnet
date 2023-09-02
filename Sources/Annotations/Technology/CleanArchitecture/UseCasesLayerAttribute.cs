using JetBrains.Annotations;

namespace P3Model.Annotations.Technology.CleanArchitecture;

[PublicAPI]
public class UseCasesLayerAttribute : LayerAttribute
{
    public new const string Name = "Use Cases";
    
    public UseCasesLayerAttribute() : base(Name) { }
}