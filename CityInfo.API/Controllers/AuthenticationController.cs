using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CityInfo.API.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthenticationController(IConfiguration configuration)
    {
        
        _configuration = configuration ?? throw new ArgumentException(nameof(configuration)); ;
    }

    [HttpPost("authenticate")]
    public ActionResult<string> Authenticate([FromBody] UserCredentialsDto userCredentials)
    {
        var user = ValidateUserCredentials(userCredentials.UserName, userCredentials.Password);

        if (user == null)
        {
            
            return Unauthorized();
        }

        var securityKey =
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:SecretForKey"]!));

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claimsForToken = new List<Claim>
        {
            new Claim("sub", user.UserId.ToString()),
            new Claim("given_name", user.FirstName),
            new Claim("family_name", user.FirstName),
            new Claim("city", user.City)
        };

        var jsonWebToken = new JwtSecurityToken(
            _configuration["Authentication:Issuer"],
            _configuration["Authentication:Audience"],
            claimsForToken,
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(1),
            signingCredentials);

        var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jsonWebToken);

        return Ok(tokenToReturn);
    }

    private User ValidateUserCredentials(string userName, string password)
    {
        return new User(1, userName ?? "Matteo", "Mati", "Ha", "Antwerp");

    }
}