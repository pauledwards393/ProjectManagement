using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProjectManagement.Web.Providers
{
    public static class PlanningAuthorityProvider
    {
        public static IEnumerable<Models.PlanningAuthority> GetPlanningAuthoritiesByCounty(int countyId)
        {      
            var PlanningAuthorities = new List<Models.PlanningAuthority>();

            string sql = "SELECT CountyId, Id, Name FROM PlanningAuthority WHERE CountyId = @CountyId";
            string connectionString = ConfigurationManager.ConnectionStrings["MBProjectConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@CountyId", countyId);

                try
                {
                    conn.Open();
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        PlanningAuthorities.Add(Models.PlanningAuthority.Create(reader));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return PlanningAuthorities;
        }
    }
}