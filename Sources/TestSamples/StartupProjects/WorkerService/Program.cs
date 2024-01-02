using P3Model.Annotations.Technology;
using TestSamples.WorkerService;

[assembly: DeployableUnit("worker-service")]

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => { services.AddHostedService<Worker>(); })
    .Build();

await host.RunAsync();