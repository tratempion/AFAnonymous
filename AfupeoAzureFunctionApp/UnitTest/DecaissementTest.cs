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
    public class DecaissementTest
    {
        ISqlManager sqlManager;
        internal class ExpectedDraftDecaissement
        {

            public string id { get; set; }
            public string operation { get; set; }
            public string banque { get; set; }
            public string compte { get; set; }
            public string mode_paiement { get; set; }
            public string date_compta { get; set; }
            public string date_operation { get; set; }
            public string date_valeur { get; set; }
            public string montant_ttc { get; set; }
            public string montant_ht { get; set; }
            public override bool Equals(object obj)
            {
                ExpectedDraftDecaissement decaissementTMP = (ExpectedDraftDecaissement)obj;
                if(
                    decaissementTMP.id==this.id &&
                    decaissementTMP.operation==this.operation &&
                    double.Parse(decaissementTMP.montant_ttc, CultureInfo.InvariantCulture )== double.Parse(this.montant_ttc, CultureInfo.InvariantCulture) &&
                    decaissementTMP.mode_paiement==this.mode_paiement &&
                    DateTime.Parse(decaissementTMP.date_valeur)==DateTime.Parse(this.date_valeur) &&
                    DateTime.Parse(decaissementTMP.date_operation) ==DateTime.Parse(this.date_operation) &&
                    DateTime.Parse(decaissementTMP.date_compta) ==DateTime.Parse(this.date_compta) &&
                    decaissementTMP.banque==this.banque)
                {
                    return true;
                }
                return false;
            }
            public override int GetHashCode()
            {
                return (this.id
                    +this.banque
                    +this.compte
                    +this.date_compta
                    +this.date_operation
                    +this.date_valeur
                    +this.mode_paiement
                    +this.montant_ttc
                    +this.operation
                    ).GetHashCode();
            }
        }
        internal class ExpectedSousClassification
        {
            public string id { get; set; }
            public string  name { get; set; }
        }
        internal class ExpectedClassification
        {
            public Dictionary<string,ExpectedSousClassification> classification { get; set; }
        }
        public DecaissementTest()
        {
            this.sqlManager = InitDataBase.GetConnexion();

        }

        [TestInitialize]
        public void SetupTestDataBase()
        {
            this.sqlManager = InitDataBase.GetConnexion();
        }
        [TestMethod]
        public void GetDraftDecaissementTest()
        {
            var decDAO = new DecaissementDAO(sqlManager);
            DecaissementDTO DTOTmp = new DecaissementDTO()
            {
                banque = "BNP",
                date_compta = "25/02/2019",
                date_operation = "26/02/2019",
                date_valeur = "27/02/2019",
                mode_paiement = "AUCUN",
                montant_ht = "1000",
                montant_ttc = "1200.12",
                operation = "unitTestDecaissement1"
            };
            decDAO.SaveDraftDecaissement(DTOTmp);
            var result = JsonConvert.DeserializeObject<List<ExpectedDraftDecaissement>> (decDAO.GetDraftDecaissement());
            Assert.IsTrue(result[0].banque == DTOTmp.banque);
            Assert.IsTrue(Convert.ToDateTime(result[0].date_compta) == Convert.ToDateTime(DTOTmp.date_compta));
            Assert.IsTrue(Convert.ToDateTime(result[0].date_operation) == Convert.ToDateTime(DTOTmp.date_operation));
            Assert.IsTrue(Convert.ToDateTime(result[0].date_valeur) == Convert.ToDateTime(DTOTmp.date_valeur));
            Assert.IsTrue(result[0].mode_paiement == DTOTmp.mode_paiement);
            Assert.IsTrue(Double.Parse(result[0].montant_ttc.Replace('.', ',')) == Double.Parse(DTOTmp.montant_ttc.Replace('.', ',')));
            Assert.IsTrue(result[0].operation == DTOTmp.operation);

        }
        [TestMethod]
        public void DeleteDecaissementTest()
        {
            var decDAO = new DecaissementDAO(sqlManager);
            DecaissementDTO DTOTmp = new DecaissementDTO()
            {
                banque = "BNP",
                date_compta = "25/02/2019",
                date_operation = "26/02/2019",
                date_valeur = "27/02/2019",
                mode_paiement = "AUCUN",
                montant_ht = "1000",
                montant_ttc = "1200.12",
                operation = "unitTestDecaissement1"
            };
            decDAO.SaveDraftDecaissement(DTOTmp);
            var result = JsonConvert.DeserializeObject<List<ExpectedDraftDecaissement>>(decDAO.GetDraftDecaissement());
            Assert.IsTrue(result.Count == 1);
            decDAO.DeleteDecaissement(result[0].id);
            result = JsonConvert.DeserializeObject<List<ExpectedDraftDecaissement>>(decDAO.GetDraftDecaissement());
            Assert.IsTrue(result.Count == 0);
        }
        [TestMethod]
        public void EditDecaissementTest()
        {
            var decDAO = new DecaissementDAO(sqlManager);
            DecaissementDTO DTOTmp1 = new DecaissementDTO()
            {
                banque = "BNP",
                date_compta = "25/02/2019",
                date_operation = "26/02/2019",
                date_valeur = "27/02/2019",
                mode_paiement = "AUCUN",
                montant_ht = "1000",
                montant_ttc = "1200.12",
                operation = "unitTestDecaissement1"
            };
            DecaissementDTO DTOTmp2 = new DecaissementDTO()
            {
                banque = "HSBC",
                date_compta = "25/5/2019",
                date_operation = "3/02/2019",
                date_valeur = "27/02/2017",
                mode_paiement = "CHQ",
                montant_ttc = "3",
                operation = "unitTestDecaissement2"
            };
            decDAO.SaveDraftDecaissement(DTOTmp1);
            var result = JsonConvert.DeserializeObject<List<ExpectedDraftDecaissement>>(decDAO.GetDraftDecaissement());
            DTOTmp2.id = result[0].id;
            decDAO.EditDecaissement(DTOTmp2);
            result = JsonConvert.DeserializeObject<List<ExpectedDraftDecaissement>>(decDAO.GetDraftDecaissement());
            Assert.IsTrue(result[0].banque == DTOTmp2.banque);
            Assert.IsTrue(Convert.ToDateTime(result[0].date_compta) == Convert.ToDateTime(DTOTmp2.date_compta));
            Assert.IsTrue(Convert.ToDateTime(result[0].date_operation) == Convert.ToDateTime(DTOTmp2.date_operation));
            Assert.IsTrue(Convert.ToDateTime(result[0].date_valeur) == Convert.ToDateTime(DTOTmp2.date_valeur));
            Assert.IsTrue(result[0].mode_paiement == DTOTmp2.mode_paiement);
            Assert.IsTrue(Double.Parse(result[0].montant_ttc.Replace('.', ',')) == Double.Parse(DTOTmp2.montant_ttc.Replace('.', ',')));
            Assert.IsTrue(result[0].operation == DTOTmp2.operation);
            Assert.IsTrue(result[0].compte.Equals( "compte 1 HSBC"));
            DTOTmp2.banque = "BNP";
            DTOTmp2.compte= "compte 2 BNP";
            decDAO.EditDecaissement(DTOTmp2);
            result = JsonConvert.DeserializeObject<List<ExpectedDraftDecaissement>>(decDAO.GetDraftDecaissement());
            Assert.IsTrue(result[0].compte.Equals( "compte 2 BNP"));


        }
        [TestMethod]
        public void ValidateDecaissement()
        {
            var decDAO = new DecaissementDAO(sqlManager);
            DecaissementDTO DTOTmp = new DecaissementDTO()
            {
                banque = "BNP",
                date_compta = "25/02/2019",
                date_operation = "26/02/2019",
                date_valeur = "27/02/2019",
                mode_paiement = "AUCUN",
                montant_ht = "1000",
                montant_ttc = "1200.12",
                operation = "unitTestDecaissement1"
            };
            decDAO.SaveDraftDecaissement(DTOTmp);
            var resultNotValidated = JsonConvert.DeserializeObject<List<DecaissementDTO>>(decDAO.GetDraftDecaissement());
            var resultValidated = JsonConvert.DeserializeObject<List<DecaissementDTO>>(decDAO.GetValidatedDecaissements());
            Assert.IsTrue(resultNotValidated.Count > 0);
            Assert.IsTrue(resultValidated.Count == 0);
            DTOTmp.id = resultNotValidated[0].id;
            decDAO.ValidateDecaissement(DTOTmp);
            resultValidated = JsonConvert.DeserializeObject<List<DecaissementDTO>>(decDAO.GetValidatedDecaissements());
            //ne devrait pas pouvoir valider un decaissement sans sous classif
            Assert.IsTrue(resultValidated.Count == 0);
            var listClassif =JsonConvert.DeserializeObject<Dictionary<string,List<SousClassificationDTO>>>( 
                new ClassificationDAO(sqlManager).GetClassification());
            DTOTmp.id_sous_classification= listClassif["ADMINISTRATIF"][1].id;
            DTOTmp.mois_valeur="2";
            decDAO.ValidateDecaissement(DTOTmp);
            resultValidated = JsonConvert.DeserializeObject<List<DecaissementDTO>>(decDAO.GetValidatedDecaissements());
            Assert.IsTrue(resultValidated.Count == 1);

        }
    }
}
