using CommandLine;
using CommandLine.Text;

namespace DeepLTranslateTool;

class Program
{
    public class Options
    {
        [Option('a', "adapter", Default = "plaintext", HelpText = "Adapter to use for input and output files.")]
        public string? InputFile { get; set; }
    }

    static void Main(string[] args)
    {
        var parser = new Parser(with => with.HelpWriter = null);
        var parserResult = parser.ParseArguments<Options>(args);
        parserResult.WithParsed(options => Run(options)).WithNotParsed(errs => DisplayHelp(parserResult));
    }

    static void DisplayHelp<T>(ParserResult<T> result) =>
        Console.WriteLine(
            HelpText.AutoBuild(
                result,
                helpText =>
                {
                    helpText.AdditionalNewLineAfterOption = false;
                    return helpText;
                }
            )
        );

    private static void Run(Options options)
    {
        //do stuff
    }
}
