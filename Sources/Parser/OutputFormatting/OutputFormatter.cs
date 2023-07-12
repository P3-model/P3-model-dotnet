using System.Threading.Tasks;
using P3Model.Parser.ModelSyntax;

namespace P3Model.Parser.OutputFormatting;

public interface OutputFormatter
{
    Task Clean();
    Task Write(Model model);
}