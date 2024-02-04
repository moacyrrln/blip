using System.Text.Json.Serialization;
using System.Net.Http.Json;

namespace GitHubRepoApi.Services
{
    public class GitHubService : IGitHubService
    {
        private readonly HttpClient _httpClient;

        public GitHubService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse> GetFormattedRepositoriesAsync()
        {
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("GitHubRepoApi");

            var url = "https://api.github.com/orgs/takenet/repos?per_page=100";
            var response = await _httpClient.GetAsync(url);
            var repositories = await response.Content.ReadFromJsonAsync<IEnumerable<Repository>>();

            var formattedResponse = new ApiResponse();

            foreach (var repo in repositories.Where(r => r.Language == "C#").OrderBy(r => r.CreatedAt).Take(5))
            {
                formattedResponse.Content.Items.Add(new ApiResponseItem
                {
                    Header = new ApiResponseHeader
                    {
                        Value = new ApiResponseValue
                        {
                            Title = repo.Name,
                            Text = repo.Description,
                            Uri = "https://github.com/moacyrrln/blip/blob/master/images/take.jpg?raw=true"
                        }
                    },
                    Options = new List<ApiResponseOptions>()
                    {
                        new ApiResponseOptions
                        {
                            Label = new ApiResponseLink
                                {
                                    Value = new LinkValue
                                    {
                                        Title = repo.Name,
                                        Uri = repo.HtmlUrl
                                    }
                                }
                        }
                    }
                });
            }

            return formattedResponse;
        }
    }

    public class Repository
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public DateTime CreatedAt { get; set; }
        public Owner Owner { get; set; }
        [JsonPropertyName("html_url")]
        public string HtmlUrl { get; set; }
    }

    public class Owner
    {
        public string AvatarUrl { get; set; }
    }

    public class ApiResponse
    {
        public ApiResponseContent Content { get; set; } = new ApiResponseContent();
    }

    public class ApiResponseContent
    {
        [JsonPropertyName("itemType")]
        public string ItemType { get; set; } = "application/vnd.lime.document-select+json";
        public IList<ApiResponseItem> Items { get; set; } = new List<ApiResponseItem>();
    }

    public class ApiResponseItem
    {
        public ApiResponseHeader Header { get; set; } = new ApiResponseHeader();
        public IList<ApiResponseOptions> Options { get; set; } = new List<ApiResponseOptions>();
    }

    public class ApiResponseHeader
    {
        public string Type { get; set; } = "application/vnd.lime.media-link+json";
        public ApiResponseValue Value { get; set; } = new ApiResponseValue();
    }

    public class ApiResponseOptions
    {
        public ApiResponseLink Label { get; set; }
    }

    public class ApiResponseLink
    {
        public string Type { get; set; } = "application/vnd.lime.web-link+json";
        public LinkValue Value { get; set; }
    }

    public class LinkValue
    {
        public string Title { get; set; }
        public string Uri { get; set; }
    }

    public class ApiResponseValue
    {
        public string Title { get; set; }
        public string Text { get; set; }
        [JsonPropertyName("type")]
        public string MediaType { get; set; } = "image/jpeg";
        public string Uri { get; set; }
    }

    // Interface IGitHubService e outras definições conforme necessário
}








//using System.Text.Json.Serialization;

//namespace GitHubRepoApi.Services
//{
//    public class GitHubService : IGitHubService
//    {
//        private readonly HttpClient _httpClient;

//        public GitHubService(HttpClient httpClient)
//        {
//            _httpClient = httpClient;
//        }

//        public async Task<IEnumerable<Repository>> GetOldestRepositoriesAsync()
//        {
//            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("GitHubRepoApi");

//            var url = "https://api.github.com/orgs/takenet/repos?per_page=100";

//            var response = await _httpClient.GetAsync(url);

//            var repositories = await response.Content.ReadFromJsonAsync<IEnumerable<Repository>>();

//            return repositories?.Where(r => r.Language == "C#").OrderBy(r => r.CreatedAt).Take(5) ?? Enumerable.Empty<Repository>();
//        }
//    }

//    public class Repository
//    {
//        public string? Name { get; set; }
//        public string? Description { get; set; }

//        [JsonPropertyName("created_at")]
//        public DateTime CreatedAt { get; set; }

//        public string? Language { get; set; }
//    }
//}
