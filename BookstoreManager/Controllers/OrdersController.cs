using BookstoreManager.Data;
using BookstoreManager.Models;
using BookstoreManager.OrderManager;
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
        private IOrderManager _orderManager;
        private IDatabaseManager _databaseManager;

        public OrdersController(IOrderManager orderManager, IDatabaseManager databaseManager)
        {
            _orderManager = orderManager;
            _databaseManager = databaseManager;

        }
        public async Task<ActionResult> Index(int id, int count)
        {
            using (var context = new DatabaseContext())
            {
                try
                {
                    if (!await _orderManager.IsOrderPossibleAsync(id, count))
                    {
                        return View((object)_orderManager.GetMessage());
                    }
                    await _orderManager.OrderBookAsync(id, count);
                    return View((object)_orderManager.GetMessage());
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
                    return View(await _databaseManager.GetOrdersAsync());
                }
                catch (SqlException)
                {
                    return View("../Error/NoDb");
                }
            }
        }
    }
}
