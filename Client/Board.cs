using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    [Subcommand(typeof(PingCommand))]
    [Subcommand(typeof(Sample1Command))]
    [Subcommand(typeof(Sample2Command))]
    public class Board
    {
        public Board()
        {
        }

        protected int OnExecute(CommandLineApplication app)
        {
            // this shows help even if the --help option isn't specified
            app.ShowHelp();
            Console.WriteLine("Show Board");
            return 1;
        }

    }
}
