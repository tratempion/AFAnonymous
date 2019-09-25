using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DAL;
using DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AfupeoAzureFunctionApp
{
    public class SaveDraftDecaissement
    {

        ISqlManager sqlManager;

        public SaveDraftDecaissement(ISqlManager sqlManager)
        {
            this.sqlManager = sqlManager;
        }

        [FunctionName("SaveDraftDecaissement")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req,
            ILogger log)
        {
            string json;
            dynamic data = await req.Content.ReadAsAsync<object>();

            try
            {


                json = new DecaissementDAO(sqlManager).SaveDraftDecaissement(new DecaissementDTO()
                        {
                            operation = data?.operation,
                            compte = data?.compte,
                            banque = data?.banque,
                            mode_paiement = data?.mode_paiement,
                            date_compta = data?.date_compta,
                            date_operation = data?.date_operation,
                            date_valeur = data?.date_valeur,
                            montant_ttc = data?.montant_ttc
                        }
                    );

            }
            catch (SqlException e)
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
