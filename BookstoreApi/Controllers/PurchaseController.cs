using BookstoreApi.Data;
using BookstoreApi.DTOs;
using BookstoreApi.Exceptions;
using BookstoreApi.Repositories.Interfaces;
using BookstoreApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseService _purchaseSevice;


        public PurchaseController(IPurchaseService purchaseService)
        {
            _purchaseSevice = purchaseService;
        }

       /// <summary>
        /// Returns the purchase for the given ID.
        /// </summary>
        /// <param name="id">The ID of the purchase.</param>
        /// <returns>The purchase matching the specified ID.</returns>
        [HttpGet("{id:int}")]
        [ActionName("GetByIdAsync")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var purchase = await _purchaseSevice.GetPurchaseByIdAsync(id);
            return Ok(purchase);
        }
        /// <summary>
        /// Performs a book purchase.
        /// </summary>
        /// <param name="purchaseItems">The purchase request details.</param>
        /// <returns>The result of the purchase operation.</returns>
        [HttpPost]
        [Authorize(Policy = "User")]
        public async Task<IActionResult> CreateAsync([FromBody] PurchaseCreateDTO purchaseCreateDTO)
        {
            var purchase = await _purchaseSevice.MakePurchaseAsync(purchaseCreateDTO);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = purchase.Id }, purchase);

        }
        /// <summary>
        /// Deletes a purchase by the given ID.
        /// </summary>
        /// <param name="id">The ID of the purchase to delete.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id:int}")]
        [Authorize(Policy = "Admin")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await  _purchaseSevice.DeletePurchaseAsync(id);

            return  NoContent();
        }
       
       
    }
}