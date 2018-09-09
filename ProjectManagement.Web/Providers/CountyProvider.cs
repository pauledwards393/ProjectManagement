using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProjectManagement.Web.Providers
{
    public class CountyProvider
    {
        public IEnumerable<Models.County> GetCounties()
        {
            var counties = new List<Models.County>();
       
            string sql = "SELECT Id, Name FROM County";
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
                        counties.Add(Models.County.Create(reader));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return counties;
        }
    }
}