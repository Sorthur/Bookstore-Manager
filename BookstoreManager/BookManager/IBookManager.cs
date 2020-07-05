using BookstoreManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreManager.BookManager
{
    public interface IBookManager
    {
        bool BookExists(List<Book> books, Book book);
        List<Book> GetAvailableBooks();
        int GetNumberOfBooks();
        int GetAverageNumberOfPages();
        int GetUniqueNumberOfAuthors();
        List<Book> GetBooksContainingGivenPhrase(string phrase);

    }
}
