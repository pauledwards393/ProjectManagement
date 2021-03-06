﻿using System;
using System.Collections.Generic;

namespace ProjectManagement.Web.Models
{
    public class Project
    {
        public string Code { get; set; }
        public string Contact { get; set; }
        public Address ClientAddress { get; set; }
        public int ClientTypeId { get; set; }
        public int? CountyId { get; set; }
        public int Department { get; set; }
        public string Description { get; set; }
        public bool Detailed { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Id { get; set; }
        public string Introducer { get; set; }
        public Address InvoiceAddress { get; set; }
        public string InvoiceContact { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Name { get; set; }
        public int? PlanningAuthorityId { get; set; }
        public string ProjectManager { get; set; }
        public List<int> Sectors { get; set; }
        public DateTime? StartDate { get; set; }
        public int Status { get; set; }
        public string ProjectCity { get; set; }
    }
}