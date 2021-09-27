using RoamlerLocationSearch.Domain.Entities;
using RoamlerLocationSearch.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoamlerLocationSearch.Business
{
    public class LocationSearchService : ILocationSearchService
    {
        private readonly ILocationDataAccess _LocationDataAccess;

        public LocationSearchService(ILocationDataAccess locationDataAccess)
        {
            _LocationDataAccess = locationDataAccess;
        }

        public List<Location> GetLocations(Location location, int maxDistance, int maxResults)
        {
            List<Location> filteredList = new List<Location>();
            try
            {
                //Get all Locations
                List<Location> Locations = new List<Location>();
                Locations = _LocationDataAccess.GetLocations();


                //Sort by Distance
                List<Location> SortedList = Locations.ToList();//.OrderBy(o => o.CalculateDistance(pLocation)).ToList(); // do the orderby

                //Filter the Locations with the same Distance, Longitude and Latitude
                List<Location> filterRepeated = SortedList.GroupBy(x => new { x.Distance, x.Longitude, x.Latitude })
                                                   .Select(g => g.First())
                                                   .ToList();

                //Filter by the max Number of Results
                filteredList = filterRepeated.Where(x => x.Distance <= maxDistance).Take(maxResults).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return filteredList;
        }

        public List<Location> GetLocationsParallel(Location location, int maxDistance, int maxResults)
        {
            List<Location> filteredList = new List<Location>();
            try
            {
                //Get all Locations
                List<Location> Locations = new List<Location>();
                Locations = _LocationDataAccess.GetLocations();


                //Sort by Distance
                //Added Parallelism
                List<Location> SortedList = Locations.AsParallel()
                                                    .WithDegreeOfParallelism(Environment.ProcessorCount)
                                                   // .OrderBy(o => o.CalculateDistance(pLocation))
                                                    .ToList();

                //Filter the Locations with the same Distance, Longitude and Latitude
                List<Location> filterRepeated = SortedList.AsParallel()
                                                    .WithDegreeOfParallelism(Environment.ProcessorCount)
                                                    .GroupBy(x => new { x.Distance, x.Longitude, x.Latitude })
                                                    .Select(g => g.First())
                                                    .ToList();

                //Filter by the max Number of Results
                filteredList = filterRepeated.Where(x => x.Distance <= maxDistance).Take(maxResults).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return filteredList;
        }

        public async Task<List<Location>> GetLocationsAsync(Location pLocation, int maxDistance, int maxResults)
        {
            List<Location> filteredList = new List<Location>();
            try
            {
                List<Location> Locations = await _LocationDataAccess.GetLocationsAsync();

                //Sort by Distance
                List<Location> SortedList = Locations;//.OrderBy(o => o.CalculateDistance(pLocation)).ToList();

                //Filter the Locations with the same Distance, Longitude and Latitude
                List<Location> filterRepeated = SortedList.GroupBy(x => new { x.Distance, x.Longitude, x.Latitude })
                                                   .Select(g => g.First())
                                                   .ToList();

                //Filter by the max Number of Results
                filteredList = filterRepeated.Where(x => x.Distance <= maxDistance).Take(maxResults).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return filteredList;
        }
    }
}
