using Microsoft.Extensions.Caching.Memory;
using RoamlerLocationSearch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoamlerLocationSearch.DataAccess
{
    public class CsvLocationDataAccess: ILocationDataAccess
    {
        private IMemoryCache _cache;

        public CsvLocationDataAccess(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public List<Location> GetLocations()
        {
            try
            {
                List<Location> Locations = new List<Location>();
                Locations = getAllLocationsCacheStreamReader();

                return Locations;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // cache + File Read + Parallel
        private List<Location> getAllLocationsCacheFileReadParallel()
        {
            try
            {
                List<Location> Locations = new List<Location>();

                if (!_cache.TryGetValue("LocationsList", out Locations))
                {
                    string fileLocation = "C:\\Users\\chath\\source\\repos\\LocationSearch.DataAccess\\Resources\\locations(5).csv";
                    string[] AllLines = File.ReadAllLines(fileLocation);

                    var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * 4 };

                    Parallel.For(0, AllLines.Length, options, i =>
                    {
                        string[] data = AllLines[i].Split(new[] { "\",\"" }, StringSplitOptions.RemoveEmptyEntries);
                        string Address = data.ElementAt(0) == null ? "" : data.ElementAt(0).TrimStart('\"');
                        double Latitude = data.Length > 1 ? double.Parse(data.ElementAt(1)) : 0.0;
                        double Longitude = data.Length > 2 ? double.Parse(data.ElementAt(2).TrimEnd('\"')) : 0.0;

                        Locations.Add(new Location()
                        {
                            Address = Address,
                            Latitude = Latitude,
                            Longitude = Longitude,
                            Distance = 0
                        });
                    });

                    // Set cache options.
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        // Keep in cache for this time, reset time if accessed.
                        .SetSlidingExpiration(TimeSpan.FromMinutes(3));

                    // Save data in cache.
                    _cache.Set("LocationsList", Locations, cacheEntryOptions);
                }
                return Locations;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // cache + stream + while
        private List<Location> getAllLocationsCacheStreamReader()
        {
            try
            {
                List<Location> Locations = new List<Location>();

                if (!_cache.TryGetValue("LocationsList", out Locations))
                {
                    string fileLocation = "C:\\Users\\chath\\source\\repos\\LocationSearch.DataAccess\\Resources\\locations(5).csv";
                    using (StreamReader sr = new StreamReader(fileLocation))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] data = line.Split(new[] { "\",\"" }, StringSplitOptions.RemoveEmptyEntries);
                            string Address = data.ElementAt(0) == null ? "" : data.ElementAt(0).TrimStart('\"');
                            double Latitude = data.Length > 1 ? double.Parse(data.ElementAt(1)) : 0.0;
                            double Longitude = data.Length > 2 ? double.Parse(data.ElementAt(2).TrimEnd('\"')) : 0.0;
                            Locations.Add(new Location()
                            {
                                Address = Address,
                                Latitude = Latitude,
                                Longitude = Longitude,
                                Distance = 0
                            });

                        }
                    }

                    // Set cache options.
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        // Keep in cache for this time, reset time if accessed.
                        .SetSlidingExpiration(TimeSpan.FromMinutes(3));

                    // Save data in cache.
                    _cache.Set("LocationsList", Locations, cacheEntryOptions);
                }
                return Locations;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // cache + File Read + Linq
        private List<Location> getAllLocationsFileReaderLinq()
        {
            try
            {
                List<Location> Locations = new List<Location>();

                if (!_cache.TryGetValue("LocationsList", out Locations))
                {
                    string fileLocation = "C:\\Users\\chath\\source\\repos\\LocationSearch.DataAccess\\Resources\\locations(5).csv";

                    string[] AllLines = File.ReadAllLines(fileLocation);

                    var qLocations = AllLines.Select(data =>
                    {
                        var data2 = data.Split(new[] { "\",\"" }, StringSplitOptions.RemoveEmptyEntries);
                        string Address = data2.ElementAt(0) == null ? "" : data2.ElementAt(0).TrimStart('\"');
                        double Latitude = data2.Length > 1 ? double.Parse(data2.ElementAt(1)) : 0.0;
                        double Longitude = data2.Length > 2 ? double.Parse(data2.ElementAt(2).TrimEnd('\"')) : 0.0;
                        return new Location()
                        {
                            Address = Address,
                            Latitude = Latitude,
                            Longitude = Longitude,
                            Distance = 0
                        };

                    });
                    Locations = qLocations.ToList();

                    // Set cache options.
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        // Keep in cache for this time, reset time if accessed.
                        .SetSlidingExpiration(TimeSpan.FromMinutes(3));

                    // Save data in cache.
                    _cache.Set("LocationsList", Locations, cacheEntryOptions);
                }
                return Locations;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
