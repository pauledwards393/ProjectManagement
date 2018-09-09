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

        public bool IsValid
        {
            get
            {
                return !string.IsNullOrWhiteSpace(AddressLine1) ||
                    !string.IsNullOrWhiteSpace(AddressLine2) ||
                    !string.IsNullOrWhiteSpace(CompanyName) ||
                    !string.IsNullOrWhiteSpace(County) ||
                    !string.IsNullOrWhiteSpace(Postcode) ||
                    !string.IsNullOrWhiteSpace(TownOrCity) ||
                    Id != null;
            }
        }
    }
}