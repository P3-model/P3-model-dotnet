using P3Model.Annotations.Technology;

[assembly: DeployableUnit("MySystem-main-monolith")]

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();