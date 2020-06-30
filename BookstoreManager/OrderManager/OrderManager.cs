using BookstoreManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookstoreManager.OrderManager
{
    public class OrderManager : IOrderManager
    {
        private string Message;

        public bool IsOrderPossible(Book book, int count)
        {
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

        public string GetMessage()
        {
            return Message;
        }
    }
}