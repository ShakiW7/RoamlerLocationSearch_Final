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
            if (latitude < -90 || latitude > 90 || longitude < -180 || longitude > 180)
            {
                string errorMessage = "Arguments Out of Range !";
                return StatusCode(400, errorMessage);
            }

            try
            {
                DateTime startTime = DateTime.UtcNow;

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

                DateTime endTime = DateTime.UtcNow;
                TimeSpan timeSpent = endTime - startTime;
                returnSearchResult.TotalDuration = timeSpent.TotalSeconds;

                return Ok(returnSearchResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("getLocationsParallel")]
        public IActionResult GetLocationsParallel(double latitude, double longitude, int maxDistance, int maxResults)
        {
            if (latitude < -90 || latitude > 90 || longitude < -180 || longitude > 180)
            {
                string errorMessage = "Arguments Out of Range !";
                return StatusCode(400, errorMessage);
            }

            try
            {
                DateTime startTime = DateTime.UtcNow;

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

                DateTime endTime = DateTime.UtcNow;
                TimeSpan timeSpent = endTime - startTime;
                returnSearchResult.TotalDuration = timeSpent.TotalSeconds;

                return Ok(returnSearchResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("getLocationsAsync")]
        public async Task<IActionResult> GetLocationsAsync(double latitude, double longitude, int maxDistance, int maxResults)
        {
            if (latitude < -90 || latitude > 90 || longitude < -180 || longitude > 180)
            {
                string errorMessage = "Arguments Out of Range !";
                return StatusCode(400, errorMessage);
            }

            try
            {
                DateTime startTime = DateTime.UtcNow;

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

                DateTime endTime = DateTime.UtcNow;
                TimeSpan timeSpent = endTime - startTime;
                returnSearchResult.TotalDuration = timeSpent.TotalSeconds;

                return Ok(returnSearchResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
