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
using System.Data.SqlClient;
using System.Net;
using System.Collections.Generic;
using System.Text;
using DAL;
using Microsoft.Extensions.DependencyInjection;
using DTO;

namespace AfupeoAzureFunctionApp
{
    public class SaveEncaissement
    {
        ISqlManager sqlManager;

        public SaveEncaissement(ISqlManager sqlManager)
        {
            this.sqlManager = sqlManager;
        }

        [FunctionName("SaveEncaissement")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            string json;
            dynamic data = await req.Content.ReadAsAsync<object>();
 
            try
            {

                json = new EncaissementDAO(sqlManager).SaveEncaissement(new EncaissementDTO()
                {
                    societe=data?.societe,
                    compte=data?.compte,
                    banque=data?.banque,
                    total_ht=data?.total_ht,
                    total_tva=data?.total_tva,
                    total_ttc=data?.total_ttc,
                    date_reglement=data?.date_reglement,
                    mode_paiement=data?.mode_paiement,
                    transfert_compte=data?.transfert_compte
                });
                 
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
