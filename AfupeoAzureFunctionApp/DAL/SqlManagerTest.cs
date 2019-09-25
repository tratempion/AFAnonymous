using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Net;
using System.Data;
using System.Data.SqlTypes;

namespace DAL
{

    public class SqlManagerTest : ISqlManager
    {
        public IEnumerable<Dictionary<string, object>> Serialize(SqlDataReader reader)
        {
            var results = new List<Dictionary<string, object>>();

            var cols = new List<string>();

            for (var i = 0; i < reader.FieldCount; i++)
            {
                cols.Add(reader.GetName(i));
            }

            while (reader.Read())
            {
                results.Add(SerializeRow(cols, reader));
            }

            return results;
        }


        private Dictionary<string, object> SerializeRow(IEnumerable<string> cols, SqlDataReader reader)
        {
            var result = new Dictionary<string, object>();

            foreach (var col in cols)
            {
                result.Add(col, reader[col]);
            }

            return result;
        }
        public string ExecQuery(string text, Dictionary<string, Object> parameters = null)
        {
            SqlConnectionStringBuilder builder;
            builder = new SqlConnectionStringBuilder();
            builder.DataSource = "afupeo.database.windows.net";
            builder.UserID = "afupeo_admin";
            builder.Password = "Expaceo+2019";
            builder.InitialCatalog = "afupeo-test";
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(text))
                {


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        var r = Serialize(reader);
                        return JsonConvert.SerializeObject(r, Formatting.Indented);
                    }
                }

            }
        }
        public string ExecProcedure(string procedure, Dictionary<string, Object> parameters = null)
        {

            SqlConnectionStringBuilder builder;
            builder = new SqlConnectionStringBuilder();
            builder.DataSource = "afupeo.database.windows.net";
            builder.UserID = "afupeo_admin";
            builder.Password = "Expaceo+2019";
            builder.InitialCatalog = "afupeoSQL-dev";
            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(procedure, connection) { CommandType = System.Data.CommandType.StoredProcedure })
                {
                    if (parameters != null)
                    {

                        foreach (var item in parameters)
                        {
                            command.Parameters.AddWithValue(item.Key, string.IsNullOrEmpty((item.Value ?? "").ToString()) ? (object)DBNull.Value : (item.Value ?? "").ToString());

                        }
                    }

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        var r = Serialize(reader);
                        return JsonConvert.SerializeObject(r, Formatting.Indented);
                    }
                }

            }
        }
    }
}
