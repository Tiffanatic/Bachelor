﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RapidTime.Frontend.Models
{
    public class CreateCustomerResource
    {
        public string Name { get; set; }
        public int CVRNumber { get; set; }
        public int CompanyTypeId { get; set; }
        public DateTime YearlyReview { get; set; }
        public InvoiceCurrencyEnum InvoiceCurrency { get; set; }
        public string InvoiceEmail { get; set; }

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
