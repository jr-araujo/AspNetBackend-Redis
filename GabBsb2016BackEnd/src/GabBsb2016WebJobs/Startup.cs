using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using GabBsb2016.Crosscutting.Caching;
using GabBsb2016.Crosscutting.Configuration;
using Microsoft.Azure.WebJobs;
using System;

namespace GabBsb2016WebJobs
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }
        public static IServiceCollection Services { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            builder.AddEnvironmentVariables();
            Configuration = builder.Build().ReloadOnChanged("appsettings.json");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<Config>(Configuration.GetSection("Config"));
            services.AddSingleton<RedisConnectionManager>();
            services.AddTransient<ProcessQueue>();

            Services = services;
        }

        public void Configure(IApplicationBuilder app)
        {
        }

        public static void Main(string[] args)
        {
            try
            {
                WebApplication.Run<Startup>(args);
            }
            catch { }

            IServiceProvider provider = Services. BuildServiceProvider();
            PresentationsQueueProcessor.ProcessQueue = provider.GetService<ProcessQueue>();

            JobHostConfiguration config = new JobHostConfiguration();
            config.DashboardConnectionString = "";
            config.StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=gabbsb2016storage;AccountKey=lHGPhjE8tJzF3+V48XOB0sSbMQ+x/0ZlWNNLUkRER7kXR8YyUpmnfH6EWBxFWkLf4C48a9ds5S8Hd6DY1JxkKA==";

            JobHost host = new JobHost(config);
            host.RunAndBlock();
        }
    }
}
