using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookstoreManager.Models
{
    public class Order
    {
        public int Id { get; set; }
        public Book OrderedBook { get; set; }
        public int NumberOfOrderedBooks { get; set; }

        public Order() { }

        public Order(Book orderedBook, int numberOfOrderedBooks)
        {
            OrderedBook = orderedBook;
            NumberOfOrderedBooks = numberOfOrderedBooks;
        }
    }
}