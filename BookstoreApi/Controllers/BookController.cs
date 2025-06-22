using AutoMapper;
using BookstoreApi.Data;
using BookstoreApi.DTOs;
using BookstoreApi.Helpers;
using BookstoreApi.Models;
using BookstoreApi.Parameters;
using BookstoreApi.Repositories.Interfaces;
using BookstoreApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace BookstoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }
        /// <summary>
        /// Returns a list of books.
        /// </summary>
        /// <returns>A list of books.</returns>
        [HttpGet]
        public async Task<ActionResult<List<BookDTO>>> Get([FromQuery] BookParameters parameters)
        {
            var pagedList = await _bookService.GetAllAsync(parameters);
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(pagedList.Metadata));
            return Ok(pagedList.Items);
        }
        /// <summary>
        /// Returns the book for the given ID.
        /// </summary>
        /// <param name="id">The ID of the book.</param>
        /// <returns>The book matching the specified ID.</returns>
        [HttpGet("Category/{id:int}")]
        public async Task<ActionResult<List<BookDTO>>> GetByCategoryId(int id, [FromQuery] BookParameters parameters)
        {
            var pagedList = await _bookService.GetByCategoryId(id, parameters);
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(pagedList.Metadata));
            return Ok(pagedList.Items);
        }
        /// <summary>
        /// Returns a list of books that belong to a specific category.
        /// </summary>
        /// <param name="categoryId">The ID of the category.</param>
        /// <returns>A list of books in the specified category.</returns>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookDTO>> GetById(int id)
        {
            var bookDTO = await _bookService.GetByIdAsync(id);
            return Ok(bookDTO);
        }
        /// <summary>
        /// Adds a new book.
        /// </summary>
        /// <param name="book">The book object to add.</param>
        /// <returns>The added book.</returns>
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Post([FromBody] BookCreateDTO bookDTO)
        {
            var book = await _bookService.CreateAsync(bookDTO);
            return CreatedAtAction(nameof(GetById), new { id = book.Id }, bookDTO);
        }
        /// <summary>
        /// Updates a book by the given ID.
        /// </summary>
        /// <param name="id">The ID of the book to update.</param>
        /// <param name="book">The updated book object.</param>
        /// <returns>The updated book.</returns>
        [HttpPut("{id:int}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Put(int id, [FromBody] BookCreateDTO bookDTO)
        {
            await _bookService.UpdateAsync(id, bookDTO);
            return Ok(bookDTO);
        }
        /// <summary>
        /// Deletes a book by the given ID.
        /// </summary>
        /// <param name="id">The ID of the book to delete.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _bookService.DeleteAsync(id);
            return NoContent();
        }
    }
}