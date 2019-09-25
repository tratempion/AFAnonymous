using System;
using DTO;
namespace DAL
{
    public class PaiementMethodDAO:CRUD
    {
        private ISqlManager sqlManager;

        public PaiementMethodDAO(ISqlManager sqlManager)
        {
            this.sqlManager = sqlManager;
        }

        public string GetPayementMethod()
        {
            return sqlManager.ExecProcedure("GetPayementMethod");
        }
        public void Create(string data)
        {
            var paiementMethodDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<PaiementMethodDTO>(data);
            sqlManager.ExecQuery(@"insert into Mode_paiement (name)
                                values ('" + paiementMethodDTO.name + "');");

        }

        public void Delete(string data)
        {
            var paiementMethodDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<PaiementMethodDTO>(data);
            sqlManager.ExecQuery(@"delete from  Mode_paiement where id='"+ paiementMethodDTO.id+ "';");
        }


        public string Read()
        {
            return sqlManager.ExecQuery(@"select * from Mode_paiement ;");
        }

        public void Update(string data)
        {
            var paiementMethodDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<PaiementMethodDTO>(data);
            sqlManager.ExecQuery(@"update Mode_paiement set name='"+paiementMethodDTO.name+"'" +
                " where id='"+paiementMethodDTO.id+"';");
        }
    }
}