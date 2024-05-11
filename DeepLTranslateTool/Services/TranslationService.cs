using DeepLTranslateTool.Models;
using DeepLTranslateTool.Output;

namespace DeepLTranslateTool.Services;

public class TranslationService
{
    private readonly DeepL.Translator _translator;
    private readonly string _sourceLanguage;
    private readonly TranslationCache<string>? _cache = null;
    private readonly IOutput? _output = null;

    private const int CacheExpirationDays = 30;
    private const string CacheFilePath = "translation_cache.json";

    public TranslationService(DeepL.Translator translator, string sourceLanguage, IOutput? output = null, bool cache = true)
    {
        _translator = translator;
        _sourceLanguage = sourceLanguage;
        if (cache)
        {
            _cache = new TranslationCache<string>(CacheFilePath, TimeSpan.FromDays(CacheExpirationDays));
        }
        _output = output;
    }

    public List<TranslationResult> TranslateQueries(List<TranslationQuery> queries, IEnumerable<string> languages)
    {
        TranslationResult[] results = new TranslationResult[queries.Count * languages.Count()];
        int resultsFromCache = 0;

        Parallel.For(
            0,
            results.Length,
            i =>
            {
                var query = queries.ElementAt(i % queries.Count);
                var targetLanguage = languages.ElementAt(i / queries.Count);
                string key = $"{targetLanguage}_{query.Text}";

                if (_cache != null && _cache.TryGet(key, out string? cachedResult) && cachedResult != null)
                {
                    results[i] = new TranslationResult(query.Text,cachedResult, targetLanguage);
                    Interlocked.Increment(ref resultsFromCache);
                }
                else
                {
                    string translation = _translator.TranslateTextAsync(query.Text, _sourceLanguage, targetLanguage).Result.ToString();
                    results[i] = new TranslationResult(query.Text, translation, targetLanguage);
                    _cache?.Set(key, translation);
                }
            }
        );

        _output?.WriteLine($"Translations retrieved from cache: {resultsFromCache}/{results.Length}.", true);
        _cache?.WriteCache();
        return results.ToList();
    }
}
