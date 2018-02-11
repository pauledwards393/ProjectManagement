using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ProjectManagement.Web.Models
{
    [Serializable]
    public class PlanningAuthority
    {
        [JsonIgnore]
        public int CountyId { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public static PlanningAuthority Create(IDataRecord record)
        {
            return new PlanningAuthority
            {
                CountyId = Convert.ToInt32(record["CountyId"]),
                Id = Convert.ToInt32(record["Id"]),
                Name = record["Name"].ToString()
            };
        }
    }
}