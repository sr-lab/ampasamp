using CommandLine;
using CommandLine.Text;

namespace Ampasamp
{
    /// <summary>
    /// Represents the command-line options for the program.
    /// </summary>
    internal class SampleOptions
    {
        [Option('d', "database", Required = true,
            HelpText = "The full password database to use.")]
        public string Database { get; set; }

        [Option('t', "task", Required = true,
            HelpText = "The task file to execute.")]
        public string Task { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
