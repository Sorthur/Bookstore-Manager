using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookstoreManager.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public int Edition { get; set; }
        public int NumberOfPages { get; set; }
        public bool IsHardCover { get; set; }
        public int Quantity { get; set; }
        public bool IsAvailable { get; set; }
        public decimal Price { get; set; }

        public Book()
        {
            IsAvailable = true;
        }

        public Book(string title, string author, int year, int edition, int numberOfPages, bool isHardCover, decimal price, int quantity = 1)
        {
            Title = title;
            Author = author;
            Year = year;
            Edition = edition;
            NumberOfPages = numberOfPages;
            IsHardCover = isHardCover;
            Quantity = quantity;
            Price = price;
            IsAvailable = true;
        }
    }
}