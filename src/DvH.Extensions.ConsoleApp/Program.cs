using DvH.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace DvH.Extensions.ConsoleApp
{
    class Program
    {
        static IConfiguration Configuration;

        static void Main(string[] args)
        {
            CreateDefaultBuilder().Build().Run();
        }

        static IHostBuilder CreateDefaultBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, configuration) =>
                {
                    configuration.Sources.Clear();
                    configuration.AddUserSecrets<Program>(context.HostingEnvironment);
                    
                    Configuration = configuration.Build();
                })
                .ConfigureServices((context, services) =>
                {
                    //...
                });
        }
    }
}
