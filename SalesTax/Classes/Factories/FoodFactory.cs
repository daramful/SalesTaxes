using SalesTax.Classes.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesTax.Classes.Factories
{
    // concrete factory
    public class FoodFactory : ProductFactory
    {
        public override Product CreateProduct(string name, decimal price, int qty, bool isImported)
        {
            return new Food(name, price, isImported, qty);
        }
    }
}
