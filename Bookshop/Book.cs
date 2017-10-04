using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookshop
{
    public class Book 
    {
        public string Name { get; set; }
        public string SerialNumber { get; set; }
        public double Price { get; set; }

        Book() { }

        //public int Compare(Book x, Book y)
        //{
        //    return x.Price.CompareTo(y.Price);
       // }
    }
}

