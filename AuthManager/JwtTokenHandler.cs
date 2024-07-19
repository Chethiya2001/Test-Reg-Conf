using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthManager.Data;
using AuthManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AuthManager;

public class JwtTokenHandler
{
    public const string JWT_SECURITY_KEY = "my to secret key use to got the token";

    private const int JWT_EXPIRY_MINUTES = 20;
    private readonly AppDbContext _context;

    public JwtTokenHandler(AppDbContext userAccountsList)
    {
        _context = userAccountsList;
    }
    public async Task<AuthenticationResponse?> GenerateJSONWebTokenAsync(AuthenticationRequest authenticationRequest)
    {
        if (string.IsNullOrEmpty(authenticationRequest.UserName) || string.IsNullOrEmpty(authenticationRequest.Password))
        {
            return null;
        }

        var userAccount = await _context.Users.FirstOrDefaultAsync(x => x.UserName == authenticationRequest.UserName);

        if (userAccount == null || !BCrypt.Net.BCrypt.Verify(authenticationRequest.Password, userAccount.Password))
        {
            return null;
        }

        var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(JWT_EXPIRY_MINUTES);
        var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
        var claimsIdentity = new ClaimsIdentity(new List<Claim>
        {
            new Claim(ClaimTypes.Name, userAccount!.UserName!),
            new Claim(ClaimTypes.NameIdentifier, userAccount.Id.ToString())
        });

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(tokenKey),
            SecurityAlgorithms.HmacSha256
        );

        var securityTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = tokenExpiryTimeStamp,
            SigningCredentials = signingCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(securityTokenDescriptor);
        var token = tokenHandler.WriteToken(securityToken);

        return new AuthenticationResponse
        {
            UserId = userAccount.Id,
            UserName = userAccount.UserName,
            ExpireIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds,
            JwtToken = token
        };
    }

}


