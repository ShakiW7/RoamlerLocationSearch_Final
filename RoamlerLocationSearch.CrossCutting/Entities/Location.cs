using System;
using System.Collections.Generic;
using System.Text;

namespace RoamlerLocationSearch.Domain.Entities
{
    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; }
        public double Distance { get; set; }
    }
}
