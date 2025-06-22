using BookstoreApi.Controllers;
using BookstoreApi.DTOs;
using BookstoreApi.Helpers;
using BookstoreApi.Models;
using BookstoreApi.Parameters;
using BookstoreApi.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;

namespace BookstoreApiTests.UnitTests
{
    public class CategoryControllerTest
    {
        private readonly Mock<ICategoryService> _mockService;
        private readonly CategoryController _controller;
        private readonly Mock<HttpContext> _mockHttpContext; 
        private readonly Mock<HttpResponse> _mockHttpResponse; 
        private readonly HeaderDictionary _responseHeaders;
        public CategoryControllerTest()
        {
            _mockService = new Mock<ICategoryService>();
            _mockHttpContext = new Mock<HttpContext>();
            _mockHttpResponse = new Mock<HttpResponse>();
            _responseHeaders = new HeaderDictionary();
            _mockHttpResponse.SetupGet(r => r.Headers).Returns(_responseHeaders); 
            _mockHttpContext.SetupGet(c => c.Response).Returns(_mockHttpResponse.Object);
            _controller = new CategoryController(_mockService.Object);
            _controller.ControllerContext = new ControllerContext()
            {
                HttpContext = _mockHttpContext.Object
            };
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOkWithListOfCategories()
        {
            var categories = new List<CategoryDTO>
            {
                new CategoryDTO { Id = 1, Name = "Romance" },
                new CategoryDTO { Id = 2, Name = "Terror" }
            };
            var metadata = new PaginationMetadata
            {
                CurrentPage = 1,
                PageSize = 2,
                TotalCount = 4,
                TotalPages = 2,
                HasPrevious = false,
                HasNext = true
            };
            var pagedResult = new PagedResult<CategoryDTO>
            {
                Items = categories,
                Metadata = metadata
            };
            _mockService.Setup(s => s.GetAllAsync(It.IsAny<CategoryParameters>()))
                .ReturnsAsync(pagedResult);
            var categoryParameters = new CategoryParameters
            {
                PageNumber = 1,
                PageSize = 50
            };
            var result = await _controller.GetAllAsync(categoryParameters);
            var actionResult = Assert.IsType<ActionResult<List<CategoryDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedCategories = Assert.IsAssignableFrom<List<CategoryDTO>>(okResult.Value);
            Assert.Equal(2, returnedCategories.Count);
            Assert.Equal(categories[0].Name, returnedCategories[0].Name);
            Assert.True(_responseHeaders.ContainsKey("X-Pagination"));
            var headerValue = _responseHeaders["X-Pagination"].ToString();
            var deserializedMetadata = JsonConvert.DeserializeObject<PaginationMetadata>(headerValue);
            Assert.Equal(metadata.TotalCount, deserializedMetadata.TotalCount);
            Assert.Equal(metadata.TotalPages, deserializedMetadata.TotalPages);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOkWithCategory()
        {
            var category = new CategoryDTO { Id = 1, Name = "Suspense" };
            _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(category);
            var result = await _controller.GetByIdAsync(1);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedCategory = Assert.IsType<CategoryDTO>(okResult.Value);
            Assert.Equal("Suspense", returnedCategory.Name);
        }

        [Fact]
        public async Task PostAsync_ReturnsCreatedAtAction()
        {
            var createDTO = new CategoryCreateDTO { Name = "Aventura" };
            var createdCategory = new Category { Id = 10, Name = "Aventura" };
            _mockService.Setup(s => s.CreateAsync(createDTO)).ReturnsAsync(createdCategory);
            var result = await _controller.PostAsync(createDTO);
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(nameof(_controller.GetByIdAsync), createdResult.ActionName);
            Assert.Equal(createdCategory.Id, createdResult.RouteValues["id"]);
            var returnedValue = Assert.IsType<CategoryCreateDTO>(createdResult.Value);
            Assert.Equal(createDTO.Name, returnedValue.Name);
        }

        [Fact]
        public async Task PutAsync_ReturnsNoContent()
        {
            var updateDTO = new CategoryCreateDTO { Name = "Atualizada" };
            _mockService.Setup(s => s.UpdateAsync(1, updateDTO)).Returns(Task.CompletedTask);
            var result = await _controller.PutAsync(1, updateDTO);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsNoContent()
        {
            _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);
            var result = await _controller.DeleteAsync(1);
            Assert.IsType<NoContentResult>(result);
        }
    }
}

