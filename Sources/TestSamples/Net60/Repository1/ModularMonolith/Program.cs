using P3Model.Annotations.Technology;

[assembly: DeployableUnit("DeployableUnit1")]

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();