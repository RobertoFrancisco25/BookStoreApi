using AutoMapper;
using BookstoreApi.Data;
using BookstoreApi.DTOs;
using BookstoreApi.Helpers;
using BookstoreApi.Models;
using BookstoreApi.Pagination;
using BookstoreApi.Parameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookstoreApi.Repositories;
using BookstoreApi.Repositories.Interfaces;
using BookstoreApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;


namespace BookstoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        /// <summary>
        ///  Returns all registered categories.
        /// </summary>
        /// <returns>List of categories.</returns>
        [HttpGet]
        public async Task<ActionResult<List<CategoryDTO>>> GetAllAsync([FromQuery] CategoryParameters parameters)
        {
            var pagedResult = await _categoryService.GetAllAsync(parameters);
            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(pagedResult.Metadata));
            return Ok(pagedResult.Items);
        }
        /// <summary>
        /// Returns the category for the given ID.
        /// </summary>
        /// <param name="id">The ID of the category.</param>
        /// <returns>The category matching the specified ID.</returns>
        [HttpGet("{id:int}")]
        [ActionName("GetByIdAsync")]
        public async Task<ActionResult<CategoryDTO>> GetByIdAsync(int id)
        {
            var categoryDTO = await _categoryService.GetByIdAsync(id);
            return Ok(categoryDTO);
        }
        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="Name">The name of the category to create.</param>
        /// <returns>The created category.</returns>
        [HttpPost]
        [Authorize(Policy = "Admin")]
        public async Task<ActionResult<CategoryDTO>> PostAsync([FromBody] CategoryCreateDTO categoryDTO)
        {
            var createCategory = await _categoryService.CreateAsync(categoryDTO);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = createCategory.Id }, categoryDTO);
        }
        /// <summary>
        /// Updates an existing category.
        /// </summary>
        /// <param name="id">The ID of the category to update.</param>
        /// <param name="Name">The new name for the category.</param>
        /// <returns>The updated category.</returns>
        [HttpPut("{id:int}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] CategoryCreateDTO categoryDTO)
        {
            await _categoryService.UpdateAsync(id, categoryDTO);
            return NoContent();
        }
        /// <summary>
        /// Deletes a category by the given ID.
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>No content.</returns> 
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _categoryService.DeleteAsync(id);
            return NoContent();
        }
    }
}