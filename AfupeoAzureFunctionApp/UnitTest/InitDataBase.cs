using DAL;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationTest
{
    public static class InitDataBase
    {
        public static ISqlManager GetConnexion()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            string connexionString = config.GetConnectionString("Test");

            ISqlManager SqlManager = new SqlManager(connexionString);
            string sql_structure_reset = System.IO.File.ReadAllText("sql_structure_reset.txt");
            string sql_data_reset = System.IO.File.ReadAllText("sql_data_reset.txt");
            string some_data = System.IO.File.ReadAllText("some_data.txt");
            List<string> batchs = new List<string>();
            batchs.AddRange(sql_structure_reset.Split("GO"));
            batchs.AddRange(sql_data_reset.Split("GO"));
            //batchs.AddRange(some_data.Split("GO"));

            foreach (var batch in batchs)
            {
                try
                {

                    SqlManager.ExecQuery(batch, null);
                }
                catch (Exception e)
                {

                    throw e;
                }
            }
            return SqlManager;
        }
    }
}
