using System;
using System.Data;

namespace ProjectManagement.Web.Models
{
    public class ClientType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static ClientType Create(IDataRecord record)
        {
            return new ClientType
            {
                Id = Convert.ToInt32(record["Id"]),
                Name = record["Name"].ToString()
            };
        }
    }
}