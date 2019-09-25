using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
    public class DecaissementDTO
    {
        public string id { get; set; }
        public string operation { get; set; }
        public string compte { get; set; }
        public string banque { get; set; }
        public string mode_paiement { get; set; }
        public string date_compta { get; set; }
        public string date_operation { get; set; }
        public string date_valeur { get; set; }
        public string montant_ttc { get; set; }
        public string tva_deductible { get; set; }
        public string montant_ht { get; set; }
        public string versement_tva { get; set; }
        public string id_sous_classification { get; set; }
        public string details { get; set; }
        public string facture_verifie { get; set; }
        public string mois_valeur { get; set; }
        public string transfert_compte { get; set; }

    }
}
