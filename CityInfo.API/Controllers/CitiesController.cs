using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

// Controller class contains additional helper methods to use when returning views, which isn't need in designing an API

[ApiController]
[Route("api/cities")]
public class CitiesController : ControllerBase
{
    [HttpGet()]
    public JsonResult GetCities()
    {
       return new JsonResult(
            new List<object>
            {
                new
                {
                    id = 1,
                    name = "New York City"
                },
                new
                {
                    id = 2,
                    name = "Antwerp"
                }
            });
    }
}