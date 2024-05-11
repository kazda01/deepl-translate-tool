using System.Text.Json;

namespace DeepLTranslateTool.Services;

public class TranslationCache<T>
{
    private readonly string _cacheFilePath;
    private readonly TimeSpan _expirationTime;
    private readonly Dictionary<string, CachedItem<T>> _cache;

    public TranslationCache(string cacheFilePath, TimeSpan expirationTime)
    {
        _cacheFilePath = cacheFilePath;
        _expirationTime = expirationTime;

        if (!File.Exists(_cacheFilePath))
        {
            File.WriteAllText(_cacheFilePath, "{}");
        }
        _cache = ReadCache();
    }

    public void Set(string key, T value)
    {
        _cache[key] = new CachedItem<T>(value, DateTime.Now);
    }

    public bool TryGet(string key, out T? value)
    {
        if (_cache.TryGetValue(key, out var cachedItem))
        {
            if (DateTime.Now - cachedItem.CreationTime <= _expirationTime)
            {
                value = cachedItem.Value;
                return true;
            }
            _cache.Remove(key);
        }

        value = default;
        return false;
    }

    private Dictionary<string, CachedItem<T>> ReadCache()
    {
        string json = File.ReadAllText(_cacheFilePath);
        var cache = JsonSerializer.Deserialize<Dictionary<string, CachedItem<T>>>(json) ?? throw new FileLoadException("Error reading cache file.");
        return cache;
    }

    public void WriteCache()
    {
        string json = JsonSerializer.Serialize(_cache, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_cacheFilePath, json);
    }

    private record CachedItem<U>(U Value, DateTime CreationTime);
}
