using System;
using System.Collections.Generic;
using RapidTime.Core.Models.Address;

namespace RapidTime.Core.Models
{
    public class CustomerEntity : BaseEntity
    {
        public string Name { get; set; }
        public int? CVRNumber { get; set; }
        public AddressAggregateEntity Address { get; set; }
        public int CompanyTypeId { get; set; }
        public CompanyTypeEntity CompanyTypeEntity { get; set; }
        public DateTime YearlyReview { get; set; }
        public InvoiceCurrencyEnum InvoiceCurrency { get; set; }
        public string InvoiceMail { get; set; }
        public IList<CustomerContact> CustomerContacts { get; set; }
        public IList<AssignmentEntity> Assignments { get; set; }

        public enum InvoiceCurrencyEnum
        {
            DKK = 0,
            SEK = 1,
            NOK = 2,
            GBP = 3,
            EUR = 4
        }
    }
    
}