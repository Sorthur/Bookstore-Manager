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
                if (context.Books.Any(b => b.Title == book.Title
                && b.Edition == book.Edition
                && b.IsAvailable == true))
                {
                    return true;
                }
                return false;
            }
        }
    }
}