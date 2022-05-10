using Dotmim.Sync;
using Dotmim.Sync.Sqlite;
using Dotmim.Sync.Web.Client;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
 
    [Command("sample1", Description = "This command will execute a simple sync to the server. local sqlite database is created if not exists.")]
    public class Sample1Command
    {
        public Sample1Command(IConfiguration configuration, IOptions<ApiOptions> apiOptions)
        {
            ApiOptions = apiOptions.Value;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public ApiOptions ApiOptions { get; }

        public async Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            var serverOrchestrator = new WebClientOrchestrator(ApiOptions.SyncAddress);

            // Second provider is using plain old Sql Server provider, relying on triggers and tracking tables to create the sync environment
            var clientProvider = new SqliteSyncProvider("sample01.db");

            // Creating an agent that will handle all the process
            var agent = new SyncAgent(clientProvider, serverOrchestrator);

            try
            {
                var progress = new SynchronousProgress<ProgressArgs>(args => Console.WriteLine($"{args.ProgressPercentage:p}:\t{args.Message}"));
                // Launch the sync process
                var s1 = await agent.SynchronizeAsync(progress);
                // Write results
                Console.WriteLine(s1);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
            return 1;
        }
    }
}
