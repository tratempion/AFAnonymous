using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net.Http;
using System.Net;
using DAL;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AfupeoAzureFunctionApp
{
    public class GetPaymentMethod
    {
        ISqlManager sqlManager;

        public GetPaymentMethod(ISqlManager sqlManager)
        {
            this.sqlManager = sqlManager;
        }

        [FunctionName("GetPaymentMethod")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            string retour = "";
            try
            {
                retour = new PaiementMethodDAO(sqlManager).GetPayementMethod();
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
                Content = new StringContent(retour)
            };
        }
    }
}
