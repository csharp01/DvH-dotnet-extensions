using DvH.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace DvH.Extensions.ConsoleApp
{
    class Program
    {
        static IHostEnvironment HostEnvironment;
        static IConfiguration Configuration;
    
        static void Main(string[] args)
        {
            IHost host = null;

            try
            {
                host = CreateDefaultBuilder()
                    .Build();

                Console.WriteLine($"Environment name: {HostEnvironment.EnvironmentName}");

                host.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                if (host is not null)
                    host.Dispose();
            }
        }

        static IHostBuilder CreateDefaultBuilder()
        {
            return Host.CreateDefaultBuilder()
                .UseConsoleLifetime()
                .ConfigureAppConfiguration((context, configuration) =>
                {
                    HostEnvironment = context.HostingEnvironment;
                    configuration.Sources.Clear();
                    configuration.AddUserSecrets<Program>(context.HostingEnvironment, fallbackToDefaultSecretsFileName: false);
                    
                    Configuration = configuration.Build();
                })
                .ConfigureServices((context, services) =>
                {
                    //...
                });
        }
    }
}
