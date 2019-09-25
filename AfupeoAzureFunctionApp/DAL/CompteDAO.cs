using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using DTO;
namespace DAL
{
    public class CompteDAO:CRUD
    {
        ISqlManager sqlManager;

        public CompteDAO(ISqlManager sqlManager)
        {
            this.sqlManager = sqlManager;
        }
        internal class tmpClassPosition
        {
            public string position { get; set; }
            public string date { get; set; }
        }
        internal class tmpClassCompte
        {
            public string date { get; set; }
            public string compte { get; set; }
            public string position { get; set; }
        }
        internal class TmpClassBanque
        {
            public string banque { get; set; }
            public string compte { get; set; }
            public string position { get; set; }

            public override bool Equals(object obj)
            {
                try
                {
                    if (((TmpClassBanque)obj).banque == this.banque) return true;
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(banque, compte, position);
            }
        }
        public string GetAllAccountPositionAtDate(string _date)
        {
            Dictionary<string, object> dictionnaire = new Dictionary<string, object>();
            var date = DateTime.Parse(_date, new System.Globalization.CultureInfo("fr-FR"));
            dictionnaire.Add("date", date.ToString("dd/MM/yyyy"));
            return sqlManager.ExecProcedure("GetAllAccountsPositionAtDate", dictionnaire);
        }

        public string GetTotalPositionAtInterval(string date_debut, string date_fin, string mode, string compte=null)
        {

            Dictionary<string, object> dictionnaire = new Dictionary<string, object>();
            var date1 = DateTime.Parse(date_debut, new System.Globalization.CultureInfo("fr-FR"));
            var date2 = DateTime.Parse(date_fin, new System.Globalization.CultureInfo("fr-FR"));
            if (date1 > date2) throw new Exception("Erreur : date_debut doit etre superieur à date_fin");
            var retour = new List<tmpClassPosition>();
            if (compte == null)
            {

                if (mode == "annee")
                {
                    for (int i = date1.Year; i <= date2.Year; i++)
                    {
                        var datetime = DateTime.Parse("01/01/" + (i + 1));
                        datetime = datetime.AddDays(-1);
                        string dateString = datetime.Day.ToString() + "/" + datetime.Month.ToString() + "/" + datetime.Year.ToString();
                        dictionnaire = new Dictionary<string, object>();
                        dictionnaire.Add("date", dateString);
                        string retourSQL = sqlManager.ExecProcedure("GetTotalPositionAtDate", dictionnaire);
                        retour.Add(JsonConvert.DeserializeObject<List<tmpClassPosition>>(retourSQL)[0]);

                    }

                }
                else if (mode == "mois")
                {
                    date1 = DateTime.Parse("01/" + (date1.Month + 1) + "/" + date1.Year, new System.Globalization.CultureInfo("fr-FR"));
                    date1 = date1.AddDays(-1);
                    while (date1 <= date2)
                    {

                        dictionnaire = new Dictionary<string, object>();
                        dictionnaire.Add("date", date1.ToString("dd/MM/yyyy"));
                        string retourSQL = sqlManager.ExecProcedure("GetTotalPositionAtDate", dictionnaire);
                        retour.Add(JsonConvert.DeserializeObject<List<tmpClassPosition>>(retourSQL)[0]);

                        date1 = date1.AddMonths(2);
                        date1 = DateTime.Parse("01/" + (date1.Month) + "/" + date1.Year, new System.Globalization.CultureInfo("fr-FR"));
                        date1 = date1.AddDays(-1);
                    }

                }
                else if (mode == "jours")
                {
                    while (date1 <= date2)
                    {

                        dictionnaire = new Dictionary<string, object>();
                        dictionnaire.Add("date", date1.ToString("dd/MM/yyyy"));
                        string retourSQL = sqlManager.ExecProcedure("GetTotalPositionAtDate", dictionnaire);
                        retour.Add(JsonConvert.DeserializeObject<List<tmpClassPosition>>(retourSQL)[0]);

                        date1 = date1.AddDays(1);
                    }
                }
                else
                {
                    throw new Exception("Erreur : parametre 'mode' invalide. attendu : 'mois' ou 'annee'");
                }
            }
            else
            {
                if (mode == "mois")
                {
                    date1 = DateTime.Parse("01/" + (date1.Month + 1) + "/" + date1.Year, new System.Globalization.CultureInfo("fr-FR"));
                    date1 = date1.AddDays(-1);
                    while (date1 <= date2)
                    {

                        dictionnaire = new Dictionary<string, object>();
                        dictionnaire.Add("date", date1.ToString("dd/MM/yyyy"));
                        dictionnaire.Add("compte", compte);
                        string retourSQL = sqlManager.ExecProcedure("GetTotalAccountPositionAtDate", dictionnaire);
                        retour.Add(JsonConvert.DeserializeObject<List<tmpClassPosition>>(retourSQL)[0]);

                        date1 = date1.AddMonths(2);
                        date1 = DateTime.Parse("01/" + (date1.Month) + "/" + date1.Year, new System.Globalization.CultureInfo("fr-FR"));
                        date1 = date1.AddDays(-1);
                    }
                }
                else if (mode == "jours")
                {
                    while (date1 <= date2)
                    {

                        dictionnaire = new Dictionary<string, object>();
                        dictionnaire.Add("date", date1.ToString("dd/MM/yyyy"));
                        dictionnaire.Add("compte", compte);
                        string retourSQL = sqlManager.ExecProcedure("GetTotalAccountPositionAtDate", dictionnaire);
                        retour.Add(JsonConvert.DeserializeObject<List<tmpClassPosition>>(retourSQL)[0]);

                        date1 = date1.AddDays(1);
                    }
                }
                else
                {
                    throw new Exception("Erreur : parametre 'mode' invalide. attendu : 'mois' ou 'annee'");
                }

            }

            return JsonConvert.SerializeObject(retour);
        }

        public void Create(string data)
        {
            var compteDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<CompteDTO>(data);
            string banque = !compteDTO.banque.ToString().Contains("0000")
                ? compteDTO.banque.ToString() : null;
            if (banque != null)
            {
                sqlManager.ExecQuery(@"insert into compte (numero,id_banque)
                                values ('" + compteDTO.numero + "','" +
                                    banque + "');");

            }
            else
            {
                sqlManager.ExecQuery(@"insert into compte (numero)
                                values ('" + compteDTO.numero + "');");

            }
        }

        public string Read()
        {
            return sqlManager.ExecQuery("select * from compte;");
        }

        public void Update(string data)
        {
            var compteDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<CompteDTO>(data);
            if (compteDTO.id == null || compteDTO.id.ToString() == "")
                throw new System.Exception("id null");
            if (compteDTO.banque.ToString().Contains("0000"))
            {
                sqlManager.ExecQuery("update compte set numero = '"
                    + compteDTO.numero +
                    "' where id = '" + compteDTO.id + "';");
            }
            else
            {

                sqlManager.ExecQuery("update compte set numero = '"
                    + compteDTO.numero + "'," +
                    "id_banque='" + compteDTO.banque + "'" +
                    " where id = '" + compteDTO.id + "';");
            }
        }

        public void Delete(string data)
        {
            var compteDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<CompteDTO>(data);
            if (compteDTO.id == null || compteDTO.id.ToString() == "")
                throw new System.Exception("id null");
            sqlManager.ExecQuery("delete from compte where id = '" + compteDTO.id + "';");
        }
    }
}
