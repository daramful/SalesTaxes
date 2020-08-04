using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SalesTax.Classes.Enums
{
    //to determine Product Factory
    public enum ProductType
    {
        [Description("(1) Book")]
        Book = 1,
        [Description("(2) Food")]
        Food = 2,
        [Description("(3) Medical")]
        Medical = 3,
        [Description("(4) Other" )]
        Other = 4
    }
}
