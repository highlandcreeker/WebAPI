using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FBPortal.API.Models.DTOs
{
    public class Invoice
    {
        public Guid InvoiceId { get; set; }
        public string Name { get; set; }
        public string Vendor { get; set; }
        public decimal AmountPaid { get; set; }

    }
}