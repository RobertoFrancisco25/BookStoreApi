using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BookstoreApi.DTOs;
using BookstoreApi.Exceptions;
using BookstoreApi.Models;
using BookstoreApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookstoreApi.Services;

public class AuthService : IAuthService
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthService(ITokenService tokenService, UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task<LoginResult> LoginAsync(LoginDTO login)
    {
        var user = await _userManager.FindByEmailAsync(login.Email);
        
        if (user is null || !await _userManager.CheckPasswordAsync(user, login.Password))
        {
            return  new LoginResult
            {
                Succeeded = false,
                ErrorMessage = "Email or Password invalid!"
            };
          
        }
        var userRoles = await _userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.Email!),
            new Claim("id", user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var token = _tokenService.GenerateAccessToken(authClaims, _configuration);
        var refreshToken = _tokenService.GenerateRefreshToken();
        _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"],
            out int refreshTokenValidityInMinutes);
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiration =
            DateTime.UtcNow.AddMinutes(refreshTokenValidityInMinutes);
        await _userManager.UpdateAsync(user);
           
        return  new LoginResult
        {
            Succeeded = true,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken,
            Expiration = token.ValidTo
        };
        
    }

    public async Task RegisterAsync(RegisterDTO register)
    {
        var emailExists = await _userManager.FindByEmailAsync(register.Email);
        if (emailExists is not null)
        {
            throw new ConflictException("Email already exists");
        }

        ApplicationUser user = new()
        {
            UserName = register.Email,
            Email = register.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
        };
        var result = await _userManager.CreateAsync(user, register.Password);
        if (!result.Succeeded)
        {
            throw new InternalServerErrorException("User Creation Failed!");
               
        };
        await _userManager.AddToRoleAsync(user, "User");
    }
    public async Task DeleteUserAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            throw new NotFoundException("User not found");
        }
        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Any())
        {
            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            if (!result.Succeeded)
            {
                throw new InternalServerErrorException("An error occurred while removing the user's roles.");
            }
        }
        var deleteUserResult = await _userManager.DeleteAsync(user);
        if (!deleteUserResult.Succeeded)
        {
            throw new InternalServerErrorException("User Deletion Failed!");
        }
    }
    
    public async Task<Object> CreateRefreshTokenAsync(TokenDTO token)
    {
        if (token is null)
        {
            throw new BadRequestException("Invalid client request");
        }
        string? accessToken = token.AccessToken 
                             ?? throw new ArgumentNullException(nameof(token));
        string? refreshToken = token.RefreshToken
                               ?? throw new ArgumentException(nameof(token));

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _configuration);

        if (principal is null)
        {
            throw new BadRequestException("Invalid access token/refresh token");
        }
        string email = principal.Identity.Name;
        var user = await _userManager.FindByEmailAsync(email!);

        if (user is null || user.RefreshToken != refreshToken
                         || user.RefreshTokenExpiration <= DateTime.UtcNow)
        {
                
            throw new BadRequestException("Invalid access token/refresh token");
        }
        var newAccessToken = _tokenService.GenerateAccessToken(
            principal.Claims.ToList(), _configuration);
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);
        return new ObjectResult(new
        {
            acessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            refreshToken = newRefreshToken,
        });

    }

    public async Task RevokeAsync(EmailDTO emailDto)
    {
        var user = await _userManager.FindByEmailAsync(emailDto.Email);
        if(user is null) throw new BadRequestException("User not found");
        user.RefreshToken = null;
        await _userManager.UpdateAsync(user);
    }

    public async Task CreateRoleAsync(RoleNameDTO roleName)
    {
        var roleExist = await _roleManager.FindByNameAsync(roleName.Name);
        if (roleExist is not null)
        {
           throw new ConflictException("Role already exists.");
        }
        var role = await _roleManager.CreateAsync(new IdentityRole(roleName.Name));
        
        if (!role.Succeeded)
        {
           throw new BadRequestException("Role Creation Failed.");
        }
        
    }

    public async Task AddUserRoleAsync(UserRoleDTO userRole)
    {
        var user = await _userManager.FindByEmailAsync(userRole.Email);
        if (user is null)
        {
            throw new BadRequestException("User not found");
        }
        if (await _userManager.IsInRoleAsync(user, userRole.RoleName))
        {
            throw new BadRequestException($"User {user.Email} is already in role '{userRole.RoleName}'");
        }
        var result = await _userManager.AddToRoleAsync(user, userRole.RoleName);
        if (!result.Succeeded)
        {
            throw new BadRequestException($"Failed to add user {user.Email} to role '{userRole.RoleName}'");
        }
    }



}