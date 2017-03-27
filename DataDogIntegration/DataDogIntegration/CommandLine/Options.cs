using CommandLine;
using CommandLine.Text;

namespace DataDogIntegration.CommandLine
{
    public class CommandArgumentOptions
    {
        
        // Omitting long name, default --verbose
        [Option("v", HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        //--d
        [Option('d', "dashboard", Required = false, HelpText = "Dashboard Id to process on.")]
        public string DashboardId { get; set; }

        [Option('p', "PingdomId", Required = false, HelpText = "Pingdom check id.")]
        public string PingdomId { get; set; }


        [Option('n', "PingdomName", Required = false, HelpText = "Pingdom Display Name.")]
        public string PingdomName { get; set; }

        [Option('t', "title", Required = false, HelpText = "Widget title name.")]
        public string WidgetName { get; set; }


        //[Option('o', "OutputFile", Required = false, HelpText = "output file")]
        //public string OutFilePath { get; set; }

        [Option('i', "InputFile", Required = false, HelpText = "Pipe Deliminated file to process on")]
        public string InputFilePath { get; set; }
        


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [HelpOption(HelpText = "Display this help screen.")]
        public string GetUsage()
        {
            
            var help = new HelpText
            {
                Heading = new HeadingInfo(
                    $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name} Version: {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}"),
                Copyright = new CopyrightInfo(" ", System.DateTime.Now.Year),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            help.AddPreOptionsLine(
                $"Usage: {System.Reflection.Assembly.GetExecutingAssembly().GetName().Name} site action");
            help.AddOptions(this);
            return help;
          
        }
    }
}
