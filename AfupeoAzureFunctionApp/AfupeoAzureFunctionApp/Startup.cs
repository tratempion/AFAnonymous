using DAL;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: WebJobsStartup(typeof(AfupeoAzureFunctionApp.StartUp))]
namespace AfupeoAzureFunctionApp
{
    // Implement IWebJobStartup interface.
    public class StartUp : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            string connexionString = config.GetConnectionString("Production");
            builder.Services.AddSingleton<ISqlManager, SqlManager>(s => new SqlManager(connexionString));

        }
    }
}