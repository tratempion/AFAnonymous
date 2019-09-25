using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using DAL;
using System.Net;
using System.Collections.Generic;
using DTO;
using Microsoft.Extensions.DependencyInjection;

namespace AfupeoAzureFunctionApp
{
    public class GetBudget
    {
        ISqlManager sqlManager;

        public GetBudget(ISqlManager sqlManager)
        {
            this.sqlManager = sqlManager;
        }


        [FunctionName("GetBudget")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");
            string annee;
            string classification;
            req.GetQueryParameterDictionary().TryGetValue("classification", out classification);
            req.GetQueryParameterDictionary().TryGetValue("annee", out annee);

            string retour = new BudgetDAO(sqlManager).GetBudget(annee, classification);
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(retour))
            };
        }
    }
}
