using P3Model.Parser.ModelSyntax.DomainPerspective.DynamicModel;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;

namespace P3Model.Parser.ModelSyntax;

public class ModelCache
{
    private readonly Model _model;

    public ModelCache(Model model) => _model = model;

    private DomainModulesHierarchy? _domainModulesHierarchy;
    public DomainModulesHierarchy DomainModulesHierarchy => _domainModulesHierarchy ??= new DomainModulesHierarchy(
        _model.Relations.OfType<DomainModule.ContainsDomainModule>());
    
    private DomainBuildingBlocks? _domainBuildingBlocks;
    public DomainBuildingBlocks DomainBuildingBlocks => _domainBuildingBlocks ??= new DomainBuildingBlocks(
        _model.Relations.OfType<DomainModule.ContainsBuildingBlock>());

    private ProcessesHierarchy? _processesHierarchy;
    public ProcessesHierarchy ProcessesHierarchy => _processesHierarchy ??= new ProcessesHierarchy(
        _model.Relations.OfType<Process.ContainsSubProcess>());
}