using BookstoreApi.DTOs;
using BookstoreApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreApi.Services.Interfaces;

public interface IAuthService
{
    public Task<LoginResult> LoginAsync(LoginDTO login);
    public Task RegisterAsync(RegisterDTO register);
    public Task<Object> CreateRefreshTokenAsync(TokenDTO token);
    public Task RevokeAsync(EmailDTO emaiolDTO);
    public Task CreateRoleAsync(RoleNameDTO roleName);
    public Task AddUserRoleAsync(UserRoleDTO userRole);
}