using Dotmim.Sync;
using Dotmim.Sync.Sqlite;
using Dotmim.Sync.Web.Client;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
 
    [Command("sample2", Description = "This command will execute a sync on an existing sqlite database")]
    public class Sample2Command
    {
        public Sample2Command(IConfiguration configuration, IOptions<ApiOptions> apiOptions)
        {
            ApiOptions = apiOptions.Value;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public ApiOptions ApiOptions { get; }

        public async Task<int> OnExecuteAsync(CommandLineApplication app)
        {

            var sqliteDbName = HelperDatabase.GetRandomName("sample2");
            var sqliteConnectionString = HelperDatabase.GetSqliteDatabaseConnectionString(sqliteDbName);

            // Create a sqlite database

            var script = @"CREATE TABLE [Vehicle] (
	                        [Id] text  NOT NULL COLLATE NOCASE
	                        ,[VehicleType] integer  NOT NULL 
	                        ,[Make] text  NULL COLLATE NOCASE
	                        ,[Year] text  NULL COLLATE NOCASE
	                        ,PRIMARY KEY ([Id]))";

            await HelperDatabase.ExecuteSqliteScriptAsync(sqliteDbName, script);

            // Add some datas BEFORE sync is initiated
            var insertVehicles = @"Insert into Vehicle (Id, VehicleType, Make, Year) 
                                   Values ('" + Guid.NewGuid().ToString() + "', 1, 'Make From Client', '2000')";

            await HelperDatabase.ExecuteSqliteScriptAsync(sqliteDbName, insertVehicles);

            // Now we have an existing database we want to sync

            var serverOrchestrator = new WebClientOrchestrator(ApiOptions.SyncAddress);
            var clientProvider = new SqliteSyncProvider(sqliteConnectionString);

            // Creating an agent that will handle all the process
            var agent = new SyncAgent(clientProvider, serverOrchestrator);

            try
            {
                var progress = new SynchronousProgress<ProgressArgs>(args => Console.WriteLine($"{args.ProgressPercentage:p}:\t{args.Message}"));
                // Launch the sync process 
                // On the first sync, the rows from server are downloaded locally
                // but local rows are not uploaded since they are not tracked
                var s1 = await agent.SynchronizeAsync(progress);
                // Write results
                Console.WriteLine(s1);

                // Now we are marking clients rows not tracked yet, for next sync
                await agent.LocalOrchestrator.UpdateUntrackedRowsAsync();

                // now we are making a second sync to upload these rows to server
                s1 = await agent.SynchronizeAsync(progress);
                // Write results
                Console.WriteLine(s1);

                HelperDatabase.DropSqliteDatabase(sqliteDbName);
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
