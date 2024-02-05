using GitHubRepoApi3.Services;
using Microsoft.AspNetCore.Mvc;

namespace GitHubRepoApi3.Controllers
{
    [ApiController]
    [Route("")]
    public class GitHubController : ControllerBase
    {
        private readonly IGitHubService _gitHubService;

        public GitHubController(IGitHubService gitHubService)
        {
            _gitHubService = gitHubService;
        }

        [HttpGet]
        [Route("github")]
        public async Task<IActionResult> Get()
        {
            try
            {
                var response = await _gitHubService.GetFormattedRepositoriesAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
