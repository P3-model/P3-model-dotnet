using P3Model.Annotations.Domain;

namespace TestSamples.MainProject.Domain.DomainBuildingBlocks.SampleModule;

public class SampleUseCase(SampleDddRepository repository)
{
    [UseCase(nameof(SampleUseCase))]
    public async Task Execute()
    {
        var sampleAggregate = await repository.GetSampleAggregate();
        sampleAggregate.BehaviorDependantOnDomainService(new SampleDddDomainService());

        var sampleValueObjects = new List<SampleDddValueObject[]>
        {
            new[] { new SampleDddValueObject() }
        };
        PrivateMethod(sampleValueObjects);
        PrivateStaticMethod(new SampleDddFactory().Create());
    }

    private static void PrivateStaticMethod(SampleDddEntity entity) { }
    private static void PrivateMethod(IEnumerable<SampleDddValueObject[]> valueObjects) { }
}