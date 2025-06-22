namespace BookstoreApi.DTOs;

public class TokenDTO
{
    /// <summary>
    /// The JWT access token used for authentication.
    /// </summary>
    public string? AccessToken { get; set; }

    /// <summary>
    /// The refresh token used to obtain a new access token.
    /// </summary>
    public string? RefreshToken { get; set; }
}