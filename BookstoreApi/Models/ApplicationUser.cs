using Microsoft.AspNetCore.Identity;

namespace BookstoreApi.Models;

public class ApplicationUser : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }
}