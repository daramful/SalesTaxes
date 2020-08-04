using SalesTax.Classes.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTax.Classes.Factories
{
    // concrete factory
    public class BookFactory : ProductFactory
    {
        public override Product CreateProduct(string name, decimal price, int qty, bool isImported)
        {
            return new Book(name, price, isImported, qty);
        }
    }
}
