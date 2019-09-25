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
using System.Text;

namespace AfupeoAzureFunctionApp
{
    public class GetBudgetBilan
    {
        ISqlManager sqlManager;

        public GetBudgetBilan(ISqlManager sqlManager)
        {
            this.sqlManager = sqlManager;
        }

        [FunctionName("GetBudgetBilan")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get",  Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string annee;
            req.GetQueryParameterDictionary().TryGetValue("annee", out annee);
            string retour = "";
            try
            {
                retour = new BudgetDAO(sqlManager).GetBudgetBilan(annee);
            }
            catch (Exception e)
            {

                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(e.Message)
                };
            }
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(retour,Encoding.UTF8, "application/json")
            };

        }
    }
}
