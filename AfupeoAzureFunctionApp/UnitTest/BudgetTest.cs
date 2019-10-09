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
    public class BudgetTest
    {
        internal class ExpectedReturn
        {
            public string id { get; set; }
            public string retour { get; set; }
        }

        ISqlManager sqlManager;
        [TestInitialize]
        public void SetupTestDataBase()
        {
            //this.sqlManager = InitDataBase.GetConnexion();
        }
        [TestMethod]
        public void SaveAndGetBudget()
        {
            var budgetDAO = new BudgetDAO(sqlManager);
            var listClassif = JsonConvert.DeserializeObject<Dictionary<string, List<SousClassificationDTO>>>(
               new ClassificationDAO(sqlManager).GetClassification());
            var budgetDTO = new BudgetDTO()
            {
                budget = "2",
                id_sous_classification = listClassif["EQUIPEMENTS_FOURNITURES_REPARATION"][1].id,
                periode = DateTime.Parse("25/02/2019", CultureInfo.CurrentCulture)
            };
            budgetDAO.SaveBudget(budgetDTO);
            var result = JsonConvert.DeserializeObject<Dictionary<string, List<DTO.BudgetDTO>>>(
                budgetDAO.GetBudget("2019", "EQUIPEMENTS_FOURNITURES_REPARATION"));
            Assert.AreEqual(Double.Parse(result["1"][0].budget.Replace('.', ',')), Double.Parse(budgetDTO.budget.Replace('.', ',')));
        }
        [TestMethod]
        public void DeleteBudget()
        {
            var budgetDAO = new BudgetDAO(sqlManager);
            var listClassif = JsonConvert.DeserializeObject<Dictionary<string, List<SousClassificationDTO>>>(
               new ClassificationDAO(sqlManager).GetClassification());
            var budgetDTO1 = new BudgetDTO()
            {
                budget = "2",
                id_sous_classification = listClassif["EQUIPEMENTS_FOURNITURES_REPARATION"][1].id,
                periode = DateTime.Parse("25/02/2019", CultureInfo.CurrentCulture)
            };
            budgetDAO.SaveBudget(budgetDTO1);
            var result = JsonConvert.DeserializeObject<Dictionary<string, List<BudgetDTO>>>(
                budgetDAO.GetBudget("2019", "EQUIPEMENTS_FOURNITURES_REPARATION"));
            Assert.IsTrue(result["1"].Count > 0);
            budgetDAO.DeleteBudget(new Guid(result["1"][0].id));
            result = JsonConvert.DeserializeObject<Dictionary<string, List<BudgetDTO>>>(
                budgetDAO.GetBudget("2019", "EQUIPEMENTS_FOURNITURES_REPARATION"));
            Assert.IsTrue(result["1"].Count ==  0);
        }
        [TestMethod]
        public void EditBudget()
        {
            var budgetDAO = new BudgetDAO(sqlManager);
            var listClassif = JsonConvert.DeserializeObject<Dictionary<string, List<SousClassificationDTO>>>(
               new ClassificationDAO(sqlManager).GetClassification());
            var budgetDTO1 = new BudgetDTO()
            {
                budget = "2",
                id_sous_classification = listClassif["EQUIPEMENTS_FOURNITURES_REPARATION"][1].id,
                periode = DateTime.Parse("25/02/2019", CultureInfo.CurrentCulture)
            };
            budgetDAO.SaveBudget(budgetDTO1);
            var result = JsonConvert.DeserializeObject<Dictionary<string, List<BudgetDTO>>>(
                budgetDAO.GetBudget("2019", "EQUIPEMENTS_FOURNITURES_REPARATION"));
            var budgetDTO2 = new BudgetDTO()
            {
                budget = "3",
                id = result["1"][0].id,
                id_sous_classification = listClassif["EQUIPEMENTS_FOURNITURES_REPARATION"][0].id,
                sous_classification = listClassif["EQUIPEMENTS_FOURNITURES_REPARATION"][0].name,
                periode = DateTime.Parse("23/11/2019", CultureInfo.CurrentCulture)
            };
            budgetDAO.EditBudget(budgetDTO2);
            result = JsonConvert.DeserializeObject<Dictionary<string, List<BudgetDTO>>>(
                budgetDAO.GetBudget("2019", "EQUIPEMENTS_FOURNITURES_REPARATION"));
            var budgetDTO3 = new BudgetDTO()
            {
                id = result["10"][0].id,
                budget = result["10"][0].budget,
                sous_classification = result["10"][0].sous_classification,
                id_sous_classification = result["10"][0].id_sous_classification
            };
            Assert.AreEqual(budgetDTO2.id, budgetDTO3.id);
            Assert.AreEqual(budgetDTO2.id_sous_classification, budgetDTO3.id_sous_classification);
            Assert.AreEqual(budgetDTO2.sous_classification, budgetDTO3.sous_classification);
            Assert.AreEqual(double.Parse(budgetDTO2.budget), double.Parse(budgetDTO3.budget.Replace('.', ',')));
        }
        [TestMethod]
        
        public void MoneyConverter()
        {
            string test1 = new BudgetDAO(null).toMoneyEuros(300);
            string test2 = new BudgetDAO(null).toMoneyEuros(3000);
            string test3 = new BudgetDAO(null).toMoneyEuros(30000);
            string test4 = new BudgetDAO(null).toMoneyEuros(300000);
            string test5 = new BudgetDAO(null).toMoneyEuros(3000000);
            string test6 = new BudgetDAO(null).toMoneyEuros(30000000);
            string test7 = new BudgetDAO(null).toMoneyEuros(30000000.0);
            string test8 = new BudgetDAO(null).toMoneyEuros(30000000.1);
            string test9 = new BudgetDAO(null).toMoneyEuros(30000000.23);
            string test10 = new BudgetDAO(null).toMoneyEuros(30000000.02);
            string test11 = new BudgetDAO(null).toMoneyEuros(0.021457);
            string test12 = new BudgetDAO(null).toMoneyEuros(000.021457);
            string test13 = new BudgetDAO(null).toMoneyEuros(0000000.021457);
            string test14 = new BudgetDAO(null).toMoneyEuros(0);
            string test15 = new BudgetDAO(null).toMoneyEuros(0.0);
            string test16 = new BudgetDAO(null).toMoneyEuros(-30000000.23);
            string test17 = new BudgetDAO(null).toMoneyEuros(-300000000.23);
            string test18 = new BudgetDAO(null).toMoneyEuros(-3000000000.23);
            Assert.AreEqual("300&nbsp€", test1);
            Assert.AreEqual("3&nbsp000&nbsp€", test2);
            Assert.AreEqual("30&nbsp000&nbsp€", test3);
            Assert.AreEqual("300&nbsp000&nbsp€", test4);
            Assert.AreEqual("3&nbsp000&nbsp000&nbsp€", test5);
            Assert.AreEqual("30&nbsp000&nbsp000&nbsp€", test6);
            Assert.AreEqual("30&nbsp000&nbsp000&nbsp€", test7);
            Assert.AreEqual("30&nbsp000&nbsp000,1&nbsp€", test8);
            Assert.AreEqual("30&nbsp000&nbsp000,23&nbsp€", test9);
            Assert.AreEqual("30&nbsp000&nbsp000,02&nbsp€", test10);
            Assert.AreEqual("0,021457&nbsp€", test11);
            Assert.AreEqual("0,021457&nbsp€", test12);
            Assert.AreEqual("0,021457&nbsp€", test13);
            Assert.AreEqual("0&nbsp€", test14);
            Assert.AreEqual("0&nbsp€", test15);
            Assert.AreEqual("-30&nbsp000&nbsp000,23&nbsp€", test16);
            Assert.AreEqual("-300&nbsp000&nbsp000,23&nbsp€", test17);
            Assert.AreEqual("-3&nbsp000&nbsp000&nbsp000,23&nbsp€", test18);
        }
    }
}

