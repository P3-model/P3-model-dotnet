using JetBrains.Annotations;

namespace P3Model.Parser.OutputFormatting.Configuration;

public class OutputFormatBuilder
{
    private readonly List<OutputFormatter> _formatters = new();
    
    [PublicAPI]
    public OutputFormatBuilder Use(params OutputFormatter[] formatters)
    {
        _formatters.AddRange(formatters);
        return this;
    }

    public IReadOnlyCollection<OutputFormatter> Build() => _formatters.AsReadOnly();
}