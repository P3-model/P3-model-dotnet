using P3Model.Annotations;
using P3Model.Annotations.Domain.StaticModel.DDD;

namespace TestSamples.MainProject.Domain.DomainBuildingBlocks.SampleModule;

[DddAggregate]
[ShortDescription("*lorem ipsum* **dolor** sit amet")]
public class SampleDddAggregate
{
    public void BehaviorDependantOnDomainService(SampleDddDomainService domainService) { }
}