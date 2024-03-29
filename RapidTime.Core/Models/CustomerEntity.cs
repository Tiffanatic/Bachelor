﻿using System;
using System.Collections.Generic;
using RapidTime.Core.Models.Address;

namespace RapidTime.Core.Models
{
    public class CustomerEntity : BaseEntity
    {
        public string Name { get; set; }
        public int CvrNumber { get; set; }
        public int CompanyTypeId { get; set; }
        public CompanyTypeEntity CompanyTypeEntity { get; set; }
        public DateTime YearlyReview { get; set; }
        public InvoiceCurrencyEnum InvoiceCurrency { get; set; }
        public string InvoiceMail { get; set; }
        public IList<CustomerContact> CustomerContacts { get; set; }
        public IList<AssignmentEntity> Assignments { get; set; }
        public IList<AddressAggregateEntity> AddressAggregates { get; set; }

        public enum InvoiceCurrencyEnum
        {
            Dkk = 0,
            Sek = 1,
            Nok = 2,
            Gbp = 3,
            Eur = 4
        }
    }
    
}