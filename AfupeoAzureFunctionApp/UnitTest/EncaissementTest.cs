using Microsoft.VisualStudio.TestTools.UnitTesting;
using DAL;
using DTO;
using AfupeoAzureFunctionApp;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace IntegrationTest
{
    [TestClass]
    public class EncaissementTest
    {
        internal class ExpectedEncaissement
        {
            public string id { get; set; }
            public string societe { get; set; }
            public string compte { get; set; }
            public string banque{ get; set; }
            public string total_ht { get; set; }
            public string total_ttc { get; set; }
            public string total_tva{ get; set; }
            public string date_reglement { get; set; }
            public string mode_paiement{ get; set; }
            public string transfert_compte{ get; set; }
            public string date_ajout{ get; set; }

        }
        internal class ExpectedReturn
        {
            public string id { get; set; }
            public string retour { get; set; }
        }

        ISqlManager sqlManager;
        [TestInitialize]
        public void SetupTestDataBase()
        {
            this.sqlManager = InitDataBase.GetConnexion();
        }
        [TestMethod]

        public void SaveAndGetEncaissementTest()
        {
            var encaissementDAO = new EncaissementDAO(sqlManager);
            EncaissementDTO DTOTmp = new EncaissementDTO()
            {
                banque = "BNP",
                date_reglement = "25/02/2019",
                total_ttc = "12",
                compte = "compte 2 BNP",
                mode_paiement = "VIR",
                societe = "BIS",
                total_ht = "1",
                total_tva = "1",
                transfert_compte = "0"
            };
            var idRetour = encaissementDAO.SaveEncaissement(DTOTmp);
            var result = JsonConvert.DeserializeObject<List<ExpectedEncaissement>>(encaissementDAO.GetAllEncaissement());
            Assert.IsTrue(result[0].compte == DTOTmp.compte);
            Assert.IsTrue(Convert.ToDateTime(result[0].date_reglement) == Convert.ToDateTime(DTOTmp.date_reglement));
            Assert.IsTrue(Double.Parse(result[0].total_ttc.Replace('.', ',')) == Double.Parse(DTOTmp.total_ttc.Replace('.', ',')));

        }
        [TestMethod]
        public void DeleteEncaissementTest()
        {
            var encaissementDAO =  new EncaissementDAO(sqlManager);
            EncaissementDTO DTOTmp = new EncaissementDTO()
            {
                banque = "BNP",
                date_reglement = "25/02/2019",
                total_ttc = "12",
                 compte="compte 2 BNP",
                 mode_paiement="VIR",
                 societe="BIS",
                 total_ht="1",
                 total_tva="1",
                 transfert_compte="0"
            };
            string retour = encaissementDAO.SaveEncaissement(DTOTmp);
            var id= JsonConvert.DeserializeObject < List < ExpectedReturn >> (retour)[0].id;
            var result = JsonConvert.DeserializeObject<List<EncaissementDTO>>(encaissementDAO.GetAllEncaissement());
            Assert.IsTrue(result.Count == 1);
            encaissementDAO.DeleteEncaissement(id);
            result = JsonConvert.DeserializeObject<List<EncaissementDTO>>(encaissementDAO.GetAllEncaissement());
            Assert.IsTrue(result.Count == 0);
        }
        [TestMethod]
        public void EditDecaissementTest()
        {
            var encaissementDAO = new EncaissementDAO(sqlManager);
            EncaissementDTO DTOTmp1 = new EncaissementDTO()
            {
                banque = "BNP",
                date_reglement = "25/02/2019",
                mode_paiement = "AUCUN",
                total_ttc = "1000",
                compte="compte 2 BNP",
                societe="TERSIO",
                total_ht="800",
                total_tva="200",
                transfert_compte="1"
            };
            EncaissementDTO DTOTmp2 = new EncaissementDTO()
            {
                banque = "HSBC",
                date_reglement = "21/03/2017",
                mode_paiement = "VIR",
                total_ttc = "5000",
                societe = "BIS",
                total_ht = "3000",
                total_tva = "2000",
                transfert_compte = "0"
            };
            encaissementDAO.SaveEncaissement(DTOTmp1);
            var result = JsonConvert.DeserializeObject<List<ExpectedEncaissement>>(encaissementDAO.GetAllEncaissement());
            DTOTmp2.id = result[0].id;
            DTOTmp1.id = result[0].id;
            encaissementDAO.EditEncaissement(DTOTmp2);
            result = JsonConvert.DeserializeObject<List<ExpectedEncaissement>>(encaissementDAO.GetAllEncaissement());
            var retour = result[0];
            var DTOTmp3 = new EncaissementDTO()
            {
                id=retour.id,
                banque = retour.banque,
                date_reglement = DateTime.Parse(retour.date_reglement.Split('T')[0].Replace('-','/'),CultureInfo.CurrentCulture).ToString("dd/MM/yyyy"),
                mode_paiement = retour.mode_paiement,
                total_ttc = Double.Parse(retour.total_ttc.Replace('.',',')).ToString(),
                societe = retour.societe,
                total_ht = Double.Parse(retour.total_ht.Replace('.', ',')).ToString(),
                total_tva = Double.Parse(retour.total_tva.Replace('.', ',')).ToString(),
                transfert_compte = Double.Parse(retour.transfert_compte.Replace('.', ',')).ToString()
            };
            Assert.IsTrue(DTOTmp3.Equals(DTOTmp2));
            DTOTmp2.banque = "BNP";
            DTOTmp2.compte = "compte 2 BNP";
            encaissementDAO.EditEncaissement(DTOTmp2);
            result = JsonConvert.DeserializeObject<List<ExpectedEncaissement>>(encaissementDAO.GetAllEncaissement());
            Assert.IsTrue(result[0].compte.Equals("compte 2 BNP"));


        }
        
    }
}
