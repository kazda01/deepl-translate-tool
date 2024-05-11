using CommandLine;
using CommandLine.Text;
using DeepLTranslateTool.Exceptions;

namespace DeepLTranslateTool;

[Verb("translate", HelpText = "Translate text.")]
public class TranslateOptions
{
    [Option("api-key", HelpText = "DeepL API authentication key.", Required = true)]
    public string ApiKey { get; set; } = string.Empty;

    [Option('a', "adapter", Default = "plaintext", HelpText = "Adapter to use for input and output files.")]
    public string Adapter { get; set; } = "plaintext";

    [Option('s', "source-language", HelpText = "Source language for translation.", Default = "en")]
    public string SourceLanguage { get; set; } = "en";

    [Option('l', "languages", HelpText = "Languages to translate to, separated by space.", Default = new[] { "en" })]
    public IEnumerable<string> Languages { get; set; } = new[] { "en" };

    [Option('i', "input-file", HelpText = "Input file to translate. If not specified, input file is defined by choosen adapter.")]
    public string? InputFile { get; set; }

    [Option("no-cache", HelpText = "Whether to disable caching of translation results. Caching is enabled by default and queries are cached for 1 month", Default = false)]
    public bool NoCache { get; set; }

    [Option('v', "verbose", Default = false, HelpText = "Print verbose output.")]
    public bool Verbose { get; set; }
}

[Verb("list-languages", HelpText = "List supported target languages.")]
public class ListLanguagesOptions
{
    [Option("api-key", HelpText = "DeepL API authentication key.", Required = true)]
    public string ApiKey { get; set; } = string.Empty;
}

[Verb("list-source-languages", HelpText = "List supported source languages.")]
public class ListSourceLanguagesOptions
{
    [Option("api-key", HelpText = "DeepL API authentication key.", Required = true)]
    public string ApiKey { get; set; } = string.Empty;
}

class Program
{
    static void Main(string[] args)
    {
        var parser = new Parser(with => with.HelpWriter = null);
        var parserResult = parser.ParseArguments<TranslateOptions, ListLanguagesOptions, ListSourceLanguagesOptions>(args);
        parserResult
            .WithParsed<TranslateOptions>(options => Run(options))
            .WithParsed<ListLanguagesOptions>(options => Translator.ListLanguages(options.ApiKey))
            .WithParsed<ListSourceLanguagesOptions>(options => Translator.ListSourceLanguages(options.ApiKey))
            .WithNotParsed(errs => DisplayHelp(parserResult, errs));
    }

    static void DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errs)
    {
        Console.WriteLine(
            HelpText.AutoBuild(
                result,
                helpText =>
                {
                    helpText.AdditionalNewLineAfterOption = false;
                    return helpText;
                }
            )
        );
        if (!errs.IsHelp())
            Environment.Exit(1);
    }

    private static void Run(TranslateOptions options)
    {
        Translator translator;
        try
        {
            translator = new Translator(options);
            translator.Translate();
        }
        catch (TranslationException ex)
        {
            Console.Error.WriteLine($"Error translating text:: {ex.Message}");
            Environment.Exit(1);
            return;
        }
        catch (TranslatorInitializationException ex)
        {
            Console.Error.WriteLine($"Error initializing translator: {ex.Message}");
            Environment.Exit(1);
            return;
        }
        catch (AdapterException ex)
        {
            Console.Error.WriteLine($"Error in adapter: {ex.Message}");
            Environment.Exit(1);
            return;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unknown Error: {ex.Message}");
            Environment.Exit(1);
            return;
        }
    }
}
