using RoamlerLocationSearch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoamlerLocationSearch.Domain.Models
{
    public class SearchResult
    {
        public List<Location> Locations { get; set; }
        public int MaxResults { get; set; }
        public string Error { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int RecordCount { get; set; }
        public int MaxDistance { get; set; }
        public double TotalDuration { get; set; } 
    }
}
