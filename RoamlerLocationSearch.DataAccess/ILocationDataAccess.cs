using RoamlerLocationSearch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoamlerLocationSearch.DataAccess
{
    public interface ILocationDataAccess
    {
        List<Location> GetLocations();
    }
}
