using LinkedFit.APPLICATION.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LinkedFit.API.Controllers
{
    [Route("api/v1/search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private ILogger<SearchController> _logger;
        private ISearchService _searchService;

        public SearchController(ILogger<SearchController> logger,ISearchService searchService)
        {
            _logger = logger;
            _searchService = searchService;
        }

        [HttpGet]
        public async Task<IActionResult> Search(string query)
        {
            try
            {
                _logger.LogInformation($"Search query: {query}");
                var result = await _searchService.Search(query);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
