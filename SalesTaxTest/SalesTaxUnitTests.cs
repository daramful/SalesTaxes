using Microsoft.VisualStudio.TestTools.UnitTesting;
using SalesTax;
using SalesTax.Classes.Cart;
using SalesTax.Classes.Factories;
using SalesTax.Classes.Products;
using System.Configuration;
using System.Linq;

namespace SalesTaxTest
{
    [TestClass]
    public class SalesTaxUnitTests
    {
        private const string SALESTAXKEY = "SalesTaxrate";
        private const string IMPORTTAXKEY = "ImportTaxrate";
        private const string RECEIPTFILEKEY = "ReceiptFilename";
        public TaxSettings TaxSettings = new TaxSettings();
        public static decimal SALES_TAXRATE;
        public static decimal IMPORT_TAXRATE;

        /** Verify if configuration values exist */
        [TestInitialize]
        public void Setup()
        {
            var salesTaxExists = decimal.TryParse(ConfigurationManager.AppSettings[SALESTAXKEY], out SALES_TAXRATE);
            var importTaxExists = decimal.TryParse(ConfigurationManager.AppSettings[IMPORTTAXKEY], out IMPORT_TAXRATE);
            var receiptNameExists = ConfigurationManager.AppSettings[RECEIPTFILEKEY];

            if (salesTaxExists && importTaxExists)
            {
                TaxSettings.SalesTaxrate = SALES_TAXRATE;
                TaxSettings.ImportTaxrate = IMPORT_TAXRATE;
            }
        }

        [TestMethod]
        public void TestCalculateTax_Book_Imported()
        {
            ProductFactory productFactory = new BookFactory();
            var name = "book";
            var price = 11.25m;
            var qty = 2;
            bool isImported = true;
            Product book = productFactory.CreateProduct(name, price, qty, isImported);
            decimal expectedTax = 1.20m;
            var actualTax = book.CalculateTax(TaxSettings);
            Assert.AreEqual(expectedTax, actualTax);
        }

        [TestMethod]
        public void TestCalculateTax_Book_NotImported()
        {
            ProductFactory productFactory = new BookFactory();
            var name = "book";
            var price = 12.49m;
            var qty = 3;
            bool isImported = false;
            Product book = productFactory.CreateProduct(name, price, qty, isImported); 
            decimal expectedTax = 0m;
            var actualTax = book.CalculateTax(TaxSettings);

            Assert.AreEqual(expectedTax, actualTax);
        }

        [TestMethod]
        public void TestCalculateTax_Food_Imported()
        {
            ProductFactory productFactory = new FoodFactory();
            var name = "Box of Chocolates";
            var price = 11.25m;
            var qty = 2;
            bool isImported = true;
            Product food = productFactory.CreateProduct(name, price, qty, isImported);
            decimal expectedTax = 1.20m;
            var actualTax = food.CalculateTax(TaxSettings);

            Assert.AreEqual(expectedTax, actualTax);
        }

        [TestMethod]
        public void TestCalculateTax_Food_NotImported()
        {
            ProductFactory productFactory = new FoodFactory();
            var name = "Huge Candies";
            var price = 8.99m;
            var qty = 3;
            bool isImported = false;
            Product food = productFactory.CreateProduct(name, price, qty, isImported);
            decimal expectedTax = 0m;
            var actualTax = food.CalculateTax(TaxSettings);

            Assert.AreEqual(expectedTax, actualTax);
        }

        [TestMethod]
        public void TestCalculateTax_Medical_Imported()
        {
            ProductFactory productFactory = new MedicalFactory();
            var name = "Packet of headache pills";
            var price = 9.75m;
            var qty = 1;
            bool isImported = true;
            Product meds = productFactory.CreateProduct(name, price, qty, isImported);
            decimal expectedTax = .50m;
            var actualTax = meds.CalculateTax(TaxSettings);

            Assert.AreEqual(expectedTax, actualTax);
        }

        [TestMethod]
        public void TestCalculateTax_Medical_NotImported()
        {
            ProductFactory productFactory = new MedicalFactory();
            var name = "Crutches";
            var price = 150;
            var qty = 2;
            bool isImported = false;
            Product meds = productFactory.CreateProduct(name, price, qty, isImported);
            decimal expectedTax = 0m;
            var actualTax = meds.CalculateTax(TaxSettings);

            Assert.AreEqual(expectedTax, actualTax);
        }

        [TestMethod]
        public void TestCalculateTax_Other_Imported()
        {
            ProductFactory productFactory = new OthersFactory();
            var name = "Light";
            var price = 5.99m;
            var qty = 3;
            bool isImported = true;
            Product others = productFactory.CreateProduct(name, price, qty, isImported);
            decimal expectedTax = 2.7m;
            var actualTax = others.CalculateTax(TaxSettings);

            Assert.AreEqual(expectedTax, actualTax);
        }

        [TestMethod]
        public void TestCalculateTax_Other_NotImported()
        {
            ProductFactory productFactory = new OthersFactory();
            var name = "Gaming Computer";
            var price = 2399.99m;
            var qty = 2;
            bool isImported = false;
            Product others = productFactory.CreateProduct(name, price, qty, isImported);
            decimal expectedTax = 480m;
            var actualTax = others.CalculateTax(TaxSettings);

            Assert.AreEqual(expectedTax, actualTax);
        }

        [TestMethod]
        public void TestUpdateCart_AddSameProduct()
        {
            ShoppingCart shoppingCart = new ShoppingCart();
            ProductFactory productFactory = new OthersFactory();
            var name = "Gaming Computer";
            var price = 2399.99m;
            var qty = 1;
            bool isImported = false;
            Product others = productFactory.CreateProduct(name, price, qty, isImported);
            // load first product
            shoppingCart.UpdateCart(others);
            
            // Add same product
            shoppingCart.UpdateCart(others);

            // One object of products exist in shopping cart
            Assert.AreEqual(shoppingCart.CartSize(), 1);


            var product = shoppingCart.Products.Where(item => item.Name == name).FirstOrDefault();
            Assert.AreEqual(product.Qty, 2);
        }

        [TestMethod]
        public void TestUpdateCart_AddDifferentProduct()
        {
            ShoppingCart shoppingCart = new ShoppingCart();
            ProductFactory productFactory = new OthersFactory();
            var name = "Gaming Computer";
            var price = 2399.99m;
            var qty = 1;
            bool isImported = false;
            Product others = productFactory.CreateProduct(name, price, qty, isImported);
            // load first product
            shoppingCart.UpdateCart(others);

            productFactory = new OthersFactory();
            name = "Gaming mouse";
            price = 60.99m;
            qty = 1;
            isImported = false;
            others = productFactory.CreateProduct(name, price, qty, isImported);
            // Add same product
            shoppingCart.UpdateCart(others);


            // One object of products exist in shopping cart
            Assert.AreEqual(shoppingCart.CartSize(), 2);
            var product = shoppingCart.Products.Where(item => item.Name == name).FirstOrDefault();
            Assert.AreEqual(product.Qty, 1);
        }

    }
}
