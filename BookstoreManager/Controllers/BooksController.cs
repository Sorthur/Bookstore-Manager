using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookstoreManager.Data;
using BookstoreManager.Models;
using System.Data.SqlClient;
using System.Data.Entity.Migrations;

namespace BookstoreManager.Controllers
{
    public class BooksController : Controller
    {
        public async Task<ActionResult> Index()
        {
            using (var context = new DatabaseContext())
            {
                try
                {
                    return View(await context.Books.Where(b => b.IsAvailable == true).ToListAsync());
                }
                catch (SqlException)
                {
                    return View("../Error/NoDb");
                }
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Author,Year,Edition,NumberOfPages,IsHardCover,Quantity,Price")] Book book)
        {
            if (ModelState.IsValid)
            {
                using (var context = new DatabaseContext())
                {
                    try
                    {
                        context.Books.Add(book);
                        await context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (SqlException)
                    {
                        return View("../Error/NoDb");
                    }
                }
            }
            return View(book);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            using (var context = new DatabaseContext())
            {
                try
                {
                    var book = await context.Books.FindAsync(id);
                    if (book == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                    return View(book);
                }
                catch (SqlException)
                {
                    return View("../Error/NoDb");
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Author,Year,Edition,NumberOfPages,IsHardCover,Quantity,Price")] Book book)
        {
            if (ModelState.IsValid)
            {
                using (var context = new DatabaseContext())
                {
                    context.Books.AddOrUpdate(book);
                    await context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(book);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var context = new DatabaseContext())
            {
                try
                {
                    var book = await context.Books
                        .FirstOrDefaultAsync(m => m.Id == id);
                    if (book == null)
                    {
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }

                    return View(book);
                }
                catch (SqlException)
                {
                    return View("../Error/NoDb");
                }
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            using (var context = new DatabaseContext())
            {
                try
                {
                    var book = await context.Books.FindAsync(id);
                    book.IsAvailable = false;
                    await context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (SqlException)
                {
                    return View("../Error/NoDb");
                }
            }
        }
    }
}
