﻿using System;
using System.Collections.Generic;
using RapidTime.Core.Models.Address;

namespace RapidTime.Core.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AddressAggregate Address { get; set; }
        public CompanyType CompanyType { get; set; }
        public DateTime YearlyReview { get; set; }
        public InvoiceCurrencyEnum InvoiceCurrency { get; set; }
        public string InvoiceMail { get; set; }
        public List<Contact> Contacts { get; set; }
        
        public enum InvoiceCurrencyEnum
        {
            DKK = 1,
            SEK = 2,
            NOK = 3,
            GBP = 4,
            EUR = 5
        }
    }
    
}