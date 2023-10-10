using files.Models;
using files.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace files.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly List<FileItem> fileSystemData;
        private readonly ILogger<FilesController> _logger;
        public FilesController(ILogger<FilesController> logger)
        {
            _logger = logger;
            try
            {
                string jsonFileName = "tree.json";
                string jsonFilePath = Path.Combine(AppContext.BaseDirectory, jsonFileName);
                fileSystemData = FileSystemUtility.ReadJsonFromFile(jsonFilePath);
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading JSON file.", ex);
            }
        }

        [HttpGet]
        public async Task< ActionResult<IEnumerable<FileItem>>> Get()
        {
            return Ok(fileSystemData);
        }

        [HttpGet("search")]
        public async Task < ActionResult<IEnumerable<object>>> Search(string q)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return BadRequest("Search parameter 'q' is missing.");
            }

            List<object> result = new List<object>();
            FileSystemUtility.SearchData(fileSystemData, q, result);
            return Ok(result);
        }
    }
}
