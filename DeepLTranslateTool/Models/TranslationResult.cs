namespace DeepLTranslateTool.Models;

/// <summary>
/// Represents the result of a translation.
/// </summary>
/// <param name="Text">The translated text.</param>
/// <param name="Language">The language of the translated text.</param>
public record TranslationResult(string Text, string Language);
