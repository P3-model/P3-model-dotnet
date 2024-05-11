using P3Model.Annotations.Domain;

namespace TestSamples.MainProject.Domain.DomainModules.NotModule.WithCode.SkippedWithAnnotation;


[SkipNamespaceInDomainModulesHierarchy(ApplyOnNamespace = true)]
[DomainBuildingBlock]
public class BuildingBlockFromSkippedNamespace;