using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using JetBrains.Annotations;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.DomainPerspective.StaticModel;
using P3Model.Parser.ModelSyntax.People;

namespace P3Model.Parser.CodeAnalysis.DomainPerspective;

[UsedImplicitly]
public class ModuleInfoAnalyzer : FileAnalyzer
{
    public async Task Analyze(FileInfo fileInfo, ModelBuilder modelBuilder)
    {
        if (!fileInfo.Name.EndsWith("ModuleInfo.json"))
            return;
        await using var stream = fileInfo.Open(FileMode.Open);
        var moduleInfo = await JsonSerializer.DeserializeAsync<ModuleInfo>(stream, new JsonSerializerOptions{});
        // TODO: warning logging
        if (moduleInfo is null)
            return;
        // TODO: Rethink adding same element from different analyzers.
        var module = new DomainModule(HierarchyId.FromValue(moduleInfo.Module));
        if (moduleInfo.DevelopmentOwner != null)
        {
            var team = new DevelopmentTeam(moduleInfo.DevelopmentOwner);
            modelBuilder.Add(team);
            modelBuilder.Add(new DevelopmentTeam.OwnsDomainModule(team, module));
        }
        if (moduleInfo.BusinessOwner != null)
        {
            var organizationalUnit = new BusinessOrganizationalUnit(moduleInfo.BusinessOwner);
            modelBuilder.Add(organizationalUnit);
            modelBuilder.Add(new BusinessOrganizationalUnit.OwnsDomainModule(organizationalUnit, module));
        }
    }
}