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
using DTO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text;

namespace AfupeoAzureFunctionApp
{
    public class GetAccountType
    {
        ISqlManager sqlManager;

        public GetAccountType(ISqlManager sqlManager)
        {
            this.sqlManager = sqlManager;
        }


        [FunctionName("getAccountType")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string retour = "";
            try
            {
                retour = new BanqueDAO(sqlManager).GetAccountType();
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
                Content = new StringContent(retour,Encoding.UTF8,"application/json")
            };
        }
    }
}
