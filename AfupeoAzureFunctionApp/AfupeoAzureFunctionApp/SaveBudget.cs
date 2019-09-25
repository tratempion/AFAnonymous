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
using System.Collections.Generic;
using DAL;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using DTO;

namespace AfupeoAzureFunctionApp
{
    public class SaveBudget
    {
        ISqlManager sqlManager;

        public SaveBudget(ISqlManager sqlManager)
        {
            this.sqlManager = sqlManager;
        }

        [FunctionName("SaveBudget")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string json;
            dynamic data = await req.Content.ReadAsAsync<object>();

            string annee = data?.annee;
            string id_sous_classification = data?.id_sous_classification;
            string budget= data?.budget;
            string mois_valeur= data?.mois_valeur;
            try
            {

                json = new BudgetDAO(sqlManager).SaveBudget(new BudgetDTO()
                {
                    id = annee,
                    id_sous_classification = id_sous_classification,
                    budget = budget,
                    periode = new DateTime(int.Parse(annee), int.Parse(mois_valeur), 1)

                });
            }
            catch (Exception e)
            {
                return new HttpResponseMessage(HttpStatusCode.Conflict)
                {
                    Content = new StringContent(e.ToString())
                };
            }
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json)
            };
        }
    }
}
