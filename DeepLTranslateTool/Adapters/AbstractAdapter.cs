using DeepLTranslateTool.Exceptions;
using DeepLTranslateTool.Models;

namespace DeepLTranslateTool.Adapters;

public abstract class AbstractAdapter
{
    public string Path { get; set; } = new TranslateOptions().Path;
    public string SourceLanguage { get; set; } = new TranslateOptions().SourceLanguage;

    public void WriteOutput(IEnumerable<TranslationResult> results)
    {
        if (!results.Any())
        {
            throw new AdapterException("No translation results to write.");
        }
    }
}
