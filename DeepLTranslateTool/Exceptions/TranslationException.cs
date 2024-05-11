namespace DeepLTranslateTool.Exceptions;

[Serializable]
public class TranslationException : Exception
{
    public TranslationException() { }

    public TranslationException(string message)
        : base(message) { }

    public TranslationException(string message, Exception inner)
        : base(message, inner) { }
}
