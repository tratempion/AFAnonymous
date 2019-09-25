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
using System.Collections.Generic;
using System.Net;
using DAL;
using Microsoft.Extensions.DependencyInjection;
using DTO;

namespace AfupeoAzureFunctionApp
{
    public class EditDecaissement
    {
        ISqlManager sqlManager;

        public EditDecaissement(ISqlManager sqlManager)
        {
            this.sqlManager = sqlManager;
        }

        [FunctionName("EditDecaissement")]
        public async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage req,
            ILogger log)
        {

            string json;
            dynamic data = await req.Content.ReadAsAsync<object>();

            try
            {
                DecaissementDTO decaissementDTO = new DecaissementDTO()
                {
                    id = data?.id,
                    banque = data?.banque,
                    compte = data?.compte,
                    date_compta = data?.date_compta,
                    date_operation = data?.date_operation,
                    date_valeur = data?.date_valeur,
                    details = data?.details,
                    facture_verifie = data?.facture_verifie,
                    id_sous_classification = data?.id_sousclassification,
                    mode_paiement = data?.mode_paiement,
                    mois_valeur = data?.mois_valeur,
                    montant_ht = data?.montant_ht,
                    montant_ttc = data?.montant_ttc,
                    operation = data?.operation,
                    transfert_compte = data?.transfert_compte,
                    tva_deductible = data?.tva_deductible,
                    versement_tva = data?.versement_tva
                };
                if (decaissementDTO.montant_ttc != null) decaissementDTO.montant_ttc = decaissementDTO.montant_ttc.Replace("€", "").Replace(",", "").Trim();
                if (decaissementDTO.tva_deductible != null) decaissementDTO.tva_deductible = decaissementDTO.tva_deductible.Replace("€", "").Replace(",", "").Trim();
                if (decaissementDTO.montant_ht != null) decaissementDTO.montant_ht = decaissementDTO.montant_ht.Replace("€", "").Replace(",", "").Trim();
                if (decaissementDTO.versement_tva != null) decaissementDTO.versement_tva = decaissementDTO.versement_tva.Replace("€", "").Replace(",", "").Trim();
                if (decaissementDTO.transfert_compte != null) decaissementDTO.transfert_compte = decaissementDTO.transfert_compte.Replace("€", "").Replace(",", "").Trim();
                json = new DecaissementDAO(sqlManager).EditDecaissement(decaissementDTO);
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
