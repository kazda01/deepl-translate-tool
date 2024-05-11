using DeepLTranslateTool.Adapters;

namespace DeepLTranslateTool;

public class Translator
{
    private IAdapter? _adapter;
    private readonly bool _verbose;
    private readonly string _sourceLanguage;
    private readonly DeepL.Translator _translator;
    private IEnumerable<string> _languages = Enumerable.Empty<string>();

    public Translator(TranslateOptions options)
    {
        _verbose = options.Verbose;
        _adapter = ParseAdapter(options.Adapter);
        _translator = CreateTranslator(options.ApiKey);
        _sourceLanguage = ParseSourceLanguage(options.SourceLanguage).Result;
        _languages = ParseLanguages(options.Languages).Result;
    }

    private async Task<IEnumerable<string>> ParseLanguages(IEnumerable<string> languages)
    {
        var supportedLanguages = await _translator.GetTargetLanguagesAsync();
        var languageCodes = new HashSet<string>();

        foreach (var language in languages)
        {
            var languageCode = language.ToLower();
            foreach (var supportedLanguage in supportedLanguages)
            {
                if (supportedLanguage.Code == languageCode)
                {
                    languageCodes.Add(languageCode);
                }
            }
        }

        if (!languageCodes.Any())
            throw new Exception("No valid target languages found. To see a list of supported languages, use the `list-languages` command.");

        if (_verbose)
            Console.WriteLine($"Using target languages: {string.Join(", ", languageCodes)}");

        return languageCodes;
    }

    private async Task<string> ParseSourceLanguage(string sourceLanguage)
    {
        sourceLanguage = sourceLanguage.ToLower();
        var languages = await _translator.GetSourceLanguagesAsync();
        foreach (var language in languages)
        {
            if (language.Code == sourceLanguage)
            {
                if (_verbose)
                    Console.WriteLine($"Using source language '{language.Name}' ({language.Code}).");
                return sourceLanguage;
            }
        }
        throw new Exception($"Source language '{sourceLanguage}' not found. To see a list of supported languages, use the `list-source-languages` command.");
    }

    private DeepL.Translator CreateTranslator(string ApiKey)
    {
        if (_verbose)
            Console.WriteLine("Creating translator...");

        var translator = new DeepL.Translator(ApiKey);
        if (_verbose)
            Console.WriteLine("Translator created.");

        return translator;
    }

    private IAdapter ParseAdapter(string adapter)
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
                    if (_verbose)
                        Console.WriteLine($"Using adapter '{adapter}'.");
                    return instance;
                }
            }
        }
        throw new Exception($"Adapter '{adapter}' not found.");
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
        }
    }

    public static void ListSourceLanguages(string ApiKey)
    {
        try
        {
            var translator = new DeepL.Translator(ApiKey);
            var languages = translator.GetSourceLanguagesAsync().Result;
            Console.WriteLine("Supported target languages:");
            foreach (var language in languages)
            {
                Console.WriteLine($" - {language.Name} ({language.Code})");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error listing languages: {ex.Message}");
        }
    }
}
