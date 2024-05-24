using DeepLTranslateTool.Adapters;
using DeepLTranslateTool.Exceptions;
using DeepLTranslateTool.Output;
using DeepLTranslateTool.Services;

namespace DeepLTranslateTool;

public class Translator
{
    private readonly IAdapter _adapter;
    private readonly IOutput _output;
    private readonly string _sourceLanguage;
    private readonly DeepL.Translator _translator;
    private readonly IEnumerable<string> _languages = Enumerable.Empty<string>();
    private readonly bool _cache = true;

    public Translator(TranslateOptions options)
    {
        _output = new ConsoleOutput { Verbose = options.Verbose };
        _adapter = ParseAdapter(options.Adapter, options);
        _translator = CreateTranslator(options.ApiKey);
        _sourceLanguage = ParseSourceLanguage(options.SourceLanguage).Result;
        _languages = ParseLanguages(options.Languages).Result;
        _cache = !options.NoCache;
    }

    public void Translate()
    {
        var queries = _adapter.ParseInput();
        _output.WriteLine($"Parsed {queries.Count()} queries from input file.", true);

        var translationService = new TranslationService(_translator, _sourceLanguage, _output, _cache);
        var results = translationService.TranslateQueries(queries, _languages);
        _output.WriteLine($"Translated {results.Count} queries to {_languages.Count()} languages.");

        _adapter.WriteOutput(results);
        _output.WriteLine("Translation complete.", true);
    }

    private async Task<IEnumerable<string>> ParseLanguages(IEnumerable<string> languages)
    {
        var supportedLanguages = await _translator.GetTargetLanguagesAsync();
        var languageCodes = new HashSet<string>();

        foreach (var language in languages)
        {
            if (supportedLanguages.Any(l => l.Code.ToLower() == language.ToLower()))
            {
                languageCodes.Add(language);
                continue;
            }
            throw new TranslatorInitializationException($"Target language '{language}' not found. To see a list of supported languages, use the `list-languages` command.");
        }

        if (!languageCodes.Any())
            throw new TranslatorInitializationException("No valid target languages found. To see a list of supported languages, use the `list-languages` command.");

        _output.WriteLine($"Using target languages: {string.Join(", ", languageCodes)}.", true);

        return languageCodes;
    }

    private async Task<string> ParseSourceLanguage(string sourceLanguage)
    {
        sourceLanguage = sourceLanguage.ToLower();
        var languages = await _translator.GetSourceLanguagesAsync();
        foreach (var language in languages)
        {
            if (language.Code.ToLower() == sourceLanguage)
            {
                _output.WriteLine($"Using source language {language.Code}.", true);
                return language.Code;
            }
        }
        throw new TranslatorInitializationException($"Source language '{sourceLanguage}' not found. To see a list of supported languages, use the `list-source-languages` command.");
    }

    private DeepL.Translator CreateTranslator(string ApiKey)
    {
        _output.WriteLine("Creating translator...", true);

        var translator = new DeepL.Translator(ApiKey);
        _output.WriteLine("Translator created.", true);

        return translator;
    }

    private IAdapter ParseAdapter(string adapter, TranslateOptions options)
    {
        adapter = adapter.ToLower();
        var adapterType = typeof(IAdapter);
        var adapterTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => adapterType.IsAssignableFrom(p) && !p.IsInterface);

        foreach (var type in adapterTypes)
        {
            if (Activator.CreateInstance(type) is IAdapter instance)
            {
                if (instance.Handle == adapter)
                {
                    _output.WriteLine($"Using adapter '{adapter}'.", true);
                    if (options.InputFile != null)
                        instance.InputFile = options.InputFile;
                    instance.Path = options.Path;
                    return instance;
                }
            }
        }
        throw new TranslatorInitializationException($"Adapter '{adapter}' not found.");
    }

    public static void ListLanguages(string ApiKey)
    {
        try
        {
            var translator = new DeepL.Translator(ApiKey);
            var languages = translator.GetTargetLanguagesAsync().Result;
            Console.WriteLine("Supported target languages:");
            foreach (var language in languages)
            {
                Console.WriteLine($" - {language.Name} ({language.Code})");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error listing languages: {ex.Message}");
            Environment.Exit(1);
        }
    }

    public static void ListSourceLanguages(string ApiKey)
    {
        try
        {
            var translator = new DeepL.Translator(ApiKey);
            var languages = translator.GetSourceLanguagesAsync().Result;
            Console.WriteLine("Supported source languages:");
            foreach (var language in languages)
            {
                Console.WriteLine($" - {language.Name} ({language.Code})");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error listing languages: {ex.Message}");
            Environment.Exit(1);
        }
    }
}
