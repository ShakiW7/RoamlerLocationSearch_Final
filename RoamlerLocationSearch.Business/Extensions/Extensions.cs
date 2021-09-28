using RoamlerLocationSearch.Domain.Entities;
using System;

namespace RoamlerLocationSearch.Business.Extensions
{
    public static class Extensions
    {
        public static double CalculateDistance(this Location location1, Location location2)
        {
            var rlat1 = Math.PI * location1.Latitude / 180;
            var rlat2 = Math.PI * location2.Latitude / 180;
            var rlon1 = Math.PI * location1.Longitude / 180;
            var rlon2 = Math.PI * location2.Longitude / 180;
            var theta = location1.Longitude - location2.Longitude;
            var rtheta = Math.PI * theta / 180;
            var dist = Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;

            return dist * 1609.344;
        }
    }
}
