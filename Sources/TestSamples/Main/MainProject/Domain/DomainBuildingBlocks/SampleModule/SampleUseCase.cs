using P3Model.Annotations.Domain;

namespace TestSamples.MainProject.Domain.DomainBuildingBlocks.SampleModule;

public class SampleUseCase(SampleDddRepository repository)
{
    private SampleDddFactory? Factory { get; set; }

    [UseCase(nameof(SampleUseCase))]
    public async Task Execute()
    {
        var sampleAggregate = await repository.GetSampleAggregate();
        sampleAggregate.BehaviorDependantOnDomainService(new SampleDddDomainService());

        var sampleValueObject = new List<SampleDddValueObject[]>
        {
            new[] { new SampleDddValueObject() }
        };
        PrivateStaticMethod(new SampleDddEntity());
    }

    private static void PrivateStaticMethod(SampleDddEntity entity) { }
}