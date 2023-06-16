namespace CityInfo.API.Models;

/// <summary>
/// A DTO for a city without including points of interest
/// </summary>
public class CityWithoutPointsOfInterestDto
{
    public int Id { get; set; }

    /// <summary>
    /// the name of the city
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The description of the city
    /// </summary>
    public string? Description { get; set; }

}