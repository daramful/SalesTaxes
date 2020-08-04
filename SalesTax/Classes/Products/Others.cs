using SalesTax.Classes.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTax.Classes.Products
{
    // concrete Product: Book, Food, Medical, Others
    public class Others : Product
    {
        public Others(string name, decimal price, bool isImported, int qty)
            : base(name, price, isImported, qty)
        {
        }

        public override ProductFactory GetFactory()
        {
            return new OthersFactory();
        }
        public override decimal CalculateTax(TaxSettings _taxSettings)
        {
            decimal importedTax = 0;
            if (IsImported == true)
            {
                importedTax = Convert.ToDecimal(Price) * _taxSettings.ImportTaxrate;
            }
            var salesTax = Convert.ToDecimal(Price) * _taxSettings.SalesTaxrate;
            var eachtax = Utility.RoundOffToNearestNickle(importedTax + salesTax);
            PriceOneItemAfterTax = Price + eachtax;

            var totaltax = Qty * eachtax;
            return totaltax;
        }
    }
}
