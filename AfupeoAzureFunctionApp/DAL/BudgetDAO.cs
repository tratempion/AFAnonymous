using System;
using System.Collections.Generic;
using System.Text;
using DTO;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using RestSharp;
using System.Globalization;

namespace DAL
{
    public class BudgetDAO
    {
        internal class tmpClassBudget
        {
            public string id_budget;
            public string budget;
            public string mois_valeur;
            public string id_sous_classification;
            public string sous_classification;
        }
        internal class tmpClassClassification
        {
            public string classif { get; set; }
            public List<tmpClassSousCLassification> sousCLassifications { get; set; }
        }
        internal class ObjectString
        {
            public ObjectString(string name)
            {
                this.name = name;
            }

            public string name { get; set; }
            public override string ToString()
            {
                return name;
            }
            public override bool Equals(object obj)
            {
                if (((ObjectString)obj).name.Equals(this.name)) return true;
                return false;
            }
            public override int GetHashCode()
            {
                return name.GetHashCode();
            }
        }
        internal class tmpClassSousCLassification
        {
            public string objectif { get; set; }
            public List<tmpClassDecaissement> decaissements { get; set; }

            public override bool Equals(object obj)
            {
                try
                {
                    var objet = (tmpClassSousCLassification)obj;
                    if (objet.GetHashCode() == this.GetHashCode()) return true;

                }
                catch (Exception)
                {
                    return false;
                }
                return false;
            }
            public override int GetHashCode()
            {
                return objectif.GetHashCode();
            }

        }
        internal class tmpClassDecaissement
        {
            public string operation { get; set; }
            public string montant_ttc { get; set; }

        }
        internal class tmpDataSet
        {
            public string classif { get; set; }
            public string sous_classif { get; set; }
            public string objectif { get; set; }
            public string operation { get; set; }
            public string montant_ttc { get; set; }
        }
        private ISqlManager sqlManager;
        public BudgetDAO(ISqlManager sqlManager)
        {
            this.sqlManager = sqlManager;
        }

        public string DeleteBudget(Guid id)
        {
            Dictionary<string, object> dictionnaire = new Dictionary<string, object>();
            dictionnaire.Add("id", id.ToString());

            return sqlManager.ExecProcedure("DeleteBudget", dictionnaire);

        }
        public string EditBudget(BudgetDTO budgetDTO)
        {
            Dictionary<string, object> dictionnaire = new Dictionary<string, object>();
            dictionnaire.Add("id", budgetDTO.id.ToString());
            dictionnaire.Add("budget", budgetDTO.budget.ToString());
            dictionnaire.Add("id_sous_classification", budgetDTO.id_sous_classification.ToString());
            dictionnaire.Add("mois_valeur", budgetDTO.periode.Month.ToString());
            dictionnaire.Add("annee", budgetDTO.periode.Year.ToString());
            return sqlManager.ExecProcedure("EditBudget", dictionnaire);
        }

        public string GetBudgetBilan(string annee)
        {
            Dictionary<string, object> dictionnaire = new Dictionary<string, object>();
            dictionnaire.Add("annee", annee.ToString());
            string retour = sqlManager.ExecProcedure("GetObjectifYear", dictionnaire);
            var resultSQL = JsonConvert.DeserializeObject<List<tmpDataSet>>(retour);
            var result = new Dictionary<string, Dictionary<string, tmpClassSousCLassification>>();
            foreach (var item in resultSQL)
            {
                if (!result.ContainsKey(item.classif))
                {
                    result.Add(item.classif, new Dictionary<string, tmpClassSousCLassification>());

                }

                if (!result[item.classif].ContainsKey(item.sous_classif))
                {
                    var tmpClassSousCLassification = new tmpClassSousCLassification()
                    {
                        decaissements = new List<tmpClassDecaissement>(),
                        objectif = item.objectif == null ? "0" : item.objectif
                    };
                    result[item.classif].Add(item.sous_classif, tmpClassSousCLassification);

                }
                if (item.montant_ttc != null || item.operation != null)
                {

                    result[item.classif][item.sous_classif].decaissements.Add(new tmpClassDecaissement()
                    {
                        montant_ttc = item.montant_ttc,
                        operation = item.operation
                    });
                }
            }
            return JsonConvert.SerializeObject(result);
        }

        public string GetBudget(string annee, string classification)
        {
            Dictionary<string, object> dictionnaire = new Dictionary<string, object>();
            dictionnaire.Add("year", annee);
            dictionnaire.Add("classification", classification);
            List<tmpClassBudget> retour = new List<tmpClassBudget>();
            retour = JsonConvert.DeserializeObject<List<tmpClassBudget>>(
                sqlManager.ExecProcedure("GetBudget", dictionnaire));

            Dictionary<string, List<DTO.BudgetDTO>> result = new Dictionary<string, List<DTO.BudgetDTO>>();
            for (int i = 0; i < 12; i++)
            {
                result.Add(i.ToString(), new List<DTO.BudgetDTO>());
            }
            foreach (var item in retour)
            {

                result[((int.Parse(item.mois_valeur) - 1)).ToString()].Add(new DTO.BudgetDTO
                {
                    id = item.id_budget,
                    budget = item.budget,
                    id_sous_classification = item.id_sous_classification,
                    sous_classification = item.sous_classification
                });
            }

            return JsonConvert.SerializeObject(result);

        }

        public string SaveBudget(BudgetDTO budgetDTO)
        {
            Dictionary<string, object> dictionnaire = new Dictionary<string, object>();

            dictionnaire.Add("budget", budgetDTO.budget);
            dictionnaire.Add("id_sous_classification", budgetDTO.id_sous_classification);
            dictionnaire.Add("mois_valeur", budgetDTO.periode.Month);
            dictionnaire.Add("annee", budgetDTO.periode.Year);
            if (dictionnaire["budget"] != null) dictionnaire["budget"] = ((string)budgetDTO.budget).Split(',')[0].Replace(" ", "").Replace("€", "").Replace("-", "").Trim();
            return sqlManager.ExecProcedure("SaveBudget", dictionnaire);
        }
        public string toMoneyEuros(double amount)
        {
            string unformatedAmount = amount.ToString();
            string[] splited;
            if (unformatedAmount.Contains(','))
            {
                splited = unformatedAmount.Split(',');

            }else
            {
                splited = unformatedAmount.Split('.');
            }
            var money = splited[0];
            var toReverse = money.ToCharArray();
            Array.Reverse(toReverse);
            money = new String(toReverse);
            string centimes ="";
            if (splited.Length > 1)
            {
                centimes ="."+ splited[1];
            }
            string subMoney ="";
            int spaceCount = 0;

            if (money.Length > 0)
            {
                subMoney+=money[0];
            }
            else
            {
                subMoney+='0';
            }
            for (int i = 1;i<money.Length;i++)
            {
                if (i % 3 == 0 && money[i] !='-')
                {
                    subMoney+= ",";
                }
                subMoney += money[i];

            }
            toReverse = subMoney.ToCharArray();
            Array.Reverse(toReverse);
            money = new String(toReverse);
            money=money.Replace(" ", "&nbsp");

            return money+centimes+"&nbsp€";
        }
        private string GetHtml(string annee)
        {
            var budgetBilan = new Dictionary<string, Dictionary<string, tmpClassSousCLassification>>();
            budgetBilan = JsonConvert.DeserializeObject<
                Dictionary<string, Dictionary<string, tmpClassSousCLassification>>>(
                    this.GetBudgetBilan(annee)
                );

            string html = @"<!DOCTYPE html>
            <html>

            <head>
                <title > Rapport </title >
                <meta charset = 'UTF-8'>
 
                    <script src = 'https://code.jquery.com/jquery-3.4.1.min.js'
                    integrity = 'sha256-CSXorXvZcTkaix6Yvo6HppcZGetbYMGWSFlBw8HfCJo=' crossorigin = 'anonymous' ></script >
  
                    <link href = 'https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css' rel = 'stylesheet'
                    type = 'text/css'>
                <script src = 'https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js'></script >
 

                </head >
 

                <body >
                <div class='container'>

                    <img width = '200px' height = '200px' src = 'data:image/png;base64," + DAL.ExpaceoLogoBase64.Value + @"' alt='Red dot'/>
                                      <h1 > Rapport de suivie d'execution budgétaire "+annee+@"</h1>

                    <br>
                    <br>

                    <div class='container'>
                        <div class='col-md-6'>
		                    <h2>Bilan</h2>
                    		tableauIci
		                    
                    	</div>
                    </div>
                    <br>
                    <br>
                    <br>
                    <h3>Details</h3>
                    <table class='table'>
                        <thead>
                            <tr>
                                <th scope = 'col' > Classification </ td >
                                <th scope='col'>Objectif</td>
                                <th scope = 'col' > Montant </ td >
                                <th scope='col'>Balance</td>
                            </tr>
                        </thead>
                        <tbody>";
            double totalDecaissement = 0;
            double totalObjectif = 0;
            foreach (var classif in budgetBilan)
            {
                double totalDecaissementClassif = 0;
                double totalObjectifClassif = 0;
                foreach (var sousClassif in classif.Value)
                {
                    totalObjectifClassif += double.Parse(sousClassif.Value.objectif);
                    foreach (var decaissement in sousClassif.Value.decaissements)
                    {

                        totalDecaissementClassif += double.Parse(decaissement.montant_ttc);
                    }

                }
                totalDecaissement += totalDecaissementClassif;
                totalObjectif += totalObjectifClassif;
                if (totalDecaissementClassif > totalObjectifClassif)
                {
                    html += @"<tr class='red classif'>";

                }
                else
                {
                    html += @"<tr class='green classif'>";

                }
                html += "<td style=\"font-size: 11pt\">" + classif.Key + "</td>";
                html += "<td style=\"font-size: 11pt\">" + toMoneyEuros(totalObjectifClassif) + " </td>";
                html += "<td style=\"font-size: 11pt\">" + toMoneyEuros(totalDecaissementClassif) + "</td>";
                html += "<td style=\"font-size: 11pt\">" + toMoneyEuros((totalObjectifClassif - totalDecaissementClassif)) + "</td>";
                html += "</tr>";


                foreach (var sousClassif in classif.Value)
                {
                    totalDecaissementClassif = 0;
                    totalObjectifClassif = double.Parse(sousClassif.Value.objectif);
                    foreach (var decaissement in sousClassif.Value.decaissements)
                    {
                        totalDecaissementClassif += double.Parse(decaissement.montant_ttc);
                    }
                    if (totalDecaissementClassif > totalObjectifClassif)
                    {
                        html += "<tr class='sousclass softred'>";
                    }
                    else
                    {
                        html += "<tr class='sousclass softgreen'>";

                    }
                    html += "<td>&nbsp;&nbsp;&nbsp;&nbsp;" + sousClassif.Key + "</td>";
                    html += "<td>" + toMoneyEuros(totalObjectifClassif) + "</td>";
                    html += "<td>" + toMoneyEuros(totalDecaissementClassif) + "</td>";
                    html += "<td>" + toMoneyEuros((totalObjectifClassif - totalDecaissementClassif)) + "</td>";
                    html += "</tr>";

                }
            }
            string stringTableau = @"<table class='table'>
                            <thead>

                                <tr>

                                    <th> Objectif </th >

                                    <th> Montant </th >

                                    <th> Balance </th >

                                </tr>

                            </thead>

                            <tbody>

                                <tr>

                                    <td> " + toMoneyEuros(totalObjectif) + @" </td>


                                    <td> " + toMoneyEuros(totalDecaissement) + @" </td>


                                    <td> " + toMoneyEuros((totalObjectif - totalDecaissement)) + @" </td>


                                </tr>


                            </tbody>


                        </table> ";
            html=html.Replace("tableauIci", stringTableau);

            html += @" </tbody>
                    
                        </table>
                    </div>
        
                </div>


            </body>

            </html>
            <style>
            .classif{
                font-weight:bold;
                font-size: 12pt;
            }
            .red{
                color: red;
            }
            .softred{
                color:crimson
            }
            .softgreen{
            }
            </style>";



            return html;
        }
        public async Task<string> GeneratePDF(string annee)
        {
            var url = "https://eunorth.rotativahq.com";
            var client = new RestClient(url);

            client.AddDefaultHeader("X-ApiKey", "606da7c5a970409196258c4e5133d9a2");
            client.AddDefaultHeader("Accept", " application/json, text/plain, */*");
            RestRequest req = new RestRequest(Method.POST);
            req.AddJsonBody(new
            {
                html = this.GetHtml(annee)
            }); ;

            var response = client.Execute(req);

            return (response.ResponseUri.ToString());


        }
    }
}
