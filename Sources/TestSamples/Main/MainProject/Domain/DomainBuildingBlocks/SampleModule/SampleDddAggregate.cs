using P3Model.Annotations;
using P3Model.Annotations.Domain.DDD;

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
    private readonly SampleDddValueObject _valueObject = new();
    
    public void BehaviorDependantOnDomainService(SampleDddDomainService domainService) { }
}