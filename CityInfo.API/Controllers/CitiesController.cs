using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

// 'ControllerBase' > 'Controller' class
// 'Controller' class contains additional helper methods to use when returning views, which isn't need in designing an API

[ApiController]
[Route("api/cities")]
public class CitiesController : ControllerBase
{
    [HttpGet()]
    public ActionResult<IEnumerable<CityDto>> GetCities()
    {
       return Ok(CitiesDataStore.Current.Cities);
    }

    [HttpGet("{id}")]
    public ActionResult<CityDto> GetCity(int id)
    {
        var cityToReturn =
            CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);

        if (cityToReturn == null)
        {
            return NotFound();
        }

        return Ok(cityToReturn);
    }
}