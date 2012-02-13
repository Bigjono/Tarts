using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tarts.Web.Models.Payments
{
    public class PaymentResultViewModel
    {
        public bool Success { get; set; }
        public string Reference { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }

    }
}