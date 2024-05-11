namespace DeepLTranslateTool.Output;

public interface IOutput
{
    /// <summary>
    /// Gets or sets a value indicating whether the output should be verbose.
    /// </summary>
    public bool Verbose { get; set; }

    /// <summary>
    /// Writes a line of text to the output.
    /// </summary>
    /// <param name="message">The text to write.</param>
    /// <param name="verbose">Indicates whether the message should be written only in verbose mode.</param>
    void WriteLine(string message, bool verbose = false);
}
