using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using DAL;
using System.Collections.Generic;
using System.Text;

namespace AfupeoAzureFunctionApp
{
    public  class GetTotalPositionAtInterval
    {
        ISqlManager sqlManager;

        public GetTotalPositionAtInterval(ISqlManager sqlManager)
        {
            this.sqlManager = sqlManager;
        }

        [FunctionName("GetTotalPositionAtInterval")]
        public  async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string date_debut;
            string date_fin;
            string mode;
            string compte;
            req.GetQueryParameterDictionary().TryGetValue("date_debut", out date_debut);
            req.GetQueryParameterDictionary().TryGetValue("date_fin", out date_fin);
            req.GetQueryParameterDictionary().TryGetValue("mode", out mode);
            req.GetQueryParameterDictionary().TryGetValue("compte", out compte);

            string retour = "";
            try
            {
               
                retour = new CompteDAO(sqlManager).GetTotalPositionAtInterval(date_debut, date_fin, mode, compte);
            }
            catch (Exception e)
            {

                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(e.Message,Encoding.UTF8,"application/json")
                };
            }
            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(retour)
            };
        }
    }
}
