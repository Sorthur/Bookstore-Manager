using BookstoreManager.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BookstoreManager.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DatabaseContext() : base(@"Data Source =.\SQLEXPRESS; Initial Catalog = Lab5_mdejewski169657; Integrated Security = True") { }
    }
}