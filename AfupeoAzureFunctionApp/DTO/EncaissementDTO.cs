using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
    public class EncaissementDTO
    {
        public string id { get; set; }
        public string societe { get; set; }
        public string compte { get; set; }
        public string banque { get; set; }
        public string total_ht { get; set; }
        public string total_tva { get; set; }
        public string total_ttc { get; set; }
        public string date_reglement { get; set; }
        public string mode_paiement { get; set; }
        public string transfert_compte { get; set; }

        public override bool Equals(object obj)
        {
            return obj is EncaissementDTO dTO &&
                   id == dTO.id &&
                   societe == dTO.societe &&
                   compte == dTO.compte &&
                   banque == dTO.banque &&
                   total_ht == dTO.total_ht &&
                   total_tva == dTO.total_tva &&
                   total_ttc == dTO.total_ttc &&
                   date_reglement == dTO.date_reglement &&
                   mode_paiement == dTO.mode_paiement &&
                   transfert_compte == dTO.transfert_compte;
        }

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(id);
            hash.Add(societe);
            hash.Add(compte);
            hash.Add(banque);
            hash.Add(total_ht);
            hash.Add(total_tva);
            hash.Add(total_ttc);
            hash.Add(date_reglement);
            hash.Add(mode_paiement);
            hash.Add(transfert_compte);
            return hash.ToHashCode();
        }
    }
}
