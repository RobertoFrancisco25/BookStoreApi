namespace BookstoreApi.Models;

public class LoginResult
{
     public bool Succeeded  { get; set; }
     public string Token { get; set; }
     public string RefreshToken { get; set; }
     public DateTime Expiration { get; set; }
     public string? ErrorMessage { get; set; }
}