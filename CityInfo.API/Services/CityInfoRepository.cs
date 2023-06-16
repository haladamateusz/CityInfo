using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services;

public class CityInfoRepository: ICityInfoRepository
{
    private readonly CityInfoContext _context;

    public CityInfoRepository(CityInfoContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<City>> GetCitiesAsync()
    {
        return await _context.Cities.OrderBy(c => c.Name).ToListAsync();
    }

    public async Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(string? name, string? searchQuery,
        int pageNumber, int pageSize)
    {

        var collection = _context.Cities as IQueryable<City>;

        if (!string.IsNullOrEmpty(name))
        {
            name = name.Trim();
            collection = collection.Where(c => c.Name == name);
        }

        if (!string.IsNullOrEmpty(searchQuery))
        {
            searchQuery = searchQuery.Trim();
            collection = collection.Where(c => c.Name.Contains(searchQuery) || (c.Description != null && c.Description.Contains(searchQuery)));
        }

        var totalItemCount = await collection.CountAsync();

        var paginationMetaData = new PaginationMetadata(
            totalItemCount, pageSize, pageNumber);

        // KEY  WORD: Deferred Execution
        // IQueryable is executed on foreach loop, ToList(), ToArray(), ToDictionary(), singleton queries
        var collectionToReturn = await _context.Cities.OrderBy(c => c.Name)
            .Skip(pageSize * (pageNumber -1 ))
            .Take(pageSize).ToListAsync();

        return (collectionToReturn, paginationMetaData);
    }

    public async Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest = false)
    {
        if (includePointsOfInterest)
        {
            return await _context.Cities.Include(c=> c.PointsOfInterest).Where(c => c.Id == cityId).
                FirstOrDefaultAsync();
        }
        return await _context.Cities.Where(c => c.Id == cityId).
            FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId)
    {
        return await _context.PointsOfInterests.OrderBy(c => c.Name).Where(p => p.CityId == cityId).ToListAsync();
    }

    public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
    {
        return await _context.PointsOfInterests.Where(p => p.Id == pointOfInterestId && p.CityId == cityId).
            FirstOrDefaultAsync();
    }

    public async Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest)
    {
        var city = await GetCityAsync(cityId);
        if (city != null)
        {
            city.PointsOfInterest.Add(pointOfInterest);
        }
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync() >= 0);
    }

    public async Task<bool> CityNameMatchesCityId(string? cityName, int cityId)
    {
        return await _context.Cities.AnyAsync(c => c.Id == cityId && c.Name == cityName);
    }

    public void DeletePointOfInterestForCityAsync(PointOfInterest pointOfInterest)
    {
        _context.PointsOfInterests.Remove(pointOfInterest);
    }

    public async Task<bool> CityExistsAsync(int cityId)
    {
        return await _context.Cities.AnyAsync(c => c.Id == cityId);
    }


}