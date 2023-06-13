using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace CityInfo.API.Controllers;

// 'ControllerBase' > 'Controller' class
// 'Controller' class contains additional helper methods to use when returning views, which isn't need in designing an API

[ApiController]
[Route("api/cities")]
public class CitiesController : ControllerBase
{
    private readonly ICityInfoRepository _cityInfoRepository;
    private readonly IMapper _mapper;
    private const int MaxCitiesPageSize = 20;

    public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
    {
        _cityInfoRepository = cityInfoRepository;
        _mapper = mapper;
    }

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<CityWithoutPointsOfInterestDto>>> GetCities(
        [FromQuery] string? name, [FromQuery] string? searchQuery, 
        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        if (pageSize > MaxCitiesPageSize)
        {
            pageSize = MaxCitiesPageSize;
        }
        var (cityEntities , paginationMetadata) = await _cityInfoRepository
            .GetCitiesAsync(name, searchQuery, pageNumber, pageSize);
        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
        return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCity([FromRoute] int id, [FromQuery] bool includePointsOfInterest = false)
    {
        var city = await _cityInfoRepository.GetCityAsync(id, includePointsOfInterest);
        
        if (city == null)
        {
            return NotFound();
        }

        if (includePointsOfInterest)
        {
            return Ok(_mapper.Map<CityDto>(city));
        }

        return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));

    }
}