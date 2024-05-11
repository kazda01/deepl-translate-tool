namespace DeepLTranslateTool.Models;

/// <summary>
/// Represents the result of a translation.
/// </summary>
/// <param name="Query">The original text that was translated.</param>
/// <param name="Translation">The translated text.</param>
/// <param name="Language">The language of the translated text.</param>
public record TranslationResult(string Query, string Translation, string Language);
