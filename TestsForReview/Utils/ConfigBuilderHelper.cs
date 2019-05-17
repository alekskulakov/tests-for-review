using Microsoft.Extensions.Configuration;

namespace TestsForReview.Utils
{
    public static class ConfigBuilderHelper
    {
        public static IConfigurationBuilder SetUpBuilder(string path, bool isLocal, string profile, IConfigurationBuilder builder = null)
        {
            var b = builder ?? new ConfigurationBuilder();
            b.SetBasePath(path);
            b.AddJsonFile("appsettings.json", true, true);
            if (isLocal)
            {
                b.AddJsonFile("appsettings.local.json");
            }
            b.AddJsonFile($"appsettings.{profile}.json", true, true)
                .AddEnvironmentVariables();
            return b;
        }
    }
}
