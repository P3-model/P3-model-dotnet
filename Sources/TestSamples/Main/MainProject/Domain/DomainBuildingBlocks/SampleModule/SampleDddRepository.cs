using P3Model.Annotations.Domain.StaticModel.DDD;

namespace TestSamples.MainProject.Domain.DomainBuildingBlocks.SampleModule;

[DddRepository]
public class SampleDddRepository
{
    public Task<SampleDddAggregate> GetSampleAggregate() => Task.FromResult(new SampleDddAggregate());
}