using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookshop
{
    class Bookshelf
    {
        private ObservableCollection<Book> Books { get; set; }        

        public void RemoveBook(Book book)
        {
            Books.Remove(book);
        }

        public void AddBook(Book book)
        {
            Books.Add(book);
        }

        public Book GetBook(int index)
        {
            if (index < Books.Count)
                return Books[index];
            else return null;
        }

    }
}
