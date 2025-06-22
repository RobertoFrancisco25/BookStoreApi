using BookstoreApi.DTOs;
using BookstoreApi.Models;
using BookstoreApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    /// <summary>
    /// Authenticates a user and returns an access token along with a refresh token.
    /// </summary>
    /// <param name="loginDTO">The login credentials containing email and password.</param>
    /// <returns>An object containing the access token and refresh token.</returns>
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<LoginResult>> LoginAsync([FromBody] LoginDTO login)
    {
        var result = await _authService.LoginAsync(login);
        return Ok(result);
    }
    /// <summary>
    /// Registers a new user account.
    /// </summary>
    /// <param name="registerDTO">The registration details for the new user.</param>
    /// <returns>The created user information.</returns>
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterAsync([FromBody]RegisterDTO register)
    {
        await _authService.RegisterAsync(register);
        
        return StatusCode(StatusCodes.Status201Created, new ResponseDTO
        {
            Status = StatusCodes.Status201Created,
            Message = "User created successfully."
        });
    }
    /// <summary>
    /// Generates a new access token using the provided refresh token.
    /// </summary>
    /// <param name="tokenDTO">The token data transfer object containing the refresh token.</param>
    /// <returns>A new access token and refresh token pair.</returns>
    [HttpPost]
    [Route("refresh-token")]
    public async Task<ActionResult> CreateRefreshTokenAsync([FromBody] TokenDTO token)
    {
       var result = await _authService.CreateRefreshTokenAsync(token);
       return StatusCode(StatusCodes.Status201Created, result);
    }
    /// <summary>
    /// Revokes the refresh token associated with the specified user email.
    /// </summary>
    /// <param name="emailDTO">The email of the user whose refresh token will be revoked.</param>
    /// <returns>No content.</returns>
    [HttpPost]
    [Authorize(Policy = "Admin")]
    [Route("revoke-token")]
    public async Task<IActionResult> RevokeRefreshTokenAsync([FromBody] EmailDTO email)
    {
        await _authService.RevokeAsync(email);
        return Ok("Refresh Token revoked successfully.");
      
    }
    /// <summary>
    /// Creates a new role.
    /// </summary>
    /// <param name="roleNameDTO">The role name details.</param>
    /// <returns>The created role information.</returns>
    [HttpPost]
    [Authorize(Policy = "Admin")]
    [Route("create-role")]
    public async Task<IActionResult> CreateRoleAsync([FromBody]RoleNameDTO roleName)
    {
        await _authService.CreateRoleAsync(roleName);
        return StatusCode(StatusCodes.Status201Created, new ResponseDTO
        {
            Status = StatusCodes.Status201Created,
            Message = "Role created successfully."
        });
    }
    /// <summary>
    /// Assigns a user to a specified role.
    /// </summary>
    /// <param name="userRoleDTO">The user and role information.</param>
    /// <returns>No content.</returns>
    [HttpPost]
    [Authorize(Policy = "Admin")]
    [Route("add-user-role")]
    public async Task<IActionResult> AddUserToRoleAsync([FromBody]UserRoleDTO userRole)
    {
        await _authService.AddUserRoleAsync(userRole);
        return Ok("Role added successfully.");
    }
}