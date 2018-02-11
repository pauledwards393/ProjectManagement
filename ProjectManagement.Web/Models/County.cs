using System;
using System.Data;

namespace ProjectManagement.Web.Models
{
    public class County
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static County Create(IDataRecord record)
        {
            return new County
            {
                Id = Convert.ToInt32(record["Id"]),
                Name = record["Name"].ToString()
            };
        }
    }
}