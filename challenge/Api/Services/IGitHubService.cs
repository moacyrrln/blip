using System.Threading.Tasks;

namespace GitHubRepoApi.Services
{
    public interface IGitHubService
    {
        Task<ApiResponse> GetFormattedRepositoriesAsync();
    }
}
