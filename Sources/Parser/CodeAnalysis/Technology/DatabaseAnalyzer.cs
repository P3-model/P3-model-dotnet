using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using P3Model.Annotations.Technology;
using P3Model.Parser.CodeAnalysis.RoslynExtensions;
using P3Model.Parser.ModelSyntax;
using P3Model.Parser.ModelSyntax.Technology;

namespace P3Model.Parser.CodeAnalysis.Technology;

[UsedImplicitly]
public class DatabaseAnalyzer : SymbolAnalyzer<INamedTypeSymbol>
{
    public void Analyze(INamedTypeSymbol symbol, ModelBuilder modelBuilder)
    {
        if (!symbol.TryGetAttribute(typeof(DatabaseAttribute), out var databaseAttribute))
            return;
        var name = databaseAttribute.GetConstructorArgumentValue<string>(nameof(DatabaseAttribute.Name));
        var database = new Database(name);
        modelBuilder.Add(database, symbol);
        if (!databaseAttribute.TryGetNamedArgumentValue<string>(nameof(DatabaseAttribute.ClusterName),
                out var clusterName)) 
            return;
        var cluster = new DatabaseCluster(clusterName);
        modelBuilder.Add(cluster);
        modelBuilder.Add(new Database.BelongsToCluster(database, cluster));
    }
}