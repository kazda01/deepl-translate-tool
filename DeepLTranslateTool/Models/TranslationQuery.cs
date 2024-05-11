namespace DeepLTranslateTool.Models;

/// <summary>
/// Represents a translation query with the text to be translated, the source language, and the target language.
/// </summary>
/// <param name="Text">The text to be translated.</param>
/// <param name="SourceLanguage">The source language of the text.</param>
public record TranslationQuery(string Text, string SourceLanguage);