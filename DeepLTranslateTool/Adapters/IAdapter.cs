using DeepLTranslateTool.Models;
using DeepLTranslateTool.Exceptions;

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
    /// <returns>A collection of TranslationQuery objects.</returns>
    /// <exception cref="AdapterException">Thrown when an error occurs during parsing.</exception>
    List<TranslationQuery> ParseInput();

    /// <summary>
    /// Writes the output of the translation results.
    /// </summary>
    /// <param name="results">The collection of translation results.</param>
    /// <exception cref="AdapterException">Thrown when an error occurs during writing.</exception>
    void WriteOutput(IEnumerable<TranslationResult> results);
}
