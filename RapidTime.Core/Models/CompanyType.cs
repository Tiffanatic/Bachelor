using System.Collections.Generic;

namespace RapidTime.Core.Models
{
    public class CompanyType : BaseEntity
    {
        public string CompanyTypeName { get; set; }
        public IList<Customer> Customers { get; set; }
    }
}