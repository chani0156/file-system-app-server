using files.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;


namespace files.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly FileSystemUtility _fileSystemUtility;


        public FilesController(IMemoryCache cache, FileSystemUtility fileSystemUtility)
        {
            _cache = cache;
            _fileSystemUtility = fileSystemUtility;
        }

        // GET endpoint to retrieve file system data
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<object> primeNgNodes;
                // Try to get data from cache
                if (!_cache.TryGetValue(Constants.Constants.PrimeNgNodesCacheKey, out primeNgNodes))
                {
                    primeNgNodes = _fileSystemUtility.GetDataFromSource();
                     // Cache the data for 10 minutes
                    _cache.Set(Constants.Constants.PrimeNgNodesCacheKey, primeNgNodes, TimeSpan.FromMinutes(10));
                }

                return Ok(primeNgNodes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // GET endpoint for searching file system data
        [HttpGet("search")]
        public IActionResult Search(string q)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(q))
                {
                    return BadRequest("Search parameter 'q' is missing.");
                }

                List<object> primeNgNodes;

                if (!_cache.TryGetValue(Constants.Constants.PrimeNgNodesCacheKey, out primeNgNodes))
                {
                    primeNgNodes = _fileSystemUtility.GetDataFromSource(); // Assuming you have a method to get the data
                    _cache.Set(Constants.Constants.PrimeNgNodesCacheKey, primeNgNodes, TimeSpan.FromMinutes(10)); // Cache for 10 minutes
                }

                List<object> result = new List<object>();
                _fileSystemUtility.SearchData(primeNgNodes, q, result);

                if (result.Count == 0)
                {
                    return NoContent();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
