using P3Model.Annotations.Domain.StaticModel;

namespace TestSamples.MainProject.Domain.DomainBuildingBlocks.SampleModule;

[ProcessStep]
public class SampleProcessStep
{
    private readonly SampleDddRepository _repository;
    private SampleDddFactory? Factory { get; set; }

    public SampleProcessStep(SampleDddRepository repository) => _repository = repository;

    public async Task Execute()
    {
        var sampleAggregate = await _repository.GetSampleAggregate();
        sampleAggregate.BehaviorDependantOnDomainService(new SampleDddDomainService());

        var sampleValueObject = new List<SampleDddValueObject[]>
        {
            new[] { new SampleDddValueObject() }
        };
        PrivateStaticMethod(new SampleDddEntity());
    }

    private static void PrivateStaticMethod(SampleDddEntity entity) { }
}