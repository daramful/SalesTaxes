using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;

namespace SalesTax
{
    static class Utility
    {
        // store file in current solution folder
        // Get the current directory.
        private static readonly string path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
        public static readonly decimal IMPORT_TAXRATE;
        public static readonly decimal SALES_TAXRATE;

        private static readonly string receiptFileName = ConfigurationManager.AppSettings[Resources.AppSettings.RECEIPTFILEKEY];
        private static readonly bool isDutytaxInt = decimal.TryParse(ConfigurationManager.AppSettings[Resources.AppSettings.IMPORTTAXKEY], out IMPORT_TAXRATE);
        private static readonly bool isBasetaxInt = decimal.TryParse(ConfigurationManager.AppSettings[Resources.AppSettings.SALESTAXKEY], out SALES_TAXRATE);
        private const decimal ROUND_OFF = (decimal)0.05;
        /// <summary>
        /// Get Tax Configuration in App.Config file 
        /// </summary>
        /// <returns></returns>
        public static TaxSettings GetTaxConfiguration()
        {
            if (!isDutytaxInt || !isBasetaxInt)
            {
                throw new SystemException(Resources.Messages.ERROR_TAXCONFIGURATION);
            }

            return new TaxSettings { ImportTaxrate = IMPORT_TAXRATE, SalesTaxrate = SALES_TAXRATE };
        }
        /// <summary>
        /// Rounds off a double value to the nearest 0.05
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal RoundOffToNearestNickle(decimal value)
        {
            return Math.Round(Math.Ceiling(value/ROUND_OFF) * ROUND_OFF, 2);
        }

        /// <summary>
        /// Write one line of log message to log file each time
        /// </summary>
        /// <param name="msg"></param>
        public static void CreateReceipt(List<string> msgs)
        {
            String filePath = path + "\\" + receiptFileName;
            // WriteAllLines creates a file, writes a collection of strings to the file,
            // and then closes the file.  You do NOT need to call Flush() or Close().
            File.WriteAllLines(filePath, msgs);
            Console.WriteLine(Resources.Messages.PATH_RECEIPT, filePath);
        }


    }
}
