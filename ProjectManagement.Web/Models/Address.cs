using System;

namespace ProjectManagement.Web.Models
{
    public class Address
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string CompanyName { get; set; }
        public string County { get; set; }
        public int? Id { get; set; }
        public string Postcode { get; set; }
        public string TownOrCity { get; set; }
    }
}