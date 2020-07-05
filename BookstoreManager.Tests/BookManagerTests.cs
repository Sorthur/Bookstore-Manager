using Autofac.Extras.Moq;
using BookstoreManager.BookManager;
using BookstoreManager.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        [InlineData("book2_", 2, true)]
        [InlineData("book3", 3, true)]
        public void Should_ReturnFalse_When_BookDoesntExist(string title, int edition, bool isAvailable)
        {
            // Arrange
            bool expected = false;
            var mock = AutoMock.GetLoose();
            mock.Mock<Data.IDatabaseManager>()
                .Setup(x => x.GetAvailableBooksAsync())
                .Returns(Task.FromResult(GetSampleBooks()));

            var bookManagerMock = mock.Create<BookManager.BookManager>();
            var books = bookManagerMock.GetAvailableBooks();
            var book = CreateTestingBook(title: title, edition: edition, isAvailable: isAvailable);

            // Act
            bool actual = bookManagerMock.BookExists(books, book);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("book1", 1, true)]
        [InlineData("book2", 2, true)]
        public void Should_ReturnTrue_When_BookExists(string title, int edition, bool isAvailable)
        {
            // Arrange
            bool expected = true;
            var mock = AutoMock.GetLoose();
            mock.Mock<Data.IDatabaseManager>()
                .Setup(x => x.GetAvailableBooksAsync())
                .Returns(Task.FromResult(GetSampleBooks()));

            var bookManagerMock = mock.Create<BookManager.BookManager>();
            var books = bookManagerMock.GetAvailableBooks();
            var book = CreateTestingBook(title: title, edition: edition, isAvailable: isAvailable);

            // Act
            bool actual = bookManagerMock.BookExists(books, book);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 5, 4, 10)]
        [InlineData(0, 5, 6, 2)]
        [InlineData(0, -1, 6, 2)]
        public void Should_ReturnFalse_When_OrderNotPossible(int bookId, int count, int bookQuantity, decimal bookPrice)
        {
            // Arrange
            bool expected = false;
            var mock = AutoMock.GetLoose();
            mock.Mock<Data.IDatabaseManager>()
                .Setup(x => x.GetAvailableBookAsync(bookId))
                .Returns(Task.FromResult(CreateTestingBook(quantity: bookQuantity, price: bookPrice)));

            var orderManagerMock = mock.Create<OrderManager.OrderManager>();

            // Act
            bool actual = orderManagerMock.IsOrderPossibleAsync(bookId, count).Result;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(0, 10, 100, 10)]
        [InlineData(0, 100, 100, 1)]
        public void Should_ReturnTrue_When_OrderPossible(int bookId, int count, int bookQuantity, decimal bookPrice)
        {
            // Arrange
            bool expected = true;
            var mock = AutoMock.GetLoose();
            mock.Mock<Data.IDatabaseManager>()
                .Setup(x => x.GetAvailableBookAsync(bookId))
                .Returns(Task.FromResult(CreateTestingBook(quantity: bookQuantity, price: bookPrice)));

            var orderManagerMock = mock.Create<OrderManager.OrderManager>();

            // Act
            bool actual = orderManagerMock.IsOrderPossibleAsync(bookId, count).Result;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async void Should_DecreaseBookQuantityByOne_When_BookWasOrdered()
        {
            // Arrange
            int expected = 0;
            int count = 1;
            var book = CreateTestingBook(quantity: 1);
            var mock = AutoMock.GetLoose();

            mock.Mock<Data.IDatabaseManager>()
                .Setup(x => x.GetAvailableBookAsync(0))
                .Returns(Task.FromResult(book));

            mock.Mock<Data.IDatabaseManager>()
                .Setup(x => x.EditBookAsync(null))
                .Returns(async (Book b) => b.Quantity -= count); /////

            mock.Mock<Data.IDatabaseManager>()
                .Setup(x => x.AddOrder(null))
                .Returns(Task.FromResult(0));

            var orderManagerMock = mock.Create<OrderManager.OrderManager>();

            // Act
            await orderManagerMock.OrderBookAsync(0, count);
            int actual = book.Quantity;

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(50, 100, 150)]
        [InlineData(150, 100, 50)]
        [InlineData(200, 90, 110, 0)]
        public void Should_ReturnValidInteger_For_AveragePageNumber(params int[] numbersOfPages)
        {
            // Arrange
            int expected = 100;
            List<Book> books = new List<Book>();
            foreach (var numberOfPages in numbersOfPages)
            {
                books.Add(CreateTestingBook(numberOfPages: numberOfPages));
            }
            var mock = AutoMock.GetLoose();
            mock.Mock<Data.IDatabaseManager>()
                .Setup(x => x.GetAvailableBooksAsync())
                .Returns(Task.FromResult(books));

            var bookManagerMock = mock.Create<BookManager.BookManager>();

            // Act
            int actual = bookManagerMock.GetAverageNumberOfPages();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("Author1", "Author2", "Author3")]
        [InlineData("Author1", "Author2", "Author3", "Author3")]
        public void Should_ReturnValidInteger_For_UniqueNumberOfAuthors(params string[] authors)
        {
            // Arrange
            int expected = 3;
            List<Book> books = new List<Book>();
            foreach (var author in authors)
            {
                books.Add(CreateTestingBook(author: author));
            }
            var mock = AutoMock.GetLoose();
            mock.Mock<Data.IDatabaseManager>()
                .Setup(x => x.GetAvailableBooksAsync())
                .Returns(Task.FromResult(books));

            var bookManagerMock = mock.Create<BookManager.BookManager>();

            // Act
            int actual = bookManagerMock.GetUniqueNumberOfAuthors();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("ook", "Book1", "book2")]
        [InlineData("ook", "Book1", "ok2", "book3")]
        [InlineData("ook", "asdqwe", "book2", "book3")]
        public void Should_ReturnValidNumberOfBooksContainingGivenPhrase(string phrase, params string[] titles)
        {
            // Arrange
            int expected = 2;
            List<Book> books = new List<Book>();
            foreach (var title in titles)
            {
                books.Add(CreateTestingBook(title: title));
            }
            var mock = AutoMock.GetLoose();
            mock.Mock<Data.IDatabaseManager>()
                .Setup(x => x.GetAvailableBooksAsync())
                .Returns(Task.FromResult(books));

            var bookManagerMock = mock.Create<BookManager.BookManager>();

            // Act
            var booksWithPhrase = bookManagerMock.GetBooksContainingGivenPhrase(phrase);
            int actual = booksWithPhrase.Count();

            // Assert
            Assert.Equal(expected, actual);
        }

        private List<Book> GetSampleBooks()
        {
            return new List<Book>
            {
                CreateTestingBook(title:"book1", edition: 1, isAvailable: true),
                CreateTestingBook(title:"book2", edition: 2, isAvailable: true),
                CreateTestingBook(title:"book3", edition: 3, isAvailable: false),
            };
        }

        private Book CreateTestingBook(string title = "", string author = "", int year = 0, int edition = 0, bool isAvailable = true, int quantity = 0, decimal price = 0, int numberOfPages = 0)
        {
            return new Book
            {
                Title = title,
                Author = author,
                Year = year,
                Edition = edition,
                NumberOfPages = numberOfPages,
                IsHardCover = false,
                Quantity = quantity,
                IsAvailable = isAvailable,
                Price = price
            };
        }
    }
}
