using P3Model.Annotations.Technology;

[assembly: DeployableUnit("Module3-microservice")]

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();