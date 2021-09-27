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
            Location locationobject = new Location()
            {
                Latitude = latitude,
                Longitude = longitude
            };

           // _locationSearchService.GetLocations();
            return Ok();
        }

        [HttpGet]
        [Route("getLocationsParallel")]
        public IActionResult GetLocationsParallel(double latitude, double longitude, int maxDistance, int maxResults)
        {
            //_locationSearchService.GetLocationsParallel();
            return Ok();
        }
    }
}
