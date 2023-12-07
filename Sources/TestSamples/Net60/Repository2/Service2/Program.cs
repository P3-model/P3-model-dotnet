using P3Model.Annotations.Technology;

[assembly: DeployableUnit("Service2")]

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();