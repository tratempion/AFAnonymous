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
    public class EditBudget
    {
        ISqlManager sqlManager;
        public EditBudget(ISqlManager sqlManager)
        {
            this.sqlManager = sqlManager;
        }
        [FunctionName("EditBudget")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous,"post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {

            string json;
            dynamic data = await req.Content.ReadAsAsync<object>();
            string id = data?.id;
            string budget = data?.budget;
            string sousclass= data?.id_sous_classification;
            string annee = data?.annee;
            string mois= data?.mois_valeur;

            try
            {

                json = new BudgetDAO(sqlManager).EditBudget(new BudgetDTO()
                {
                    id = data?.id,
                    budget = data?.budget,
                    id_sous_classification = data?.id_sous_classification,
                    periode = new DateTime(int.Parse(annee), int.Parse(mois), 1),
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
