using SalesTax.Classes.Factories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTax.Classes.Products
{
    // concrete Product: Book, Food, Medical, Others
    public class Book : Product
    {
        public Book(string name, decimal price, bool isImported, int qty)
            : base(name, price, isImported, qty)
        {
        }

        public override ProductFactory GetFactory()
        {
            return new BookFactory();
        }
    }
}
