using GitHubRepoApi3.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddScoped<IGitHubService, GitHubService>();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.MapGet("/", () => "Hello World!");
app.MapControllers();

app.Run();
