using RoamlerLocationSearch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RoamlerLocationSearch.DataAccess
{
    public interface ILocationDataAccess
    {
        List<Location> GetLocations();

        Task<List<Location>> GetLocationsAsync();
    }
}
