using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using BookstoreApi.Controllers;
using BookstoreApi.Services.Interfaces;
using BookstoreApi.DTOs;
using BookstoreApi.Models;
using System.Threading.Tasks;
using BookstoreApi.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace BookstoreApiTests.UnitTests
{
    public class AuthControllerTest
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly AuthController _authController;

        public AuthControllerTest()
        {
            _mockAuthService = new Mock<IAuthService>();
            _authController = new AuthController(_mockAuthService.Object);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnOk_WhenLoginIsSuccessful()
        {
            var loginDTO = new LoginDTO { Email = "user@example.com", Password = "password123" };
            var loginResult = new LoginResult
            {
                Succeeded = true,
                Token = "access_token",
                RefreshToken = "refresh_token",
                Expiration = DateTime.UtcNow.AddHours(1)
            };
            _mockAuthService.Setup(service => service.LoginAsync(loginDTO)).ReturnsAsync(loginResult);
            var result = await _authController.LoginAsync(loginDTO);
            var actionResult = Assert.IsType<ActionResult<LoginResult>>(result);
            var requestResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnedResult = Assert.IsType<LoginResult>(requestResult.Value);
            Assert.Equal("access_token", returnedResult.Token);
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnOkWithErrorMessage_WhenLoginFails()
        {
            var loginDTO = new LoginDTO { Email = "user@example.com", Password = "wrong_password" };
            var loginResult = new LoginResult
            {
                Succeeded = false,
                ErrorMessage = "Email or Password invalid!"
            };
            _mockAuthService.Setup(service => service.LoginAsync(loginDTO))
                .ReturnsAsync(loginResult);
            var result = await _authController.LoginAsync(loginDTO);
            var actionResult = Assert.IsType<ActionResult<LoginResult>>(result);
            var requestResult = Assert.IsType<OkObjectResult>(actionResult.Result); // Verificando BadRequest
            var requestValue = Assert.IsType<LoginResult>(requestResult.Value);
            Assert.NotNull(requestValue);
            Assert.False(requestValue.Succeeded);
            Assert.Equal("Email or Password invalid!", requestValue.ErrorMessage);
        }


        [Fact]
        public async Task RegisterAsync_ShouldReturnCreated_WhenRegistrationIsSuccessful()
        {
            var registerDTO = new RegisterDTO
            {
                Email = "user@example.com",
                Password = "password123"
            };
            _mockAuthService.Setup(service => service.RegisterAsync(registerDTO)).Returns(Task.CompletedTask);
            var result = await _authController.RegisterAsync(registerDTO);
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status201Created, actionResult.StatusCode);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnBadRequest_WhenRegistrationFails()
        {
            var message = "";
            var registerDTO = new RegisterDTO
            {
                Email = "user@example.com",
                Password = "password123",
            };
            _mockAuthService.Setup(service => service.RegisterAsync(registerDTO))
                .ThrowsAsync(new ConflictException("Email already exists"));
            try
            {
                var result = await _authController.RegisterAsync(registerDTO);
                var actionResult = Assert.IsType<BadRequestObjectResult>(result);
                var returnedMessage = Assert.IsType<string>(actionResult.Value);
            }
            catch (ConflictException ex)
            {
                message = ex.Message;
            }

            Assert.Equal("Email already exists", message);
        }

        [Fact]
        public async Task CreateRefreshTokenAsync_ShouldReturnCreated_WhenRefreshTokenIsGenerated()
        {
            var tokenDTO = new TokenDTO { AccessToken = "access_token", RefreshToken = "refresh_token" };
            var result = new ObjectResult(new { accessToken = "new_access_token", refreshToken = "new_refresh_token" })
            {
                StatusCode = StatusCodes.Status201Created
            };
            _mockAuthService.Setup(service => service.CreateRefreshTokenAsync(tokenDTO)).ReturnsAsync(result.Value);
            var actionResult = await _authController.CreateRefreshTokenAsync(tokenDTO);
            var createdResult = Assert.IsType<ObjectResult>(actionResult);
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
        }

        [Fact]
        public async Task RevokeRefreshTokenAsync_ShouldReturnOk_WhenTokenIsRevoked()
        {
            var emailDTO = new EmailDTO { Email = "user@example.com" };
            _mockAuthService.Setup(service => service.RevokeAsync(emailDTO)).Returns(Task.CompletedTask);
            var result = await _authController.RevokeRefreshTokenAsync(emailDTO);
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnedMessage = Assert.IsType<string>(actionResult.Value);
            Assert.Equal("Refresh Token revoked successfully.", returnedMessage);
        }

        [Fact]
        public async Task RevokeRefreshTokenAsync_ShouldReturnBadRequest_WhenUserNotFound()
        {
            var emailDTO = new EmailDTO { Email = "user@example.com" };
            var message = "";
            _mockAuthService.Setup(service => service.RevokeAsync(emailDTO))
                .ThrowsAsync(new BadRequestException("User not found"));
            try
            {
                var result = await _authController.RevokeRefreshTokenAsync(emailDTO);
                var actionResult = Assert.IsType<BadRequestObjectResult>(result);
                var returnedMessage = Assert.IsType<string>(actionResult.Value);
            }
            catch (BadRequestException ex)
            {
                message = ex.Message;
            }

            Assert.Equal("User not found", message);
        }

        [Fact]
        public async Task CreateRoleAsync_ShouldReturnCreated_WhenRoleIsCreated()
        {
            var roleNameDTO = new RoleNameDTO { Name = "Admin" };
            _mockAuthService.Setup(service => service.CreateRoleAsync(roleNameDTO)).Returns(Task.CompletedTask);
            var result = await _authController.CreateRoleAsync(roleNameDTO);
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status201Created, actionResult.StatusCode);
        }

        [Fact]
        public async Task AddUserToRoleAsync_ShouldReturnOk_WhenUserIsAddedToRole()
        {
            var userRoleDTO = new UserRoleDTO { Email = "user@example.com", RoleName = "Admin" };
            _mockAuthService.Setup(service => service.AddUserRoleAsync(userRoleDTO)).Returns(Task.CompletedTask);
            var result = await _authController.AddUserToRoleAsync(userRoleDTO);
            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnedMessage = Assert.IsType<string>(actionResult.Value);
            Assert.Equal("Role added successfully.", returnedMessage);
        }
    }
}