using BookstoreManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookstoreManager.OrderManager
{
    public interface IOrderManager
    {
        Task<bool> IsOrderPossibleAsync(int bookId, int count);
        Task OrderBookAsync(int bookId, int count);
        string GetMessage();
    }
}
