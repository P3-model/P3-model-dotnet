using P3Model.Annotations.Technology;

[assembly: DeployableUnit("Service1-microservice")]

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();