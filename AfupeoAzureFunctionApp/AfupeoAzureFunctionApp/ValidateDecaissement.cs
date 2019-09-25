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
using DTO;

namespace AfupeoAzureFunctionApp
{
    public class ValidateDecaissement
    {
        ISqlManager sqlManager;

        public ValidateDecaissement(ISqlManager sqlManager)
        {
            this.sqlManager = sqlManager;
        }

        [FunctionName("ValidateDecaissement")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            Dictionary<string, object> dictionnaire = new Dictionary<string, object>();
            string json;
            dynamic data = await req.Content.ReadAsAsync<object>();
            dictionnaire.Add("id_decaissement", data?.id_decaissement);
            dictionnaire.Add("id_sous_classification", data?.id_sous_classification);
            dictionnaire.Add("mois_valeur", data?.mois_valeur);
            dictionnaire.Add("tva_deductible", data?.tva_deductible);
            dictionnaire.Add("montant_ht", data?.montant_ht);
            dictionnaire.Add("versement_tva", data?.versement_tva);
            string errorList = "";
            foreach (var item in dictionnaire)
            {
                if (item.Value == null)
                {
                    errorList += item.Key + " ";
                }
            }
            if (errorList != "")
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("erreur : parametres " + errorList + " manquant")
                };

            }
            try
            {
                json = new DecaissementDAO(sqlManager).ValidateDecaissement(new DecaissementDTO()
                {
                    id=data?.id_decaissement,
                    id_sous_classification=data?.id_sous_classification,
                    mois_valeur=data?.mois_valeur,
                    tva_deductible = data?.tva_deductible,
                    montant_ht= data?.montant_ht,
                    versement_tva=data?.versement_tva
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
