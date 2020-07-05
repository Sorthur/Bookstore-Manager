using BookstoreManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BookstoreManager.Data
{
    public class DatabaseManager : IDatabaseManager
    {
        public bool DatabaseExists()
        {
            using (var context = new DatabaseContext())
            {
                return context.Database.Exists();
            }
        }
        public async Task AddBookAsync(Book book)
        {
            using (var context = new DatabaseContext())
            {
                context.Books.Add(book);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteBookAsync(int id)
        {
            using (var context = new DatabaseContext())
            {
                //context.Books.Remove(book);
                var book = await context.Books.FindAsync(id);
                book.IsAvailable = false;
                await context.SaveChangesAsync();
            }
        }

        public async Task EditBookAsync(Book newBook, int bookId)
        {
            using (var context = new DatabaseContext())
            {
                context.Books.FirstOrDefault(b => b.Id == bookId).EditBook(newBook);
                await context.SaveChangesAsync();
            }
        }

        public async Task DecreaseBookQuantity(int bookId, int count)
        {
            using (var context = new DatabaseContext())
            {
                var book = context.Books.SingleOrDefault(b => b.Id == bookId);
                book.Quantity -= count;
                context.SaveChanges();
            }
        }

        public async Task<Book> GetAvailableBookAsync(int id)
        {
            using (var context = new DatabaseContext())
            {
                return await context.Books.Where(b => b.IsAvailable == true).FirstOrDefaultAsync(b => b.Id == id);
            }
        }

        public async Task<List<Book>> GetAvailableBooksAsync()
        {
            using (var context = new DatabaseContext())
            {
                return await context.Books.Where(b => b.IsAvailable == true).ToListAsync();
            }
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            using (var context = new DatabaseContext())
            {
                var orders = await context.Orders.ToListAsync();
                orders = await context.Orders
                    .Include(b => b.OrderedBook)
                    .ToListAsync();
                return orders;
            }
        }

        public async Task MakeNewOrder(int bookId, int numberOfOrderedBooks)
        {
            using (var context = new DatabaseContext())
            {
                var book = context.Books.FirstOrDefault(b => b.Id == bookId);
                var order = new Order(book, numberOfOrderedBooks, book.Price * numberOfOrderedBooks);
                context.Orders.Add(order);
                await context.SaveChangesAsync();
            }
        }
    }
}