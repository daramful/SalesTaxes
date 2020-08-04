using SalesTax.Classes.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SalesTax.Classes.Cart
{
    public class ShoppingCart
    {
        public List<Product> Products;

        public ShoppingCart()
        {
            Products = new List<Product>();
        }
        public int CartSize() => Products.Count;

        /// <summary>
        /// update the quantity of item in the shopping cart if there is a duplicate
        /// </summary>
        /// <param name="_product"></param>
        public void UpdateCart(Product _product)
        {
            //if all of attributes don't match, assume it's a different product with same name
            List<Product> productinfo = Products.Where(o => string.Equals(o.Name, _product.Name, StringComparison.OrdinalIgnoreCase)
                && decimal.Equals(o.Price, _product.Price)
                && bool.Equals(o.IsImported, _product.IsImported)
                && int.Equals(o.Type, _product.Type)).ToList();

            if (productinfo.Any())
            {
                Products.Where(o => string.Equals(o.Name, _product.Name)
                && decimal.Equals(o.Price, _product.Price)
                && bool.Equals(o.IsImported, _product.IsImported)
                && int.Equals(o.Type, _product.Type)).FirstOrDefault().Qty += _product.Qty;
            }
            else {
                Products.Add(_product);
            }
        }
    }
}
