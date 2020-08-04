using SalesTax.Classes.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTax.Classes.Factories
{
    // abstract factory
    public abstract class ProductFactory
    {
        public abstract Product CreateProduct(string name, decimal price, int qty, bool isImported);
    }
}
