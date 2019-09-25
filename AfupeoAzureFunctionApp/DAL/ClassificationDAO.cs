using DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace DAL
{
    public class ClassificationDAO:CRUD
    {
        private ISqlManager sqlManager;
        internal class tmpClassClasif
        {

            public string id_classification;
            public string classification;
            public string id_sous_classification;
            public string sous_classification;

        }
        public ClassificationDAO(ISqlManager sqlManager)
        {
            this.sqlManager = sqlManager;
        }

        public string GetClassification()
        {
            List<tmpClassClasif> retour = new List<tmpClassClasif>();

                retour = JsonConvert.DeserializeObject<List<tmpClassClasif>>(
                     sqlManager.ExecProcedure("GetClassification").Replace("\\r\\n", "").Replace("\\", ""));
            Dictionary<ClassificationDTO, List<SousClassificationDTO>> dict =
                               new Dictionary<ClassificationDTO, List<SousClassificationDTO>>();

            foreach (var item in retour)
            {
                if (!dict.ContainsKey(new ClassificationDTO( item.id_classification)))
                {
                    dict.Add(
                        new ClassificationDTO(item.id_classification,
                            item.classification),
                        new List<SousClassificationDTO>());

                }
                dict[new ClassificationDTO(item.id_classification)].Add(
                    new SousClassificationDTO( item.id_sous_classification,
                        item.sous_classification));


            }
            return JsonConvert.SerializeObject(dict);
        }

        public void Create(string data)
        {
            var classificationDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<ClassificationDTO>(data);
             sqlManager.ExecQuery(@"insert into classification  (name)
                                values ('" + classificationDTO.name + "');");

        }

        public string Read()
        {
            return sqlManager.ExecQuery(@"select * from classification ;");
        }

        public void Update(string data)
        {
            var classificationDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<ClassificationDTO>(data);
            sqlManager.ExecQuery(@"update classification set name='" + classificationDTO.name + "'" +
                " where id='" + classificationDTO.id + "';");
        }

        public void Delete(string data)
        {
            var classificationDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<ClassificationDTO>(data);
            sqlManager.ExecQuery(@"delete from  classification where id='" + classificationDTO.id + "';");
        }
    }
}