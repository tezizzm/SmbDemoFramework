using Microsoft.Extensions.Configuration;

using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace LighthouseWeb
{
    public static class ApplicationConfig
    {
        public static IConfiguration Configuration { get; set; }

        public static void RegisterConfig()
        {

            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddCloudFoundry()
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }
    }
}