using System;

namespace ProjectManagement.Web.Models
{
    public class Project
    {
        public int? Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Status { get; set; }
        public int Department { get; set; }
        public string Sectors { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public bool Detailed { get; set; }
        public string Description { get; set; }
        public string ProjectManager { get; set; }
        public int CountyId { get; set; }
        public int PlanningAuthorityId { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }        
    }
}