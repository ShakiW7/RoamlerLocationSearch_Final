using RoamlerLocationSearch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoamlerLocationSearch.Business
{
    public interface ILocationSearchService
    {
        List<Location> GetLocationsParallel(Location location, int maxDistance, int maxResults);
        List<Location> GetLocations(Location location, int maxDistance, int maxResults);
    }
}
