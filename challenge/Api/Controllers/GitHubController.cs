using Microsoft.AspNetCore.Mvc;
using GitHubRepoApi.Services;

namespace GitHubRepoApi.Controllers
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
                var repositories = await _gitHubService.GetFormattedRepositoriesAsync();
                return Ok(repositories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}
