using BookstoreManager.Data;
using BookstoreManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BookstoreManager.OrderManager
{
    public class OrderManager : IOrderManager
    {
        private IDatabaseManager _databaseManager;
        private string Message;
        public OrderManager(IDatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;
        }

        public async Task<bool> IsOrderPossibleAsync(int bookId, int count)
        {
            var book = await _databaseManager.GetAvailableBookAsync(bookId);
            if (book == null)
            {
                Message = "Nie można zamówić danej książki";
                return false;
            }
            else if (count <= 0)
            {
                Message = "Ilość zamawianych książek musi być większa od 0";
                return false;
            }
            else if (book.Quantity < count)
            {
                Message = "Nie ma tyle książek na magazynie";
                return false;
            }

            decimal orderPrice = book.Price * count;
            if (orderPrice < 40)
            {
                Message = $"Kwota zamówienia musi przekraczać 40zł; Aktualna kwota: {orderPrice}";
                return false;
            }
            return true;
        }

        public async Task OrderBookAsync(int bookId, int count)
        {
            var book = await _databaseManager.GetAvailableBookAsync(bookId);
            book.Quantity -= count;
            var newOrder = new Order(book, count, book.Price * count);

            await _databaseManager.EditBookAsync(book);
            await _databaseManager.AddOrder(newOrder);
            Message = $"Powstało nowe zamowienie na książkę \"{book.Title}\" w ilości: {count}";
        }

        public string GetMessage()
        {
            return Message;
        }
    }
}