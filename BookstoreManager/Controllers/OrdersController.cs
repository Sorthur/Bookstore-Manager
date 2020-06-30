using BookstoreManager.Data;
using BookstoreManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BookstoreManager.Controllers
{
    public class OrdersController : Controller
    {
        public async Task<ActionResult> Index(int id, int count)
        {
            using (var context = new DatabaseContext())
            {
                try
                {
                    var book = await context.Books.FindAsync(id);


                    book.Quantity -= count;

                    var newOrder = new Order(book, count, 0);
                    context.Orders.Add(newOrder);

                    await context.SaveChangesAsync();
                    return View((object)$"Powstało nowe zamowienie na książkę \"{book.Title}\" w ilości: {count}");
                }
                catch (SqlException)
                {
                    return View("../Error/NoDb");
                }
            }
        }

        public async Task<ActionResult> AllOrders()
        {
            using (var context = new DatabaseContext())
            {
                try
                {
                    var orders = await context.Orders.ToListAsync();
                    orders = await context.Orders
                        .Include(b => b.OrderedBook)
                        .ToListAsync();

                    return View(orders);
                }
                catch (SqlException)
                {
                    return View("../Error/NoDb");
                }
            }
        }
    }
}
