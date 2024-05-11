namespace DeepLTranslateTool.Exceptions;

[Serializable]
public class AdapterException : Exception
{
    public AdapterException() { }

    public AdapterException(string message)
        : base(message) { }

    public AdapterException(string message, Exception inner)
        : base(message, inner) { }
}
