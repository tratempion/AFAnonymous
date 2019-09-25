using System;
using System.Collections.Generic;
using System.Text;
using DTO;
namespace DAL
{
    public class DecaissementDAO
    {
        private ISqlManager sqlManager;
        public DecaissementDAO(ISqlManager sqlManager)
        {
            this.sqlManager = sqlManager;
        }
        public string DeleteDecaissement(string id)
        {
            Dictionary<string, object> dictionnaire = new Dictionary<string, object>();
            dictionnaire.Add("id", id.ToString());

            return sqlManager.ExecProcedure("DeleteDecaissement", dictionnaire);
        }
        public string EditDecaissement(DecaissementDTO decaissementDTO)
        {
            Dictionary<string, object> dictionnaire = new Dictionary<string, object>();
            var date_compta = DateTime.Parse(decaissementDTO.date_compta, new System.Globalization.CultureInfo("fr-FR"));
            var date_operation = DateTime.Parse(decaissementDTO.date_operation, new System.Globalization.CultureInfo("fr-FR"));
            var date_valeur = DateTime.Parse(decaissementDTO.date_valeur, new System.Globalization.CultureInfo("fr-FR"));

            dictionnaire.Add("id", decaissementDTO.id);
            dictionnaire.Add("operation", decaissementDTO.operation);
            dictionnaire.Add("compte", decaissementDTO.compte);
            dictionnaire.Add("banque", decaissementDTO.banque);
            dictionnaire.Add("mode_paiement", decaissementDTO.mode_paiement);
            dictionnaire.Add("date_compta", date_compta.ToString("dd/MM/yyyy"));
            dictionnaire.Add("date_operation", date_operation.ToString("dd/MM/yyyy"));
            dictionnaire.Add("date_valeur", date_valeur.ToString("dd/MM/yyyy"));
            dictionnaire.Add("montant_ttc", decaissementDTO.montant_ttc);
            dictionnaire.Add("tva_deductible", decaissementDTO.tva_deductible);
            dictionnaire.Add("montant_ht", decaissementDTO.montant_ht);
            dictionnaire.Add("versement_tva", decaissementDTO.versement_tva);
            dictionnaire.Add("id_sous_classification", decaissementDTO.id_sous_classification);
            dictionnaire.Add("details", decaissementDTO.details);
            dictionnaire.Add("facture_verifie", decaissementDTO.facture_verifie);
            dictionnaire.Add("mois_valeur", decaissementDTO.mois_valeur);
            dictionnaire.Add("transfert_compte", decaissementDTO.transfert_compte);
            return sqlManager.ExecProcedure("EditDecaissement", dictionnaire);

        }

        public string ValidateDecaissement(DecaissementDTO decaissementDTO)
        {
            Dictionary<string, object> dictionnaire = new Dictionary<string, object>();
            dictionnaire.Add("id_sous_classification", decaissementDTO.id_sous_classification);
            dictionnaire.Add("id_decaissement", decaissementDTO.id);
            dictionnaire.Add("mois_valeur", decaissementDTO.mois_valeur);
            dictionnaire.Add("tva_deductible", decaissementDTO.tva_deductible);
            dictionnaire.Add("montant_ht", decaissementDTO.montant_ht);
            dictionnaire.Add("versement_tva", decaissementDTO.versement_tva);
            return sqlManager.ExecProcedure("ValidateDecaissement", dictionnaire);
        }

        public string SaveDraftDecaissement(DecaissementDTO decaissementDTO)
        {
            Dictionary<string, object> dictionnaire = new Dictionary<string, object>();

            dictionnaire.Add("operation", decaissementDTO.operation);
            dictionnaire.Add("compte", decaissementDTO.compte);
            dictionnaire.Add("banque", decaissementDTO.banque);
            dictionnaire.Add("mode_paiement", decaissementDTO.mode_paiement);
            dictionnaire.Add("date_compta", decaissementDTO.date_compta);
            dictionnaire.Add("date_operation", decaissementDTO.date_operation);
            dictionnaire.Add("date_valeur", decaissementDTO.date_valeur);
            dictionnaire.Add("montant_ttc", decaissementDTO.montant_ttc);
            if (dictionnaire["montant_ttc"] != null) dictionnaire["montant_ttc"] = ((string)decaissementDTO.montant_ttc).Split(',')[0].Replace(" ", "").Replace("€", "").Replace("-", "").Trim();
            return sqlManager.ExecProcedure("SaveDraftDecaissement", dictionnaire);

        }

        public string GetValidatedDecaissements()
        {
            return sqlManager.ExecProcedure("GetValidatedDecaissements");
        }

        public string GetDraftDecaissement()
        {
            return sqlManager.ExecProcedure("GetDraftDecaissements");
        }



    }
}
