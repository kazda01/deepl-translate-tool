namespace DeepLTranslateTool.Output;

public class ConsoleOutput : IOutput
{
    public bool Verbose { get; set; } = false;

    public void WriteLine(string message, bool verbose = false)
    {
        if (verbose && !Verbose)
            return;

        Console.WriteLine(verbose ? $"[VERBOSE] {message}" : message);
    }
}
