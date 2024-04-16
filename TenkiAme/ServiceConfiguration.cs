using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using TenkiAme.Models;

namespace TenkiAme
{
    public static class ServiceConfiguration
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            var vaultName = "TenkiAme-Secrets";
            var keyVaultUrl = $"https://{vaultName}.vault.azure.net";
            var client = new SecretClient(vaultUri: new Uri(keyVaultUrl), credential: new DefaultAzureCredential());

            var secretsDict = new string[] { "MetOcean-Key" }
                .Select(async name => await client.GetSecretAsync(name))
                .Select(task => task.Result.Value)
                .ToDictionary(secret => secret.Name, secret => secret.Value);

            foreach (var name in new string[] { "MetOcean-Key" })
            {
                Console.Out.WriteLine($"{name}: {secretsDict[name]}");
            }

            services.AddSingleton<IDictionary<string, string>>(provider => secretsDict);
            //services.AddSingleton<WeatherAPIService>();
        }
    }
}