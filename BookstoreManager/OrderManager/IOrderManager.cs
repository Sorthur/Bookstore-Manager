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
        bool IsOrderPossible(Book book, int count);
        string GetMessage();
    }
}
