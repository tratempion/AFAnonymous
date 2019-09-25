using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DAL;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AfupeoAzureFunctionApp
{
    public class GetDraftDecaissements
    {

        ISqlManager sqlManager;

        public GetDraftDecaissements(ISqlManager sqlManager)
        {
            this.sqlManager = sqlManager;
        }

        [FunctionName("GetDraftDecaissements")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string retour = "";
            try
            {
                retour = new DecaissementDAO(sqlManager).GetDraftDecaissement();
            }
            catch (Exception e)
            {

                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(e.Message)
                };
            }
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(retour)
            };
        }
    }
}
