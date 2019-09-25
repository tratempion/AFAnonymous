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
using System.Text;

namespace AfupeoAzureFunctionApp
{
    public  class GetAllAccountPositionAtDate
    {
        ISqlManager sqlManager;

        public GetAllAccountPositionAtDate(ISqlManager sqlManager)
        {
            this.sqlManager = sqlManager;
        }
        [FunctionName("GetAllAccountPositionAtDate")]
        public  async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get",  Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string date;
            req.GetQueryParameterDictionary().TryGetValue("date", out date);


            string retour = "";
            try
            {
                retour = new CompteDAO(sqlManager).GetAllAccountPositionAtDate(date);
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
                Content = new StringContent(retour, Encoding.UTF8, "application/json")
            };
        }
    }
}
