using P3Model.Annotations.Technology;

[assembly: DeployableUnit("web-app")]

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();