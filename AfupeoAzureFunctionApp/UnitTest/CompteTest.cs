using AfupeoAzureFunctionApp;
using DAL;
using DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;

namespace IntegrationTest
{
    [TestClass]
    public class CompteTest
    {

        ISqlManager sqlManager;
        internal class ExpectedPositionInterval
        {
            public string date { get; set; }
            public string position { get; set; }
        }
        internal class ExpectedPositionDate
        {
            public string numero { get; set; }
            public string position { get; set; }
        }
        internal class ExpectedReturn
        {
            public string id { get; set; }
            public string retour { get; set; }
        }
        [TestInitialize]
        public void SetupTestDataBase()
        {
            this.sqlManager = InitDataBase.GetConnexion();
        }
        [TestMethod]
        public void GetAllAccountPositionAtDate()
        {
            var decaissementDAO = new DecaissementDAO(sqlManager);
            var encaissementDAO = new EncaissementDAO(sqlManager);
            var compteDAO = new CompteDAO(sqlManager);
            var classificationDAO = new ClassificationDAO(sqlManager);

            var resultPosition = GetTotalPosition(compteDAO);
            //on s'assure qu'au début, la position est à zero
            Assert.AreEqual(0, resultPosition);
            string retour = encaissementDAO.SaveEncaissement(new EncaissementDTO()
            {
                banque = "BNP",
                compte = "compte 2 BNP",
                total_ttc = "12",
                date_reglement = "02/06/2019"

            }) ;
            resultPosition = GetTotalPosition(compteDAO);
            Assert.AreEqual(12, resultPosition);

            //on fait un deuxieme encaissement dans un autre compte
            retour = encaissementDAO.SaveEncaissement(new EncaissementDTO()
            {
                banque = "HSBC",
                total_ttc = "13",
                date_reglement = "02/06/2019"

            }) ;
            resultPosition = GetTotalPosition(compteDAO);

            Assert.AreEqual(25, resultPosition);
            //on fait un troisieme encaissement, mais en 2020 pour etre sur qu'il n'est pas comptabilisé
            retour = encaissementDAO.SaveEncaissement(new EncaissementDTO()
            {
                banque = "BPOP",
                total_ttc = "11",
                date_reglement = "01/01/2020"

            });
            resultPosition = GetTotalPosition(compteDAO);
            Assert.AreEqual(25, resultPosition);


            string retour1 = decaissementDAO.SaveDraftDecaissement(new DecaissementDTO()
            {
                operation = "test",
                banque = "BNP",
                montant_ttc = "9.5",
                date_operation = "05/06/2019",
                date_compta = "05/06/2019",
                date_valeur = "05/06/2019",
                facture_verifie = "1"//ne devrai pas etre pris en compte
            });
            retour = JsonConvert.DeserializeObject< List<ExpectedReturn>>(retour1)[0].retour;
            //on s'assure qu'un decaissement non validé n'est pas pris en compte
            resultPosition = GetTotalPosition(compteDAO);
            Assert.AreEqual(25, resultPosition);


            string idSousCLass = "";
            var dicoRetour= JsonConvert.DeserializeObject 
                < Dictionary < string, List< SousClassificationDTO >>>(
                    classificationDAO.GetClassification());
            foreach (var item in dicoRetour)
            {
                idSousCLass = item.Value[0].id;
                break;
            }


            Assert.AreEqual(25, resultPosition);
            var retour2=decaissementDAO.ValidateDecaissement(new DecaissementDTO()
            {
                id = retour,
                id_sous_classification=idSousCLass,
                mois_valeur="6"
            });
            var resultValidated = JsonConvert.DeserializeObject<List<DecaissementDTO>>(decaissementDAO.GetValidatedDecaissements());
            Assert.IsTrue(resultValidated.Count == 1);

            resultPosition = GetTotalPosition(compteDAO);

            Assert.AreEqual(15.5, resultPosition);

        }
        internal double GetTotalPosition(CompteDAO compteDAO)
        {
            string retour = compteDAO.GetAllAccountPositionAtDate("31/12/2019");
            var resultPosition= JsonConvert.DeserializeObject<List<ExpectedPositionDate>>(retour);
            double total = 0;
            foreach (var item in resultPosition)
            {
                total += double.Parse((item.position ?? "0").Replace('.', ','));
            }
            return total;
        }
       
        [TestMethod]
        public void GetPositionPeriode()
        {
            var decaissementDAO = new DecaissementDAO(sqlManager);
            var encaissementDAO = new EncaissementDAO(sqlManager);
            var compteDAO = new CompteDAO(sqlManager);
            
            //on s'assure qu'au début, la position est à zero
            string retour = compteDAO.GetTotalPositionAtInterval(DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("dd/MM/yyyy"), "annee");
            var resultPosition = JsonConvert.DeserializeObject<List<ExpectedPositionInterval>>( retour);
            Assert.AreEqual(0, double.Parse(resultPosition[0].position.Replace('.',',')));

            //on enregistre un decaissement non validé
            var decaissement1 = new DecaissementDTO()
            {
                banque = "BNP",
                date_compta = "20/02/2019",
                date_valeur = "20/02/2019",
                date_operation = "20/02/2019",
                mode_paiement = "CHQ",
                operation = "unitTestDec1",
                montant_ttc = "12.2"
            };
            decaissementDAO.SaveDraftDecaissement(decaissement1);
            retour = decaissementDAO.GetDraftDecaissement();
            var resultDecaissement = JsonConvert.DeserializeObject<List<DecaissementDTO>>(
                decaissementDAO.GetDraftDecaissement());
            decaissement1.id = resultDecaissement[0].id;
            var listClassif = JsonConvert.DeserializeObject<Dictionary<string, List<SousClassificationDTO>>>(
                                new ClassificationDAO(sqlManager).GetClassification());
            decaissement1.id_sous_classification = listClassif["ADMINISTRATIF"][1].id;
            decaissement1.mois_valeur = "2";
            //on valide le decaissement
            decaissementDAO.ValidateDecaissement(decaissement1);
            retour = compteDAO.GetTotalPositionAtInterval("07/06/2019", "07/06/2019", "jours");
            resultPosition = JsonConvert.DeserializeObject<List<ExpectedPositionInterval>>(retour);
            //on s'assure que la postition après le decaissement as bien changé
            Assert.AreEqual(-12.2, double.Parse(resultPosition[0].position.Replace('.', ',')));
            retour =  compteDAO.GetTotalPositionAtInterval("13/01/2019", "13/01/2019", "jours");
            resultPosition = JsonConvert.DeserializeObject<List<ExpectedPositionInterval>>(retour);
            //on s'assure que la position AVANT le decaissement reste inchangé
            Assert.AreEqual(0, double.Parse(resultPosition[0].position.Replace('.', ',')));
            var encaissement = new EncaissementDTO()
            {
                banque = "HSBC",
                compte = "compte 1 HSBC",
                mode_paiement = "CHQ",
                total_ttc = "20",
                societe = "UNO",
                date_reglement = "14/05/2019"
            };
            encaissementDAO.SaveEncaissement(encaissement);
            retour = compteDAO.GetTotalPositionAtInterval("07/06/2019", "07/06/2019", "jours");
            resultPosition = JsonConvert.DeserializeObject<List<ExpectedPositionInterval>>(retour);
            //on s'assure que la position prend en compte les encaissement et decaissement meme sur différentes banque/compte
            Assert.AreEqual(7.8, double.Parse(resultPosition[0].position.Replace('.', ',')));


            encaissement = new EncaissementDTO()
            {
                banque = "HSBC",
                compte = "compte 1 HSBC",
                mode_paiement = "CHQ",
                total_ttc = "20",
                societe = "UNO",
                date_reglement = "14/05/2017"
            };
            encaissementDAO.SaveEncaissement(encaissement);
            retour = compteDAO.GetTotalPositionAtInterval("07/06/2018", "07/06/2018", "jours");
            resultPosition = JsonConvert.DeserializeObject<List<ExpectedPositionInterval>>(retour);
            //on s'assure que la position prend en compte l'encaissement en 2018 et ignore les mouvements ulterieurs
            Assert.AreEqual(20, double.Parse(resultPosition[0].position.Replace('.', ',')));

            retour = compteDAO.GetTotalPositionAtInterval("07/06/2019", "07/06/2019", "jours");
            resultPosition = JsonConvert.DeserializeObject<List<ExpectedPositionInterval>>(retour);
            //on s'assure que la position prend en compte tout les mouvements, peut importe le compte ou l'année
            Assert.AreEqual(27.8, double.Parse(resultPosition[0].position.Replace('.', ',')));



        }
    }
}