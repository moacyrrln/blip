using GitHubRepoApi3.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddScoped<IGitHubService, GitHubService>();

var app = builder.Build();
app.UseDeveloperExceptionPage();
app.MapControllers();

app.Run();
