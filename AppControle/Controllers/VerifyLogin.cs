using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AppControle.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AppControle.Controllers;

[Route("api/verifyLogin")]
[ApiController]
public class verifyLoginController : ControllerBase
{
    private readonly AcessControlContext _context;

    public verifyLoginController(AcessControlContext context)
    {
        _context = context;
    }

    //chave de segurança do JWT
    private readonly string secretKey = "IVM8763Z5J3JTUS1EG7QHRRCYEZOV8I8";

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginModel model)
    {
        var user = ValidateUser(model.Email, model.Password);
        if (user == null)
        {
            return Unauthorized();
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(secretKey);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim("UserId", user.Id.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            Issuer = "https://localhost:7066",
            Audience = "http://localhost:5173"
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new { token = tokenString });
    }

    private User ValidateUser(string email, string password)
    {
        var user = _context.User.SingleOrDefault(u => u.Email == email);

        if (user != null && PasswordHash.VerifyPassword(password, user.Password) == true)
        {
            return user;
        }

        return null;
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    [Authorize]
    [HttpGet("is-logged-in")]
    public IActionResult IsLoggedIn()
    {
        return Ok(new { IsLoggedIn = true });
    }

    [HttpGet("loginstatus")]
    public IActionResult LoginStatus()
    {
        if (User.Identity.IsAuthenticated)
        {
            return Ok(new { IsLoggedIn = true });
        }
        else
        {
            return Ok(new { IsLoggedIn = false });
        }
    }
}