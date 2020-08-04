using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTax
{
    class Appsettings
    {
    }

    public class TaxSettings
    {
        public decimal ImportTaxrate { get; set; }
        public decimal SalesTaxrate { get; set; }
    }
}
