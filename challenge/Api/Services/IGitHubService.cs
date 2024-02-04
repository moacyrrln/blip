namespace GitHubRepoApi3.Services
{
    public interface IGitHubService
    {
        Task<ApiResponse> GetFormattedRepositoriesAsync();
    }
}
