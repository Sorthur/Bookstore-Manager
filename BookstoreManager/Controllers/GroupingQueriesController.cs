using BookstoreManager.BookManager;
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
    public class GroupingQueriesController : Controller
    {
        public string Message { get; set; }
        public List<Book> Books { get; set; }

        private IDatabaseManager _databaseManager;
        private IBookManager _bookManager;

        public GroupingQueriesController(IBookManager bookManager, IDatabaseManager databaseManager)
        {
            _databaseManager = databaseManager;
            _bookManager = bookManager;
        }

        public ActionResult Index()
        {
            if (!_databaseManager.DatabaseExists())
            {
                return View("../Error/NoDb");
            }
            return View();
        }

        public async Task<ActionResult> GetNumberOfBooks()
        {
            if (!_databaseManager.DatabaseExists())
            {
                return View("../Error/NoDb");
            }
            Message = $"Liczba książek w bazie: {_bookManager.GetNumberOfBooks()}";
            return View("result", this);
        }


        public async Task<ActionResult> GetAverageNumberOfPages()
        {
            if (!_databaseManager.DatabaseExists())
            {
                return View("../Error/NoDb");
            }
            Message = $"Średnia ilość stron: {_bookManager.GetAverageNumberOfPages()}";
            return View("result", this);
        }

        public async Task<ActionResult> GetUniqueNumberOfAuthors()
        {
            if (!_databaseManager.DatabaseExists())
            {
                return View("../Error/NoDb");
            }
            Message = $"Liczba unikalnych autorów: {_bookManager.GetUniqueNumberOfAuthors()}";
            return View("result", this);
        }

        public async Task<ActionResult> GetBooksContainingGivenPhrase(string phrase)
        {
            if (!_databaseManager.DatabaseExists())
            {
                return View("../Error/NoDb");
            }
            Books = _bookManager.GetBooksContainingGivenPhrase(phrase);
            var count = Books.Count();
            Message = $"Liczba książek zawierających \"{phrase}\" w tytule: {count}";
            return View("result", this);
        }
    }
}
