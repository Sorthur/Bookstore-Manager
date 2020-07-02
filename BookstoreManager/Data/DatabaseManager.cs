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

        public async Task EditBookAsync(Book book)
        {
            using (var context = new DatabaseContext())
            {
                context.Books.AddOrUpdate(book);
                await context.SaveChangesAsync();
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

        public async Task AddOrder(Order order)
        {
            using (var context = new DatabaseContext())
            {
                context.Orders.Add(order);
                await context.SaveChangesAsync();
            }
        }
    }
}