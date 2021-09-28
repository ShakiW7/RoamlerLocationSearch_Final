using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using RoamlerLocationSearch.DataAccess;
using RoamlerLocationSearch.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace RoamlerLocationSearch.Tests
{
    public class CsvDataAccessTest
    {
        private readonly CsvLocationDataAccess _sut;

        public CsvDataAccessTest()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());

            var inMemorySettings = new Dictionary<string, string>
            {
                {"AppSettings:CSVFileName", "Resources\\locations(5).csv"},
                {"AppSettings:CacheRefreshInterval", "3"},
                {"AppSettings:CacheKey", "LocationsList"}
            };
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _sut = new CsvLocationDataAccess(cache, configuration);
        }

        [Fact]
        public void ReadCsvGetLocationsShouldEqualRowCount()
        {
            //Arrange
            int numberOfRows = 168891;

            //Act
            List<Location> locationSet = _sut.GetLocations();

            //Assert
            Assert.Equal(numberOfRows, locationSet.Count);

        }

        [Fact]
        public void ReadCsvGetLocationsAsyncShouldEqualRowCount()
        {
            //Arrange
            int numberOfRows = 168891;
            List<Location> locationSet = new List<Location>();

            Task.Run(async () =>
            {
               locationSet = await _sut.GetLocationsAsync();
            }).GetAwaiter().GetResult();

            //Assert
            Assert.Equal(numberOfRows, locationSet.Count);
        }
    }
}
