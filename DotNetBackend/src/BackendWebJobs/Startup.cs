using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using System;
using BackendRedis.Crosscutting.Configuration;
using BackendRedis.Crosscutting.Caching;

namespace BackendWebJobs
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
            config.StorageConnectionString = "[STRING DE CONEXÃO PARA O STORAGE NO AZURE]";

            JobHost host = new JobHost(config);
            host.RunAndBlock();
        }
    }
}
