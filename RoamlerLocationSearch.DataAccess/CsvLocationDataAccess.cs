using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _config;
        private readonly string _fileLocation;
        private readonly int _cacheInterval;
        private readonly string _cacheKey;
        public CsvLocationDataAccess(IMemoryCache memoryCache, IConfiguration config)
        {
            _cache = memoryCache;
            _config = config;

            string fileName = _config.GetSection("AppSettings").GetChildren().FirstOrDefault(x => x.Key == "CSVFileName").Value;
            _fileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            int.TryParse(_config.GetSection("AppSettings").GetChildren().FirstOrDefault(x => x.Key == "CacheRefreshInterval").Value, out _cacheInterval);
            _cacheKey = _config.GetSection("AppSettings").GetChildren().FirstOrDefault(x => x.Key == "CacheKey").Value;
        }

        public List<Location> GetLocations()
        {
            try
            {
                return getAllLocationsCacheStreamReader();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Location>> GetLocationsAsync()
        {
            try
            {
                List<Location> Locations = new List<Location>();

                if (_cache.Get(_cacheKey) == null)
                {
                    using (var reader = new StreamReader(_fileLocation))
                    {
                        reader.ReadLine();
                        string line;
                        while ((line = await reader.ReadLineAsync()) != null)
                        {
                            string[] data = line.Split(new[] { "\",\"" }, StringSplitOptions.RemoveEmptyEntries);
                            string Address = data.ElementAt(0) == null ? "" : data.ElementAt(0).TrimStart('\"');
                            double Latitude = data.Length > 1 ? double.Parse(data.ElementAt(1)) : 0.0;
                            double Longitude = data.Length > 2 ? double.Parse(data.ElementAt(2).TrimEnd('\"')) : 0.0;
                            Locations.Add(new Location(Latitude, Longitude, Address));
                        }

                        // Set cache options.
                        var cacheEntryOptions = GetCacheEntryOptions();
                        _cache.Set(_cacheKey, Locations, cacheEntryOptions);
                    }
                }
                else
                {
                    _cache.TryGetValue(_cacheKey, out Locations);
                }
                return Locations;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // cache + File Read + Parallel
        private List<Location> getAllLocationsCacheFileReadParallel()
        {
            try
            {
                List<Location> Locations = new List<Location>();

                if (_cache.Get(_cacheKey) == null)

                {
                    string[] AllLines = File.ReadAllLines(_fileLocation);

                    var options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount * 4 };

                    Parallel.For(0, AllLines.Length, options, i =>
                    {
                        string[] data = AllLines[i].Split(new[] { "\",\"" }, StringSplitOptions.RemoveEmptyEntries);
                        string Address = data.ElementAt(0) == null ? "" : data.ElementAt(0).TrimStart('\"');
                        double Latitude = data.Length > 1 ? double.Parse(data.ElementAt(1)) : 0.0;
                        double Longitude = data.Length > 2 ? double.Parse(data.ElementAt(2).TrimEnd('\"')) : 0.0;

                        Locations.Add(new Location(Latitude, Longitude, Address));
                    });

                    // Set cache options.
                    var cacheEntryOptions = GetCacheEntryOptions();
                    _cache.Set(_cacheKey, Locations, cacheEntryOptions);
                }
                else
                {
                    _cache.TryGetValue(_cacheKey, out Locations);
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

                if (_cache.Get(_cacheKey) == null)
                {
                    using (StreamReader reader = new StreamReader(_fileLocation))
                    {
                        reader.ReadLine(); //to skip the headers
                        string line;
                        while ((line = reader.ReadLine()) != null)
                        {
                            string[] data = line.Split(new[] { "\",\"" }, StringSplitOptions.RemoveEmptyEntries);
                            string Address = data.ElementAt(0) == null ? "" : data.ElementAt(0).TrimStart('\"');
                            double Latitude = data.Length > 1 ? double.Parse(data.ElementAt(1)) : 0.0;
                            double Longitude = data.Length > 2 ? double.Parse(data.ElementAt(2).TrimEnd('\"')) : 0.0;
                            Locations.Add(new Location(Latitude, Longitude, Address));
                        }
                    }

                    // Set cache options.
                    var cacheEntryOptions = GetCacheEntryOptions();
                    _cache.Set(_cacheKey, Locations, cacheEntryOptions);
                }
                else
                {
                    _cache.TryGetValue(_cacheKey, out Locations);
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

                if (_cache.Get(_cacheKey) == null)
                {
                    string[] AllLines = File.ReadAllLines(_fileLocation);
                    var qLocations = AllLines.Skip(1).Select(data =>
                    {
                        var data2 = data.Split(new[] { "\",\"" }, StringSplitOptions.RemoveEmptyEntries);
                        string Address = data2.ElementAt(0) == null ? "" : data2.ElementAt(0).TrimStart('\"');
                        double Latitude = data2.Length > 1 ? double.Parse(data2.ElementAt(1)) : 0.0;
                        double Longitude = data2.Length > 2 ? double.Parse(data2.ElementAt(2).TrimEnd('\"')) : 0.0;
                        return new Location(Latitude, Longitude, Address);

                    });
                    Locations = qLocations.ToList();

                    // Set cache options.
                    var cacheEntryOptions = GetCacheEntryOptions();
                    _cache.Set(_cacheKey, Locations, cacheEntryOptions);
                }
                else
                {
                    _cache.TryGetValue(_cacheKey, out Locations);
                }
                return Locations;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private MemoryCacheEntryOptions GetCacheEntryOptions()
        {
            return new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(_cacheInterval));
        }

    }
}
