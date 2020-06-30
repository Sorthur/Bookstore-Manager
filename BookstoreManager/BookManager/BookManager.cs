using BookstoreManager.Data;
using BookstoreManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BookstoreManager.BookManager
{
    public class BookManager : IBookManager
    {
        public bool BookExists(Book book)
        {
            using (var context = new DatabaseContext())
            {
                if (context.Books.Any(b => b.Title == book.Title) && context.Books.Any(b => b.Edition == book.Edition))
                {
                    return true;
                }
                return false;
            }
        }
    }
}