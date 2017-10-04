using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookshop
{
    class Bookshop
    {
        public string adress;

        public Dictionary<string, Bookshelf> Bookshelves { get; set; }
        public double Budget { get; set; }

        public void SellBook(Book book) { }
        public static double MonthlySales(double income, string currency) { return 0.0; }
    }
}
