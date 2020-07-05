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

namespace BookstoreManager.Controllers
{
    public class BooksController : Controller
    {
        private IBookManager _bookManager;
        private IDatabaseManager _databaseManager;

        public BooksController(IBookManager bookManager, IDatabaseManager databaseManager)
        {
            _bookManager = bookManager;
            _databaseManager = databaseManager;
        }

        public async Task<ActionResult> Index()
        {
            if (!_databaseManager.DatabaseExists())
            {
                return View("../Error/NoDb");
            }
            return View(await _databaseManager.GetAvailableBooksAsync());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Author,Year,Edition,NumberOfPages,IsHardCover,Quantity,Price")] Book book)
        {
            if (!_databaseManager.DatabaseExists())
            {
                return View("../Error/NoDb");
            }

            if (ModelState.IsValid)
            {
                var books = await _databaseManager.GetAvailableBooksAsync();
                if (_bookManager.BookExists(books, book))
                {
                    return View("BookExists");
                }
                await _databaseManager.AddBookAsync(book);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (!_databaseManager.DatabaseExists())
            {
                return View("../Error/NoDb");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            var book = await _databaseManager.GetAvailableBookAsync(id.Value);
            if (book == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(book);
        }

        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Author,Year,Edition,NumberOfPages,IsHardCover,Quantity,Price")] Book book)
        {
            if (!_databaseManager.DatabaseExists())
            {
                return View("../Error/NoDb");
            }

            if (ModelState.IsValid)
            {
                await _databaseManager.EditBookAsync(book, book.Id);
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (!_databaseManager.DatabaseExists())
            {
                return View("../Error/NoDb");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var book = await _databaseManager.GetAvailableBookAsync(id.Value);
            if (book == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (!_databaseManager.DatabaseExists())
            {
                return View("../Error/NoDb");
            }

            await _databaseManager.DeleteBookAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
