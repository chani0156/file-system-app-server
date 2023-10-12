using files.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;


namespace files.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IMemoryCache _cache;

        public FilesController(IMemoryCache cache)
        {
            _cache = cache;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<object> primeNgNodes;

                if (!_cache.TryGetValue("PrimeNgNodes", out primeNgNodes))
                {
                    string jsonFileName = "tree.json";
                    string jsonFilePath = Path.Combine(AppContext.BaseDirectory, jsonFileName);
                    string jsonContent = System.IO.File.ReadAllText(jsonFilePath);
                    JArray jsonNodes = JArray.Parse(jsonContent);

                    primeNgNodes = new List<object>();

                    foreach (var jsonNode in jsonNodes)
                    {
                        var primeNgNode = FileSystemUtility.ConvertNode(jsonNode);
                        primeNgNodes.Add(primeNgNode);
                    }

                    _cache.Set("PrimeNgNodes", primeNgNodes, TimeSpan.FromMinutes(10)); // Cache for 10 minutes
                }

                return Ok(primeNgNodes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


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

                if (!_cache.TryGetValue("PrimeNgNodes", out primeNgNodes))
                {
                    return BadRequest("PrimeNgNodes not found in cache.");
                }
                List<object> result = new List<object>();
                FileSystemUtility.SearchData(primeNgNodes, q, result);
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
