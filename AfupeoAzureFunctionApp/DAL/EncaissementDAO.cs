using DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class EncaissementDAO
    {
        private ISqlManager sqlManager;
        public EncaissementDAO(ISqlManager sqlManager)
        {
            this.sqlManager = sqlManager;
        }
        public string DeleteEncaissement(string id)
        {
            Dictionary<string, object> dictionnaire = new Dictionary<string, object>();
            dictionnaire.Add("id", id);

            return sqlManager.ExecProcedure("DeleteEncaissement", dictionnaire);

        }
        public string EditEncaissement(EncaissementDTO encaissementDTO)
        {
            var dateReglement = DateTime.Parse(encaissementDTO.date_reglement, new System.Globalization.CultureInfo("fr-FR"));

            if (encaissementDTO.total_ht != null) encaissementDTO.total_ht = ((string)encaissementDTO.total_ht).Split(',')[0].Replace(" ", "").Replace("€", "").Trim();
            if (encaissementDTO.total_tva != null) encaissementDTO.total_tva = ((string)encaissementDTO.total_tva).Split(',')[0].Replace(" ", "").Replace("€", "").Trim();
            if (encaissementDTO.total_ttc != null) encaissementDTO.total_ttc = ((string)encaissementDTO.total_ttc).Split(',')[0].Replace(" ", "").Replace("€", "").Trim();
            Dictionary<string, object> dictionnaire = new Dictionary<string, object>();
            dictionnaire.Add("id", encaissementDTO.id);
            dictionnaire.Add("societe", encaissementDTO.societe);
            dictionnaire.Add("compte", encaissementDTO.compte);
            dictionnaire.Add("banque", encaissementDTO.banque);
            dictionnaire.Add("total_ht", encaissementDTO.total_ht);
            dictionnaire.Add("total_tva", encaissementDTO.total_tva);
            dictionnaire.Add("total_ttc", encaissementDTO.total_ttc);
            dictionnaire.Add("date_reglement", dateReglement.ToString("dd/MM/yyyy"));
            dictionnaire.Add("mode_paiement", encaissementDTO.mode_paiement);
            dictionnaire.Add("transfert_compte", encaissementDTO.transfert_compte);
            return sqlManager.ExecProcedure("EditEncaissement", dictionnaire);

        }

        public string GetAllEncaissement()
        {
            return sqlManager.ExecProcedure("GetAllEncaissement");
        }

        public string GetEncaissementInterval(string debut_interval, string fin_interval)
        {
            Dictionary<string, object> dico = new Dictionary<string, object>()
            {
                {"debut_interval",DateTime.Parse(debut_interval).ToString()},
                {"fin_interval",DateTime.Parse(fin_interval).ToString()}
            };
            return sqlManager.ExecProcedure("GetEncaissementInterval", dico);
        }

        public string SaveEncaissement(EncaissementDTO encaissementDTO)
        {
            var dateReglement = DateTime.Parse(encaissementDTO.date_reglement, new System.Globalization.CultureInfo("fr-FR"));

            Dictionary<string, object> dico = new Dictionary<string, object>()
            {
                {"societe",encaissementDTO.societe},
                {"compte",encaissementDTO.compte},
                {"banque",encaissementDTO.banque},
                {"total_ht",encaissementDTO.total_ht},
                {"total_tva",encaissementDTO.total_tva},
                {"total_ttc",encaissementDTO.total_ttc},
                {"date_reglement",dateReglement.ToString("dd/MM/yyyy")},
                {"mode_paiement",encaissementDTO.mode_paiement},
                {"transfert_compte",encaissementDTO.transfert_compte}
            };
            if (dico["total_ht"] != null) dico["total_ht"] = ((string)encaissementDTO.total_ht).Split(',')[0].Replace(" ", "").Replace("€", "").Replace("-", "").Trim();
            if (dico["total_tva"] != null) dico["total_tva"] = ((string)encaissementDTO.total_tva).Split(',')[0].Replace(" ", "").Replace("€", "").Replace("-", "").Trim();
            if (dico["total_ttc"] != null) dico["total_ttc"] = ((string)encaissementDTO.total_ttc).Split(',')[0].Replace(" ", "").Replace("€", "").Replace("-", "").Trim();
            if (dico["transfert_compte"] != null) dico["transfert_compte"] = ((string)encaissementDTO.transfert_compte).Split(',')[0].Replace(" ", "").Replace("€", "").Replace("-", "").Trim();

            return sqlManager.ExecProcedure("SaveEncaissement", dico);
        }
    }
}
