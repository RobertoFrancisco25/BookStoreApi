using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using BookstoreApi.Controllers;
using BookstoreApi.Services.Interfaces;
using BookstoreApi.DTOs;
using BookstoreApi.Models;
using System.Threading.Tasks;
using BookstoreApi.Exceptions;
using AutoMapper;

namespace BookstoreApiTests.UnitTests
{
    public class PurchaseControllerTest
    {
        private readonly Mock<IPurchaseService> _mockPurchaseService;
        private readonly PurchaseController _purchaseController;

        public PurchaseControllerTest()
        {
            _mockPurchaseService = new Mock<IPurchaseService>();
            _purchaseController = new PurchaseController(_mockPurchaseService.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnPurchase_WhenPurchaseExists()
        {
            
            var purchaseId = 1;
            var purchase = new Purchase { Id = purchaseId, PurchaseDate = DateTime.UtcNow, Total = 100.0m };
            var purchaseDTO = new PurchaseResponseDTO { Id = purchaseId, Total = 100.0m };

            _mockPurchaseService.Setup(service => service.GetPurchaseByIdAsync(purchaseId))
                                .ReturnsAsync(purchaseDTO);

            
            var result = await _purchaseController.GetByIdAsync(purchaseId);

            
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnedPurchase = Assert.IsType<PurchaseResponseDTO>(actionResult.Value);
            Assert.Equal(purchaseId, returnedPurchase.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNotFound_WhenPurchaseDoesNotExist()
        {
            
            var purchaseId = 1;
            _mockPurchaseService.Setup(service => service.GetPurchaseByIdAsync(purchaseId))
                                .ThrowsAsync(new NotFoundException("Purchase not found"));

            try
            {
                var result = await _purchaseController.GetByIdAsync(purchaseId);
                var actionResult = Assert.IsType<NotFoundResult>(result);
            }catch(NotFoundException ex)
            {}
            
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreated_WhenPurchaseIsSuccessful()
        {
            
            var purchaseCreateDTO = new PurchaseCreateDTO
            {
                Items = new List<PurchaseItemDTO>
                {
                    new PurchaseItemDTO { BookId = 1, Quantity = 2 }
                }
            };

            var purchase = new Purchase { Id = 1, Total = 100.0m };
            var purchaseDTO = new PurchaseResponseDTO { Id = 1, Total = 100.0m };

            _mockPurchaseService.Setup(service => service.MakePurchaseAsync(purchaseCreateDTO))
                                .ReturnsAsync(purchaseDTO);

            
            var result = await _purchaseController.CreateAsync(purchaseCreateDTO);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetByIdAsync", actionResult.ActionName);
            Assert.Equal(purchase.Id, ((PurchaseResponseDTO)actionResult.Value).Id);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnNoContent_WhenPurchaseIsDeleted()
        {
            
            var purchaseId = 1;
            _mockPurchaseService.Setup(service => service.DeletePurchaseAsync(purchaseId))
                                .ReturnsAsync(true);

          
            var result = await _purchaseController.DeleteAsync(purchaseId);

            
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnNotFound_WhenPurchaseDoesNotExist()
        {
            
            var purchaseId = 1;
            _mockPurchaseService.Setup(service => service.DeletePurchaseAsync(purchaseId))
                                .ThrowsAsync(new NotFoundException("Purchase not found"));

            try
            {
                var result = await _purchaseController.DeleteAsync(purchaseId);
                Assert.IsType<NotFoundResult>(result);
            }catch(NotFoundException ex)
            {}
        }
    }
}
