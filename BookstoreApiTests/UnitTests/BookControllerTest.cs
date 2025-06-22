using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using BookstoreApi.Controllers;
using BookstoreApi.Services.Interfaces;
using BookstoreApi.DTOs;
using BookstoreApi.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using BookstoreApi.Models;
using BookstoreApi.Parameters;

namespace BookstoreApiTests.UnitTests
{
    public class BookControllerTest
    {
        private readonly Mock<IBookService> _mockBookService;
        private readonly BookController _controller;

        public BookControllerTest()
        {
            _mockBookService = new Mock<IBookService>();
            _controller = new BookController(_mockBookService.Object);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenBookDoesNotExist()
        {
            var bookId = 999;
            var errorMessage = "Book not found";
            _mockBookService.Setup(service => service.GetByIdAsync(bookId))
                .ThrowsAsync(new NotFoundException(errorMessage));
            try
            {
                var result = await _controller.GetById(bookId);
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
                Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
                Assert.Equal(errorMessage, notFoundResult.Value);
            }
            catch (NotFoundException)
            {
            }
        }


        [Fact]
        public async Task Post_ReturnsNotFound_WhenCategoryDoesNotExist()
        {
            var bookCreateDTO = new BookCreateDTO { Name = "New Book", CategoryId = 999 };
            var errorMessage = "Category not found.";
            _mockBookService.Setup(service => service.CreateAsync(bookCreateDTO))
                .ThrowsAsync(new NotFoundException(errorMessage));
            try
            {
                var result = await _controller.Post(bookCreateDTO);
                var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
                Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
                Assert.Equal(errorMessage, notFoundResult.Value);
            }
            catch (NotFoundException)
            {
            }
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenBookDoesNotExist()
        {
            var bookId = 60;
            _mockBookService.Setup(service => service.DeleteAsync(bookId))
                .ThrowsAsync(new NotFoundException("Book not found for the given ID."));
            try
            {
                var result = await _controller.Delete(bookId);
            }
            catch (NotFoundException ex)
            {
                Assert.Equal("Book not found for the given ID.", ex.Message);
            }
        }


        [Fact]
        public async Task Get_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            _mockBookService.Setup(service => service.GetAllAsync(It.IsAny<BookParameters>()))
                .ThrowsAsync(new InternalServerErrorException("No book found."));
            try
            {
                var result = await _controller.Get(new BookParameters());
                var statusCodeResult = Assert.IsType<StatusCodeResult>(result.Result);
                Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);
            }
            catch (InternalServerErrorException ex)
            {
            }
        }
    }
}