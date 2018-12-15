using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ProjectManagement.Web.Providers
{
    public class ClientTypeProvider
    {
        public IEnumerable<Models.ClientType> GetClientTypes()
        {
            var clientTypes = new List<Models.ClientType>();

            string sql = "SELECT Id, Name FROM ClientType";
            string connectionString = ConfigurationManager.ConnectionStrings["MBProjectConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);

                try
                {
                    conn.Open();
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        clientTypes.Add(Models.ClientType.Create(reader));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return clientTypes;
        }
    }
}