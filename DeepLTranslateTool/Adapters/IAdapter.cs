using DeepLTranslateTool.Models;

namespace DeepLTranslateTool.Adapters;

public interface IAdapter
{
    /// <summary>
    /// The handle of the adapter which is used to identify it in the command line arguments.
    /// </summary>
    public string Handle { get; }

    /// <summary>
    /// Path of working directory, where input and output files are located. Adapter should fallback to default value if not set.
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Path of the input file to be translated. Adapter should fallback to default value if not set.
    /// </summary>
    public string InputFile { get; set; }

    /// <summary>
    /// Source language for translation queries. Adapter should fallback to default value if not set.
    /// </summary>
    public string SourceLanguage { get; set; }

    /// <summary>
    /// Parses the input and returns a collection of TranslationQuery objects.
    /// </summary>
    /// <param name="success">An out parameter indicating whether the parsing was successful.</param>
    /// <returns>A collection of TranslationQuery objects.</returns>
    IEnumerable<TranslationQuery> ParseInput(out bool success);

    /// <summary>
    /// Writes the output of the translation results.
    /// </summary>
    /// <param name="results">The collection of translation results.</param>
    /// <returns>A boolean value indicating whether the write operation was successful.</returns>
    bool WriteOutput(IEnumerable<TranslationResult> results);
}
