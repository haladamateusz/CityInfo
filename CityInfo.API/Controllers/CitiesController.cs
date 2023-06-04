using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers;

public class CitiesController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}