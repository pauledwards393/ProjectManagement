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
        public static IEnumerable<Models.PlanningAuthority> GetPlanningAuthorities()
        {
			var sql = "SELECT CountyId, Id, Name FROM PlanningAuthority ORDER BY Name";

			return GetPlanningAuthorities(sql);
		}

		public static IEnumerable<Models.PlanningAuthority> GetPlanningAuthoritiesByCounty(int countyId)
		{
			var sql = "SELECT CountyId, Id, Name FROM PlanningAuthority WHERE CountyId = @CountyId ORDER BY Name";
			var param = new SqlParameter("@CountyId", countyId);

			return GetPlanningAuthorities(sql, param);
		}

		private static IEnumerable<Models.PlanningAuthority> GetPlanningAuthorities(string sql, SqlParameter param = null)
		{
			var PlanningAuthorities = new List<Models.PlanningAuthority>();

			string connectionString = ConfigurationManager.ConnectionStrings["MBProjectConnectionString"].ConnectionString;

			using (SqlConnection conn = new SqlConnection(connectionString))
			{
				SqlCommand cmd = new SqlCommand(sql, conn);

				if (param != null)
				{
					cmd.Parameters.Add(param);
				}
				
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