using System.Text.RegularExpressions;
using DeepLTranslateTool.Exceptions;
using DeepLTranslateTool.Models;

namespace DeepLTranslateTool.Adapters;

public class Yii2Adapter : AbstractAdapter, IAdapter
{
    public string Handle => "php-yii2";
    public string InputFile { get; set; } = "app.php";

    public List<TranslationQuery> ParseInput()
    {
        if (!File.Exists(System.IO.Path.Combine(Path, InputFile)))
        {
            throw new AdapterException($"Input file '{InputFile}' not found in path '{Path}'.");
        }

        try
        {
            var fileContent = File.ReadAllText(System.IO.Path.Combine(Path, InputFile));

            MatchCollection matches = Regex.Matches(fileContent, @"\s*['""](?<key>[^'""]+)['""]\s*=>\s*['""](?<value>[^'""]+)['""](?:\s*,\s*|\s*)");
            var queries = new List<TranslationQuery>();

            foreach (Match match in matches)
            {
                string query = match.Groups["key"].Value;
                queries.Add(new TranslationQuery(query, SourceLanguage));
            }

            return queries;
        }
        catch (Exception ex)
        {
            throw new AdapterException($"Error reading input file '{InputFile}'.", ex);
        }
    }

    public new void WriteOutput(IEnumerable<TranslationResult> results)
    {
        base.WriteOutput(results);

        foreach (var group in results.GroupBy(r => r.Language))
        {
            if (!Directory.Exists(System.IO.Path.Combine(Path, $"{group.Key}")))
            {
                Directory.CreateDirectory(System.IO.Path.Combine(Path, $"{group.Key}"));
            }
            var outputFile = System.IO.Path.Combine(Path, $"{group.Key}", InputFile);

            try
            {
                using StreamWriter writer = new(outputFile);

                writer.WriteLine("<?php");
                writer.WriteLine("");
                writer.WriteLine("return [");

                foreach (var result in group)
                {
                    writer.WriteLine($"    '{result.Query.Replace("'", "\\'")}' => '{result.Translation.Replace("'", "\\'")}',");
                }

                writer.WriteLine("];");
            }
            catch (Exception ex)
            {
                throw new AdapterException($"Error writing output file '{outputFile}'.", ex);
            }
        }
    }
}
