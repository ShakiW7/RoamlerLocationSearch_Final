using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoamlerLocationSearch.Business;
using RoamlerLocationSearch.Domain.Entities;
using RoamlerLocationSearch.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoamlerLocationSearch.WebAPI.Controllers
{
    [ApiVersion("1.0")]
    public class LocationSearchController : Controller
    {
        private readonly ILogger<LocationSearchController> _logger;
        private readonly ILocationSearchService _locationSearchService;

        public LocationSearchController(ILogger<LocationSearchController> logger, ILocationSearchService locationSearchService)
        {
            _logger = logger;
            _locationSearchService = locationSearchService;
        }

        [HttpGet]
        [Route("getLocations")]
        public ActionResult<SearchResult> GetLocations(double latitude, double longitude, int maxDistance, int maxResults)
        {
            DateTime t1 = DateTime.UtcNow;

            Location locationobject = new Location(latitude, longitude, "");

            List<Location> locationList = _locationSearchService.GetLocations(locationobject, maxDistance, maxResults);

            // TODO: tidy up
            SearchResult returnSearchResult = new SearchResult();
            returnSearchResult.Latitude = latitude;
            returnSearchResult.Longitude = longitude;
            returnSearchResult.MaxDistance = maxDistance;
            returnSearchResult.MaxResults = maxResults;
            returnSearchResult.RecordCount = locationList.Count;
            returnSearchResult.Locations = locationList;

            DateTime t2 = DateTime.UtcNow;
            TimeSpan t = t2 - t1;
            double d = t.TotalSeconds;
            returnSearchResult.TotalDuration = d;

            return Ok(returnSearchResult);
        }

        [HttpGet]
        [Route("getLocationsParallel")]
        public IActionResult GetLocationsParallel(double latitude, double longitude, int maxDistance, int maxResults)
        {
            DateTime t1 = DateTime.UtcNow;

            Location locationobject = new Location(latitude, longitude, "");
            List<Location> locationList = _locationSearchService.GetLocationsParallel(locationobject, maxDistance, maxResults);

            // TODO: tidy up
            SearchResult returnSearchResult = new SearchResult();
            returnSearchResult.Latitude = latitude;
            returnSearchResult.Longitude = longitude;
            returnSearchResult.MaxDistance = maxDistance;
            returnSearchResult.MaxResults = maxResults;
            returnSearchResult.RecordCount = locationList.Count;
            returnSearchResult.Locations = locationList;

            DateTime t2 = DateTime.UtcNow;
            TimeSpan t = t2 - t1;
            double d = t.TotalSeconds;
            returnSearchResult.TotalDuration = d;

            return Ok(returnSearchResult);
        }

        [HttpGet]
        [Route("getLocationsAsync")]
        public async Task<IActionResult> GetLocationsAsync(double latitude, double longitude, int maxDistance, int maxResults)
        {
            DateTime t1 = DateTime.UtcNow;

            Location locationobject = new Location(latitude, longitude, "");
            List<Location> locationList = await _locationSearchService.GetLocationsAsync(locationobject, maxDistance, maxResults);

            // TODO: tidy up
            SearchResult returnSearchResult = new SearchResult();
            returnSearchResult.Latitude = latitude;
            returnSearchResult.Longitude = longitude;
            returnSearchResult.MaxDistance = maxDistance;
            returnSearchResult.MaxResults = maxResults;
            returnSearchResult.RecordCount = locationList.Count;
            returnSearchResult.Locations = locationList;

            DateTime t2 = DateTime.UtcNow;
            TimeSpan t = t2 - t1;
            double d = t.TotalSeconds;
            returnSearchResult.TotalDuration = d;

            return Ok(returnSearchResult);
        }
    }
}
