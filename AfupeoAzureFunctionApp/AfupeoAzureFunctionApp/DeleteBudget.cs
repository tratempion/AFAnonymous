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

namespace AfupeoAzureFunctionApp
{

    public class DeleteBudget
    {
        ISqlManager sqlManager;
        public DeleteBudget(ISqlManager sqlManager)
        {
            this.sqlManager = sqlManager;
        }

        [FunctionName("DeleteBudget")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            Dictionary<string, object> dictionnaire = new Dictionary<string, object>();

            string json="";
            dynamic data = await req.Content.ReadAsAsync<object>();
            dictionnaire.Add("id", data?.id);
            try
            {

                json=new BudgetDAO(sqlManager).DeleteBudget(new Guid(((Object)(data?.id)).ToString()));
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
