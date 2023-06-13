using CityInfo.API.Entities;
using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    [HttpPost("authenticate")]
    public ActionResult<string> Authenticate([FromBody] UserCredentialsDto userCredentials)
    {
        var user = ValidateUserCredentials(userCredentials.UserName, userCredentials.Password);

        if (user == null)
        {
            return Unauthorized();
        }
    }

    private User ValidateUserCredentials(string userName, string password)
    {
        return new User(1, userName ?? "Matteo", "Mati", "Ha", "Zurich");

    }
}