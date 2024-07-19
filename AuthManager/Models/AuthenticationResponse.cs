namespace AuthManager.Models;

public class AuthenticationResponse
{
    public Guid UserId { get; set; }
    public string? UserName { get; set; }

    public string? JwtToken { get; set; }

    public int? ExpireIn { get; set; }
}
