using System;
using System.Data;

namespace ProjectManagement.Web.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static Department Create(IDataRecord record)
        {
            return new Department
            {
                Id = Convert.ToInt32(record["Id"]),
                Name = record["Name"].ToString()
            };
        }
    }
}