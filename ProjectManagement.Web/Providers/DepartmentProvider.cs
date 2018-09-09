using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProjectManagement.Web.Providers
{
    public class DepartmentProvider
    {
        public IEnumerable<Models.Department> GetDepartmentsForInsert()
        {
            return GetDepartments(null);
        }

        public IEnumerable<Models.Department> GetDepartmentsForEdit(int projectId)
        {
            var param = projectId > 0 ? projectId : (int?)null;
            return GetDepartments(param);
        }

        private IEnumerable<Models.Department> GetDepartments(int? projectId)
        {
            var departments = new List<Models.Department>();

            string sql = "SELECT Dep_ID AS Id, Name FROM Department " +
                    "WHERE isLegacy = 0 OR (@ProjectId IS NOT NULL AND Dep_ID = (SELECT DepartmentID FROM Project WHERE Project_ID = @ProjectId)) " +
                    "ORDER BY Name";

            string connectionString = ConfigurationManager.ConnectionStrings["MBProjectConnectionString"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@ProjectId", projectId ?? (object)DBNull.Value);

                try
                {
                    conn.Open();
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        departments.Add(Models.Department.Create(reader));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return departments;
        }
    }
}