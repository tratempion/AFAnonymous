using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using DAL;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AfupeoAzureFunctionApp
{
    public class GetEncaissementInterval
    {
        ISqlManager sqlManager;

        public GetEncaissementInterval(ISqlManager sqlManager)
        {
            this.sqlManager = sqlManager;
        }

        [FunctionName("GetEncaissementInterval")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");
            string debut_interval;
            string fin_interval;
            req.GetQueryParameterDictionary().TryGetValue("debut_interval", out debut_interval);
            req.GetQueryParameterDictionary().TryGetValue("fin_interval", out fin_interval);
            if (string.IsNullOrEmpty(debut_interval))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    Content = new StringContent( "Missing parameter : 'debut_interval'")
                };

            }
            if (string.IsNullOrEmpty(fin_interval))
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Missing parameter : 'fin_interval'")
                };

            }

            string retour = "";
            try
            {
                retour = new EncaissementDAO(sqlManager).GetEncaissementInterval(DateTime.Parse(debut_interval).ToString(), DateTime.Parse(fin_interval).ToString());
            }
            catch (Exception e)
            {

                return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent(e.Message)
                };
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(retour)
            };

        }
    }
}
