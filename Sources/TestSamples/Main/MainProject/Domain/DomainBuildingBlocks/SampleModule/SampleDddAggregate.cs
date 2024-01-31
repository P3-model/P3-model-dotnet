using P3Model.Annotations;
using P3Model.Annotations.Domain.StaticModel.DDD;

namespace TestSamples.MainProject.Domain.DomainBuildingBlocks.SampleModule;

[DddAggregate]
[ShortDescription("""
                  *lorem ipsum* **dolor** sit amet ...
                  - item 1
                  - item 2
                  ... consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
                  """)]
public class SampleDddAggregate
{
    public void BehaviorDependantOnDomainService(SampleDddDomainService domainService) { }
}