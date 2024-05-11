namespace DeepLTranslateTool.Exceptions;

[Serializable]
public class TranslatorInitializationException : Exception
{
    public TranslatorInitializationException() { }

    public TranslatorInitializationException(string message)
        : base(message) { }

    public TranslatorInitializationException(string message, Exception inner)
        : base(message, inner) { }
}
