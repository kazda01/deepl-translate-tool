using DeepLTranslateTool.Models;

namespace DeepLTranslateTool.Adapters;

public class PlaintextAdapter : IAdapter
{
    public string Handle => "plaintext";
    public string Path { get; set; } = Environment.CurrentDirectory;
    public string InputFile { get; set; } = "input.txt";
    public string SourceLanguage { get; set; } = "en";

    public IEnumerable<TranslationQuery> ParseInput(out bool success)
    {
        success = false;

        if (!File.Exists(System.IO.Path.Combine(Path, InputFile)))
        {
            Console.Error.WriteLine($"Input file '{InputFile}' not found in path '{Path}'.");
            return Enumerable.Empty<TranslationQuery>();
        }

        try
        {
            var lines = File.ReadAllLines(System.IO.Path.Combine(Path, InputFile));
            var queries = new HashSet<TranslationQuery>();

            foreach (var line in lines)
            {
                queries.Add(new TranslationQuery(line, SourceLanguage));
            }

            success = true;
            return queries;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error reading input file: {ex.Message}");
            return Enumerable.Empty<TranslationQuery>();
        }
    }

    public bool WriteOutput(IEnumerable<TranslationResult> results)
    {
        if (!results.Any())
        {
            Console.Error.WriteLine("No translation results to write.");
            return false;
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
                Console.Error.WriteLine($"Error writing output file '{outputFile}': {ex.Message}");
                return false;
            }
        }

        return true;
    }
}
