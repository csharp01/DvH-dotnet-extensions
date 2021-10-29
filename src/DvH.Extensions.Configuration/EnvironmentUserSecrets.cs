using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection;

namespace DvH.Extensions.Configuration
{
    public static class EnvironmentUserSecrets
    {
        public static IConfigurationBuilder AddUserSecrets<T>(this IConfigurationBuilder configuration, IHostEnvironment hostingEnvironment, bool reloadOnChange = false, bool fallbackToDefaultSecretsFileName = true)
            where T : class
        {
            Assembly assembly = typeof(T).GetTypeInfo().Assembly;

            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (hostingEnvironment == null)
            {
                throw new ArgumentNullException(nameof(hostingEnvironment));
            }

            UserSecretsIdAttribute attribute = assembly.GetCustomAttribute<UserSecretsIdAttribute>();
            if (attribute == null)
            {
                throw new InvalidOperationException($"Could not find 'UserSecretsIdAttribute' on assembly '{assembly.GetName().Name}'");
            }

            string userSecretsId = attribute.UserSecretsId;
            string secretPath = PathHelper.GetSecretsPathFromSecretsId(userSecretsId);
            string directoryPath = Path.GetDirectoryName(secretPath);
            string secretsFileName = $"secrets.{hostingEnvironment.EnvironmentName}.json";

            if (!File.Exists(Path.Combine(directoryPath, secretsFileName)))
            {
                if (!fallbackToDefaultSecretsFileName)
                {
                    throw new FileNotFoundException($"Could not find the environment user secrets file.", secretsFileName);
                }
                secretsFileName = "secrets.json";
            }

            PhysicalFileProvider fileProvider = Directory.Exists(directoryPath)
                ? new PhysicalFileProvider(directoryPath)
                : null;

            return configuration.AddJsonFile(fileProvider, secretsFileName, optional: true, reloadOnChange);
        }
    }
}
