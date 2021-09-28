using RoamlerLocationSearch.Domain.Entities;
using RoamlerLocationSearch.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoamlerLocationSearch.Business.Extensions;

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
            List<Location> Locations = new List<Location>();
            try
            {
                //Get all Locations
                
                Locations = _LocationDataAccess.GetLocations();

                // calculate distance and add to the list for each location, filter by max location, order by location and use max results
                Locations = Locations.Select(c => 
                                        {
                                            c.Distance = c.CalculateDistance(location);
                                            if(maxDistance >= c.Distance)
                                            {
                                                return c;
                                            }
                                            return null;
                                        })
                                        .Where(c => c != null)
                                        .OrderBy(c => c.Distance)
                                        .Take(maxResults)
                                        .ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Locations;
        }

        public List<Location> GetLocationsParallel(Location location, int maxDistance, int maxResults)
        {
            List<Location> Locations = new List<Location>();
            try
            {
                //Get all Locations
                Locations = _LocationDataAccess.GetLocations();

                // get distance and add to the list for each location
                Locations = Locations.AsParallel().WithDegreeOfParallelism(Environment.ProcessorCount)
                                            .Select(c =>
                                            {
                                                c.Distance = c.CalculateDistance(location);
                                                if (maxDistance >= c.Distance)
                                                {
                                                    return c;
                                                }
                                                return null;
                                            })
                                            .Where(c => c != null)
                                            .OrderBy(c => c.Distance)
                                            .Take(maxResults)
                                            .ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Locations;
        }

        public async Task<List<Location>> GetLocationsAsync(Location location, int maxDistance, int maxResults)
        {
            List<Location> Locations = new List<Location>();
            try
            {
                Locations = await _LocationDataAccess.GetLocationsAsync();

                Locations = Locations.Select(c =>
                                            {
                                                c.Distance = c.CalculateDistance(location);
                                                if (maxDistance >= c.Distance)
                                                {
                                                    return c;
                                                }
                                                return null;
                                            })
                                            .Where(c => c != null)
                                            .OrderBy(c => c.Distance)
                                            .Take(maxResults)
                                            .ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Locations;
        }
    }
}
