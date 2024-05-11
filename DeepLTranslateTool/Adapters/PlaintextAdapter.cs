using DeepLTranslateTool.Exceptions;
using DeepLTranslateTool.Models;

namespace DeepLTranslateTool.Adapters;

public class PlaintextAdapter : IAdapter
{
    public string Handle => "plaintext";
    public string Path { get; set; } = Environment.CurrentDirectory;
    public string InputFile { get; set; } = "input.txt";
    public string SourceLanguage { get; set; } = "en";

    public List<TranslationQuery> ParseInput()
    {
        if (!File.Exists(System.IO.Path.Combine(Path, InputFile)))
        {
            throw new AdapterException($"Input file '{InputFile}' not found in path '{Path}'.");
        }

        try
        {
            var lines = File.ReadAllLines(System.IO.Path.Combine(Path, InputFile));
            var queries = new List<TranslationQuery>();

            foreach (var line in lines)
            {
                queries.Add(new TranslationQuery(line, SourceLanguage));
            }

            return queries;
        }
        catch (Exception ex)
        {
            throw new AdapterException($"Error reading input file '{InputFile}'.", ex);
        }
    }

    public void WriteOutput(IEnumerable<TranslationResult> results)
    {
        if (!results.Any())
        {
            throw new AdapterException("No translation results to write.");
        }

        foreach (var group in results.GroupBy(r => r.Language))
        {
            var outputFile = System.IO.Path.Combine(Path, $"{group.Key}.txt");

            try
            {
                File.WriteAllLines(outputFile, group.Select(r => r.Text));
            }
            catch (Exception ex)
            {
                throw new AdapterException($"Error writing output file '{outputFile}'.", ex);
            }
        }
    }
}
