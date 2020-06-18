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

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetNumberOfBooks()
        {
            using (var context = new DatabaseContext())
            {
                try
                {
                    var count = await context.Books.Where(b => b.IsAvailable == true).CountAsync();
                    Message = $"Liczba książek w bazie: {count}";
                    return View("result", this);
                }
                catch (SqlException)
                {
                    return View("../Error/NoDb");
                }
            }
        }

        public async Task<ActionResult> GetAverageNumberOfPages()
        {
            using (var context = new DatabaseContext())
            {
                try
                {
                    var average = await context.Books.Where(b => b.IsAvailable == true).Select(b => b.NumberOfPages).AverageAsync();
                    Message = $"Średnia ilość stron: {(int)average}";
                }
                catch (InvalidOperationException)
                {
                    Message = "Baza jest pusta";
                }
                catch (SqlException)
                {
                    return View("../Error/NoDb");
                }
                return View("result", this);
            }
        }

        public async Task<ActionResult> GetUniqueNumberOfAuthors()
        {
            using (var context = new DatabaseContext())
            {
                try
                {
                    var count = await context.Books.Where(b => b.IsAvailable == true).Select(b => b.Author).Distinct().CountAsync();
                    Message = $"Liczba unikalnych autorów: {count}";
                    return View("result", this);
                }
                catch (SqlException)
                {
                    return View("../Error/NoDb");
                }
            }
        }

        public async Task<ActionResult> GetBooksContainingGivenPhrase(string phrase)
        {
            using (var context = new DatabaseContext())
            {
                try
                {
                    //var count = context.Books.Where(b => b.Title.Contains(phrase)).Count();
                    Books = await context.Books.Where(b => b.Title.Contains(phrase) && b.IsAvailable == true).ToListAsync();
                    var count = Books.Count();
                    Message = $"Liczba książek zawierających \"{phrase}\" w tytule: {count}";
                    return View("result", this);
                }
                catch (SqlException)
                {
                    return View("../Error/NoDb");
                }
            }
        }
    }
}