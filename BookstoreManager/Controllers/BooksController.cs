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
using BookstoreManager.BookManager;
using System.Data.SqlClient;
using System.Data.Entity.Migrations;
using BookstoreManager.Bootstrap;

namespace BookstoreManager.Controllers
{
    public class BooksController : Controller
    {
        private IBookManager _bookManager;
        private IDatabaseManager _databaseManager;
        //public BooksController()
        //{
        //    var kernel = new DiConfig().GetKernel();
        //}

        public BooksController(IBookManager bookManager, IDatabaseManager databaseManager)
        {
            _bookManager = bookManager;
            _databaseManager = databaseManager;
        }

        public async Task<ActionResult> Index()
        {
            //using (var context = new DatabaseContext())
            //{
            try
            {
                //return View(context.Books.Where(b => b.IsAvailable == true).ToListAsync());
                return View(await _databaseManager.GetAvailableBooksAsync());
                //var books = await _databaseManager.GetAvailableBooksAsync();
                //return View(books);
            }
            catch (SqlException)
            {
                return View("../Error/NoDb");
            }
            //}
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
                //using (var context = new DatabaseContext())
                //{
                try
                {
                    //    var books = context.Books.ToList();
                    //    var bookManager = new BookManager.BookManager();
                    //    if (bookManager.BookExists(books, book))
                    //    {
                    //        return View("BookExists");
                    //    }
                    //    books.Add(book);
                    //    await context.SaveChangesAsync();
                    //    return RedirectToAction(nameof(Index));
                    var books = await _databaseManager.GetAvailableBooksAsync();
                    if (_bookManager.BookExists(books, book))
                    {
                        return View("BookExists");
                    }
                    await _databaseManager.AddBookAsync(book);
                    return RedirectToAction(nameof(Index));
                }
                catch (SqlException)
                {
                    return View("../Error/NoDb");
                }
                //}
            }
            return View(book);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            //using (var context = new DatabaseContext())
            //{
            try
            {
                //var book = await context.Books.FindAsync(id);
                //if (book == null)
                //{
                //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //}
                //return View(book);
                var book = await _databaseManager.GetAvailableBookAsync(id.Value);
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
            //}
        }

        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Author,Year,Edition,NumberOfPages,IsHardCover,Quantity,Price")] Book book)
        {
            if (ModelState.IsValid)
            {
                //using (var context = new DatabaseContext())
                //{
                //context.Books.AddOrUpdate(book);
                //await context.SaveChangesAsync();
                await _databaseManager.EditBookAsync(book);
                return RedirectToAction(nameof(Index));
                //}
            }
            return View(book);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //using (var context = new DatabaseContext())
            //{
            try
            {
                //var book = await context.Books
                //    .FirstOrDefaultAsync(m => m.Id == id);
                var book = await _databaseManager.GetAvailableBookAsync(id.Value);
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
            //}
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            //using (var context = new DatabaseContext())
            //{
            try
            {
                //var book = await context.Books.FindAsync(id);
                //book.IsAvailable = false;
                //await context.SaveChangesAsync();
                await _databaseManager.DeleteBookAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException)
            {
                return View("../Error/NoDb");
            }
            //}
        }
    }
}
