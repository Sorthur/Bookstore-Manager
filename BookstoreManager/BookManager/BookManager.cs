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
        IDatabaseManager _databaseManager;

        public BookManager(IDatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;
        }

        public bool BookExists(List<Book> books, Book book)
        {
            using (var context = new DatabaseContext())
            {
                if (books.Any(b => b.Title == book.Title
                && b.Edition == book.Edition
                && b.IsAvailable == true))
                {
                    return true;
                }
                return false;
            }
        }

        public List<Book> GetAvailableBooks()
        {
            return _databaseManager.GetAvailableBooksAsync().Result;
        }

        public int GetNumberOfBooks()
        {
            return GetAvailableBooks().Count;
        }

        public int GetAverageNumberOfPages()
        {
            try
            {
                return (int)GetAvailableBooks().Select(b => b.NumberOfPages).Average();
            }
            catch (InvalidOperationException)
            {
                return 0;
            }
        }

        public int GetUniqueNumberOfAuthors()
        {
            return GetAvailableBooks().Select(b => b.Author).Distinct().Count();
        }

        public List<Book> GetBooksContainingGivenPhrase(string phrase)
        {
            return GetAvailableBooks().Where(b => b.Title.Contains(phrase)).ToList();
        }
    }
}