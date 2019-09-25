using DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class SousClassificationDao : CRUD
    {

        public ISqlManager sqlManager { get; set; }
        public SousClassificationDao(ISqlManager sqlManager)
        {
            this.sqlManager = sqlManager;
        }
        public void Create(string data)
        {
            var sousClassificationDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<SousClassificationDTO>(data);
            if (sousClassificationDTO.id_classification.ToString().Contains("0000"))
                throw new Exception("id_classification null");
            sqlManager.ExecQuery(@"insert into sous_classification  (name,id_classification)
                                values ('" + sousClassificationDTO.name + "'" +
                                ",'"+sousClassificationDTO.id_classification+"');");
        }

        public void Delete(string data)
        {
            var sousClassificationDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<SousClassificationDTO>(data);

            sqlManager.ExecQuery(@"delete from sous_classification  where id='" +
                sousClassificationDTO.id + "';");
        }

        public string Read()
        {
            return sqlManager.ExecQuery(@"select * from sous_classification");

        }

        public void Update(string data)
        {
            var sousClassificationDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<SousClassificationDTO>(data);
            if (sousClassificationDTO.id_classification.ToString().Contains("0000"))
                throw new Exception("id_classification null");
            sqlManager.ExecQuery(@"update sous_classification set name='" +
                sousClassificationDTO.name + "'," +
                "id_classification='"+sousClassificationDTO.id_classification + "'" +
                " where id='" + sousClassificationDTO.id + "';");

        }
    }
}
