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
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace AfupeoAzureFunctionApp
{
    public class DeleteEncaissement
    {
        ISqlManager sqlManager;
        public DeleteEncaissement(ISqlManager sqlManager)
        {
            this.sqlManager = sqlManager;
        }
        [FunctionName("DeleteEncaissement")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous,  "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            dynamic data = await req.Content.ReadAsAsync<object>();
            if (data?.id == null)
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("Missing parameter : 'id'")
                };
            }

            string retour = "";
            try
            {
                retour = new EncaissementDAO(sqlManager).DeleteEncaissement((string)data?.id);
            }
            catch (Exception e)
            {

                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
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
