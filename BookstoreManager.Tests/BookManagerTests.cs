using Autofac.Extras.Moq;
using BookstoreManager.BookManager;
using BookstoreManager.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookstoreManager.Tests
{
    public class BookManagerTests
    {
        [Theory]
        [InlineData("book99", 1, true)]
        [InlineData("book1", 2, true)]
        [InlineData("book2", 2, false)]
        [InlineData("book3", 3, true)]
        public void Should_ReturnFalse_When_BookDoesntExist(string title, int edition, bool isAvailable)
        {
            // Arrange
            //var bookManager = new BookManager.BookManager();
            var books = new List<Book>
            {
                CreateTestingBook("book1", 1, true),
                CreateTestingBook("book2", 2, true),
                CreateTestingBook("book3", 3, false)
            };

            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<Data.IDatabaseManager>()
                    .Setup(x => x.GetAvailableBooksAsync().Result) //todo ma to sens?
                    .Returns(GetSampleBooks());

                //var cls = mock.Create<BookManager.BookManager>();
                //var transfersService = new TransfersService(accountsServiceMock.Object);
                //accountsServiceMock.Setup(x => x.GetBalance()).Returns(1000);

                var mock2 = new Mock<Data.IDatabaseManager>();
                mock2.Setup(x => x.GetAvailableBooksAsync().Result) // todo ma to sens?
                    .Returns(GetSampleBooks());
                var cls = new BookManager.BookManager(mock2.Object);


                var gor = cls.GetBooks();
                gor = gor;
            }

            var book = CreateTestingBook(title, edition, isAvailable);

            // Act
            //bookManager.BookExists(books, book);


            // Assert
            Assert.DoesNotContain(book, books);
        }

        [Theory]
        [InlineData("book1", 1, true)]
        [InlineData("book2", 2, true)]
        public void Should_ReturnTrue_When_BookExists(string title, int edition, bool isAvailable)
        {
            // Arrange
            //var bookManager = new BookManager.BookManager();
            var books = new List<Book>
            {
                CreateTestingBook("book1", 1, true),
                CreateTestingBook("book2", 2, true),
                CreateTestingBook("book3", 3, false)
            };
            var book = CreateTestingBook(title, edition, isAvailable);

            // Act
            //bookManager.BookExists(books, book);


            // Assert
            Assert.DoesNotContain(book, books);
        }

        private static List<Book> GetSampleBooks()
        {
            return new List<Book>
            {
                CreateTestingBook("book1", 1, true),
                CreateTestingBook("book2", 2, true),
                CreateTestingBook("book3", 3, false)
            };
        }

        private static Book CreateTestingBook(string title, int edition, bool isAvailable)
        {
            return new Book
            {
                Title = title,
                Author = "",
                Year = 0,
                Edition = edition,
                NumberOfPages = 0,
                IsHardCover = false,
                Quantity = 0,
                IsAvailable = isAvailable,
                Price = 0
            };
        }
    }
}
