using System.Collections.Generic;

namespace RapidTime.Core.Models
{
    public class CompanyTypeEntity : BaseEntity
    {
        public string CompanyTypeName { get; set; }
        public IList<CustomerEntity> Customers { get; set; }
    }
}