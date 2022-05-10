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
 
    [Command("ping", Description = "Test if server is reliable and get some vehicles")]
    public class PingCommand
    {
        public PingCommand(IConfiguration configuration, IOptions<ApiOptions> apiOptions)
        {
            ApiOptions = apiOptions.Value;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public ApiOptions ApiOptions { get; }

        public async Task<int> OnExecuteAsync(CommandLineApplication app)
        {
            var httpClient = new HttpClient();

            try
            {
                var response = await httpClient.GetAsync($"{ApiOptions.DatabaseAddress}/Vehicles");

                response.EnsureSuccessStatusCode();

                var vehiclesString = await response.Content.ReadAsStringAsync();

                Console.WriteLine(vehiclesString);

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
