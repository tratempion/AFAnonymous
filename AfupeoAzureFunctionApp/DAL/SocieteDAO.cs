using DTO;
namespace DAL
{
    public  class SocieteDAO:CRUD
    {
        private ISqlManager sqlManager;

        public SocieteDAO(ISqlManager sqlManager)
        {
            this.sqlManager = sqlManager;
        }
        public string GetAllSociete()
        {
            return sqlManager.ExecProcedure("GetAllSociete");
        }


        public void Create(string data)
        {
            var societeDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<SocieteDTO>(data);
            sqlManager.ExecQuery("insert into societe (name) values ('" + societeDTO.name + "');");
        }

        public void Delete(string data)
        {
            var societeDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<SocieteDTO>(data);
            if (societeDTO.id == null || societeDTO.id.ToString() == "")
                throw new System.Exception("id null");
            sqlManager.ExecQuery("delete from societe where id = '"+societeDTO.id+"';");
        }

        public string Read()
        {
            return sqlManager.ExecQuery("select * from societe;");

        }

        public void Update(string data)
        {
            var societeDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<SocieteDTO>(data);
            if (societeDTO.id == null || societeDTO.id.ToString() == "")
                throw new System.Exception("id null");
            sqlManager.ExecQuery("update societe set name = '"+societeDTO.name+"' where id = '"+societeDTO.id+"';");

        }
    }
}