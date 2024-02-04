using System.ComponentModel;
using System.Text.Json.Serialization;

namespace GitHubRepoApi3.Services
{
    public class GitHubService : IGitHubService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GitHubService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ApiResponse> GetFormattedRepositoriesAsync()
        {
            var httpClient = _httpClientFactory.CreateClient();

            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("GitHubRepoApi3");

            var url = "https://api.github.com/orgs/takenet/repos";
            var response = await httpClient.GetAsync(url);
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
        public long Id { get; set; }
        [JsonPropertyName("node_id")]
        public string NodeId { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("full_name")]
        public string FullName { get; set; }
        public bool Private { get; set; }
        public Owner Owner { get; set; }
        [JsonPropertyName("html_url")]
        public string HtmlUrl { get; set; }
        public string Description { get; set; }
        public bool Fork { get; set; }
        public string Url { get; set; }
        [JsonPropertyName("forks_url")]
        public string ForksUrl { get; set; }
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }
        [JsonPropertyName("pushed_at")]
        public DateTime PushedAt { get; set; }
        public string Language { get; set; }
        public License License { get; set; }
        public bool Archived { get; set; }
        public bool Disabled { get; set; }
        [JsonPropertyName("open_issues_count")]
        public int OpenIssuesCount { get; set; }
    }

    public class Owner
    {
        [JsonPropertyName("login")]
        public string Login { get; set; }
        public long Id { get; set; }
        [JsonPropertyName("node_id")]
        public string NodeId { get; set; }
        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; }
        [JsonPropertyName("html_url")]
        public string HtmlUrl { get; set; }
    }

    public class License
    {
        public string Key { get; set; }
        public string Name { get; set; }
        [JsonPropertyName("spdx_id")]
        public string SpdxId { get; set; }
        public string Url { get; set; }
        [JsonPropertyName("node_id")]
        public string NodeId { get; set; }
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
}
