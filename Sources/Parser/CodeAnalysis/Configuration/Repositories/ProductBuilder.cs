namespace P3Model.Parser.CodeAnalysis.Configuration.Repositories;

public class ProductBuilder
{
    private string? _name;

    public ProductBuilder UseName(string name)
    {
        _name = name;
        return this;
    }

    public string Build() => _name ?? throw new InvalidOperationException();
}