using SalesTax.Classes.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTax.Classes.Products
{
    // abstract product: Product
    public abstract class Product
    {
        public string Name { get; set; }

        public bool IsImported { get; set; }

        public decimal Price { get; set; }
        public decimal PriceOneItemAfterTax { get; set; }
        public int Qty { get; set; }

        public int Type { get; }


        public Product(string name, decimal price, bool isImported, int qty)
        {
            Name = name;
            IsImported = isImported;
            Price = price;
            Qty = qty;
        }

        public abstract ProductFactory GetFactory();
        public virtual decimal CalculateTax(TaxSettings _taxSettings)
        {
            decimal tax = 0;
            if (IsImported == true)
            {
                tax = Convert.ToDecimal(Price) * _taxSettings.ImportTaxrate;
            }

            var eachtax = Utility.RoundOffToNearestNickle(tax);
            PriceOneItemAfterTax = Price + eachtax;

            var totaltax = Qty * eachtax;
            return totaltax;
        }
    }
}
