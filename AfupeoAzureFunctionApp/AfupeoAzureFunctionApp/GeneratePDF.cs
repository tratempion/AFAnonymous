using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DAL;
using System.Net.Http;
using System.Net;

namespace AfupeoAzureFunctionApp
{
    public class GeneratePDF
    {

        ISqlManager sqlManager;
        public GeneratePDF(ISqlManager sql)
        {
            this.sqlManager = sql;
        }
        [FunctionName("GeneratePDF")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            try
            {

                string annee = req.RequestUri.ToString();

                annee = annee.Split("annee=")[1];
                log.LogInformation("C# HTTP trigger function processed a request.");
                var retour = req.CreateResponse(HttpStatusCode.MovedPermanently);
                retour.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue()
                {
                    Public = true,
                    MaxAge = TimeSpan.FromSeconds(0)
                };
                retour.Headers.Location = new Uri(new BudgetDAO(sqlManager).GeneratePDF(annee).Result);
                return retour;

            }
            catch (Exception e)
            {

                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }
    }
}
