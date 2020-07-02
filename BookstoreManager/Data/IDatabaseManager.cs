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
        Task<List<Book>> GetAvailableBooksAsync();
        Task<Book> GetAvailableBookAsync(int id);
        Task AddBookAsync(Book book);
        Task EditBookAsync(Book book);
        Task DeleteBookAsync(int id);
        Task AddOrder(Order order);
        Task<List<Order>> GetOrdersAsync();

    }
}
