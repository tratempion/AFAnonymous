using DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class BanqueDAO:CRUD
    {
        public class tmpClassAccount
        {

            public string compte;
            public string banque;

        }
        ISqlManager sqlManager;

        public BanqueDAO(ISqlManager sqlManager)
        {
            this.sqlManager = sqlManager;
        }

        public string GetAccountType()
        {
            string retourSQL = sqlManager.ExecProcedure("getAccountType");
            List<tmpClassAccount> retour = JsonConvert.DeserializeObject<List<tmpClassAccount>>(retourSQL);

            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();

            foreach (var item in retour)
            {
                if (!dict.ContainsKey(item.banque))
                {
                    dict.Add(
                        item.banque,
                        new List<string>());

                }
                dict[item.banque].Add(item.compte);


            }
            return JsonConvert.SerializeObject(dict);
        }

        public void Create(string data)
        {
            var banqueDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<BanqueDTO>(data);
            string defaultAccount = !banqueDTO.default_account.ToString().Contains("0000")
                ? banqueDTO.default_account.ToString():null;
            if (defaultAccount != null)
            {
            sqlManager.ExecQuery(@"insert into banque (name,default_account)
                                values ('" + banqueDTO.name + "','" +
                                defaultAccount + "');");

            }
            else
            {
                sqlManager.ExecQuery(@"insert into banque (name)
                                values ('" + banqueDTO.name + "');");

            }
        }

        public void Delete(string data)
        {
            var banqueDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<BanqueDTO>(data);
            if (banqueDTO.id == null || banqueDTO.id.ToString() == "")
                throw new System.Exception("id null");
            sqlManager.ExecQuery("delete from banque where id = '" + banqueDTO.id + "';");
        }

        public string Read()
        {
            return sqlManager.ExecQuery("select * from banque;");

        }

        public void Update(string data)
        {
            var banqueDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<BanqueDTO>(data);
            if (banqueDTO.id == null || banqueDTO.id.ToString() == "")
                throw new System.Exception("id null");
            if (banqueDTO.default_account.ToString().Contains("0000"))
            {
                sqlManager.ExecQuery("update banque set name = '"
                    + banqueDTO.name  +
                    "' where id = '" + banqueDTO.id + "';");
            }
            else
            {

                sqlManager.ExecQuery("update banque set name = '"
                    + banqueDTO.name + "'," +
                    "default_account='" + banqueDTO.default_account + "'" +
                    " where id = '" + banqueDTO.id + "';");
            }


        }
    }
}
