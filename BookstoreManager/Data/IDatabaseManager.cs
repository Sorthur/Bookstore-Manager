using BookstoreManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreManager.Data
{
    public interface IDatabaseManager
    {
        bool DatabaseExists();
        Task<List<Book>> GetAvailableBooksAsync();
        Task<Book> GetAvailableBookAsync(int id);
        Task AddBookAsync(Book book);
        Task EditBookAsync(Book newBook, int bookId);
        Task DecreaseBookQuantity(int bookId, int count);
        Task DeleteBookAsync(int id);
        Task MakeNewOrder(int bookId, int numberOfOrderedBooks);
        Task<List<Order>> GetOrdersAsync();

    }
}
