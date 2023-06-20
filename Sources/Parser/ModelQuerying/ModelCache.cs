using System.Linq;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

namespace P3Model.Parser.ModelQuerying;

public class ModelCache
{
    private readonly Model _model;

    public ModelCache(Model model) => _model = model;

    private Product? _product;
    public Product Product => _product ??= _model.Elements.OfType<Product>().Single();
    
    private Hierarchy<DomainModule>? _domainModulesHierarchy;
    public Hierarchy<DomainModule> DomainModulesHierarchy => _domainModulesHierarchy ??= Hierarchy<DomainModule>.Create(
        _model.Relations.OfType<DomainModule.ContainsDomainModule>());
    
    private DomainBuildingBlocks? _domainBuildingBlocks;
    public DomainBuildingBlocks DomainBuildingBlocks => _domainBuildingBlocks ??= new DomainBuildingBlocks(
        _model.Relations.OfType<DomainModule.ContainsBuildingBlock>());

    private ProcessesHierarchy? _processesHierarchy;
    public ProcessesHierarchy ProcessesHierarchy => _processesHierarchy ??= new ProcessesHierarchy(
        _model.Relations.OfType<Process.ContainsSubProcess>());
}