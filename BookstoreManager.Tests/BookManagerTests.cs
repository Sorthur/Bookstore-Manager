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
        [InlineData("book2_", 2, true)]
        [InlineData("book3", 3, true)]
        public void Should_ReturnFalse_When_BookDoesntExist(string title, int edition, bool isAvailable)
        {
            bool expected = false;
            var mock = AutoMock.GetLoose();
            mock.Mock<Data.IDatabaseManager>()
                .Setup(x => x.GetAvailableBooksAsync())
                .Returns(Task.FromResult(GetSampleBooks()));

            var bookManagerMock = mock.Create<BookManager.BookManager>();
            var books = bookManagerMock.GetBooks();
            var book = CreateTestingBook(title, edition, isAvailable, 0, 0);

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
            var books = bookManagerMock.GetBooks();
            var book = CreateTestingBook(title, edition, isAvailable, 0, 0);

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
                .Returns(Task.FromResult(CreateTestingBook("", 0, true, bookQuantity, bookPrice)));

            var orderManagerMock = mock.Create<OrderManager.OrderManager>();

            // Act
            bool actual = orderManagerMock.IsOrderPossibleAsync(bookId, count).Result;

            // Assert
            Assert.Equal(expected, actual);
        }



        private List<Book> GetSampleBooks()
        {
            return new List<Book>
            {
                CreateTestingBook("book1", 1, true, 0, 0),
                CreateTestingBook("book2", 2, true, 0, 0),
                CreateTestingBook("book3", 3, false, 0, 0)
            };
        }

        private Book CreateTestingBook(string title, int edition, bool isAvailable, int quantity, decimal price)
        {
            return new Book
            {
                Title = title,
                Author = "",
                Year = 0,
                Edition = edition,
                NumberOfPages = 0,
                IsHardCover = false,
                Quantity = quantity,
                IsAvailable = isAvailable,
                Price = price
            };
        }
    }
}
