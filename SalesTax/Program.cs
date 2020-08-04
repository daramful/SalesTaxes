using SalesTax.Classes.Cart;
using SalesTax.Classes.Enums;
using SalesTax.Classes.Factories;
using SalesTax.Classes.Products;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesTax
{
    class Program
    {
        static void Main(string[] args)
        {
            ProductFactory productFactory = null;

            TaxSettings TaxConfig = Utility.GetTaxConfiguration();
            var currCart = new ShoppingCart();
            int count = 1;
            while (true)
            {
                Console.WriteLine(String.Format(Resources.Messages.ITEM_COUNT_HEADER, count.ToString()));
                #region Name
                Console.WriteLine(Resources.Messages.PROMPT_NAME);
                var name = Console.ReadLine().Trim().ToLower();
                #endregion

                #region Price
                Console.WriteLine(string.Format(Resources.Messages.PROMPT_PRICE, name));
                if (!decimal.TryParse(Console.ReadLine(), out decimal price))
                {
                    Console.WriteLine(Resources.Messages.ERROR_INVALIDPRICE);
                    continue;
                    //throw new SystemException("This product cannot be added to the cart due to invalid price value. Stopping the transaction");
                }
                #endregion

                #region Is Imported
                ConsoleKey responseIsImported;
                do
                {
                    Console.WriteLine(Resources.Messages.PROMPT_IMPORTED);
                    responseIsImported = Console.ReadKey(false).Key;   // true is intercept key (dont show), false is show
                    if (responseIsImported != ConsoleKey.Enter)
                        Console.WriteLine();

                } while (responseIsImported != ConsoleKey.Y && responseIsImported != ConsoleKey.N);
                bool isImported = responseIsImported == ConsoleKey.Y;
                #endregion

                #region Is Exempted
                var listOfProductType = Enum.GetNames(typeof(ProductType)).ToList<string>();
                string option_string = string.Empty;
                int index = 1;
                foreach (var type in listOfProductType)
                {
                    option_string += string.Format("\n({0}) {1}", new object[] { index.ToString(), type });
                    index++;
                }
                ProductType productType = ProductType.Other;
                Console.WriteLine(String.Format(Resources.Messages.PROMPT_EXEMPTED, option_string));
                Console.WriteLine(Resources.Messages.ENTER_DEFAULT);
                if (!Enum.TryParse(Console.ReadLine(), out productType))
                {
                    productType = ProductType.Other;
                    Console.WriteLine(Resources.Messages.SELECTED_OTHER);
                }
                else
                {
                    Console.WriteLine(String.Format(Resources.Messages.SELECTED_ITEM, productType.ToString()));
                }
                #endregion

                #region Quantity default to one
                var qty = 1;
                #endregion

                #region Confirm Additional Item
                bool confirmReadyForCheckout = false;
                ConsoleKey responseAdditionalItem;
                do
                {
                    Console.WriteLine(Resources.Messages.PROMPT_CHECKOUT);
                    responseAdditionalItem = Console.ReadKey(false).Key;   // true is intercept key (dont show), false is show
                    if (responseAdditionalItem != ConsoleKey.Enter)
                        Console.WriteLine();

                } while (responseAdditionalItem != ConsoleKey.Y && responseAdditionalItem != ConsoleKey.N);
                confirmReadyForCheckout = responseAdditionalItem == ConsoleKey.Y;
                #endregion

                #region get concrete factory of productType
                switch (productType)
                {
                    case ProductType.Book:
                        productFactory = new Book(name, price, isImported, qty).GetFactory();
                        break;
                    case ProductType.Food:
                        productFactory = new Food(name, price, isImported, qty).GetFactory();
                        break;
                    case ProductType.Medical:
                        productFactory = new Medical(name, price, isImported, qty).GetFactory();
                        break;
                    default:
                        productFactory = new Others(name, price, isImported, qty).GetFactory();
                        break;
                };

                var product = productFactory.CreateProduct(name, Convert.ToDecimal(price), Convert.ToInt32(qty), isImported);
                currCart.UpdateCart(product);
                #endregion

                if (confirmReadyForCheckout)
                {
                    break;
                }
                else
                {
                    count++;
                    continue;
                }
            }

            try
            {
                List<string> lines = new List<string>();
                foreach (var item in currCart.Products)
                {
                    var itemName = item.IsImported ? string.Format("Imported {0}", item.Name) : item.Name[0].ToString().ToUpper() + item.Name.Substring(1);

                    var getItemTax = item.CalculateTax(TaxConfig);
                    var productTotalTax = item.Qty * item.PriceOneItemAfterTax;

                    var itemized = string.Format(Resources.Messages.ITEMIZE_BODY, new object[] { itemName, productTotalTax });
                    if (item.Qty > 1)
                    {
                        itemized += " " + string.Format(Resources.Messages.ITEMIZE_MULTIPLE, new object[] { item.Qty, item.PriceOneItemAfterTax });
                    }
                    lines.Add(itemized);
                }

                var salestax = currCart.Products.Sum(item => item.CalculateTax(TaxConfig));
                var total = currCart.Products.Sum(item => item.Price * item.Qty) + salestax;
                lines.Add(string.Format(Resources.Messages.ITEMIZE_TAX, salestax));
                lines.Add(string.Format(Resources.Messages.ITEMIZE_TOTAL, total));
                Utility.CreateReceipt(lines);
            }
            catch (Exception e)
            {
                Console.WriteLine(Resources.Messages.PROCESS_FAIL, e.ToString());
            }
            finally
            {
                Console.WriteLine(Resources.Messages.TRANSACTIONEND);
                Console.ReadLine();
            }
        }
    }
}
