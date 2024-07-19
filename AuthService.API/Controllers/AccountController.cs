using AuthManager;
using AuthManager.Data;
using AuthManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthService.API.Controllers;

[ApiController]
[Route("[controller]")]

public class AccountController : ControllerBase
{
   private readonly JwtTokenHandler _jwtTokenHandler;
   private readonly AppDbContext appDbContext;

   public AccountController(JwtTokenHandler jwtTokenHandler, AppDbContext appDbContext)
   {
      _jwtTokenHandler = jwtTokenHandler;
      this.appDbContext = appDbContext;
   }

   [HttpPost("register")]
   public async Task<IActionResult> Register([FromBody] AuthenticationRequest request)
   {
      if (request == null || string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
      {
         return BadRequest("Invalid registration request");
      }

      var isExUser = await appDbContext.Users.FirstOrDefaultAsync(x => x.UserName == request.UserName);
      if (isExUser != null)
      {
         return BadRequest("User already exists");
      }

      // Hash the password
      var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

      // New user
      var userAccount = new UserAccount
      {
         UserName = request.UserName,
         Password = hashedPassword,
      };

      appDbContext.Users.Add(userAccount);
      await appDbContext.SaveChangesAsync();

      // Generate JWT token
      var authRequest = new AuthenticationRequest
      {
         UserName = request.UserName,
         Password = request.Password
      };

      var authResponse = await _jwtTokenHandler.GenerateJSONWebTokenAsync(authRequest);

      if (authResponse == null)
      {
         return BadRequest("Failed to generate token");
      }

      return Ok(authResponse);
   }
}



