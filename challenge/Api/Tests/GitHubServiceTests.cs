using Moq;
using GitHubRepoApi3.Services;
using System.Net;
using Moq.Protected;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System.Net.Http.Json;

namespace GitHubRepoApi3.Tests
{
    public class GitHubServiceTests
    {
        [Fact]
        public async Task GetFormattedRepositoriesAsync_ReturnsApiResponse()
        {
            // Mock do IHttpClientFactory
            var mockFactory = new Mock<IHttpClientFactory>();
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(new[]
                    {
                        new Repository { Name = "Repo 1", Language = "C#", CreatedAt = DateTime.Now.AddHours(-5) /* outras propriedades */ },
                        new Repository { Name = "Repo 2", Language = "C#", CreatedAt = DateTime.Now.AddHours(-4) /* outras propriedades */ },
                        new Repository { Name = "Repo 3", Language = "C#", CreatedAt = DateTime.Now.AddHours(-3) /* outras propriedades */ },
                        new Repository { Name = "Repo 4", Language = "C#", CreatedAt = DateTime.Now.AddHours(-2) /* outras propriedades */ },
                        new Repository { Name = "Repo 5", Language = "C#", CreatedAt = DateTime.Now.AddHours(-1) /* outras propriedades */ },
                    })
                });

            var client = new HttpClient(mockHttpMessageHandler.Object);
            mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

            // Mock do IMemoryCache
            var mockMemoryCache = new Mock<IMemoryCache>();
            mockMemoryCache.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());

            // Instanciando o serviço
            var service = new GitHubService(mockFactory.Object, mockMemoryCache.Object);

            // Executando o método e verificando o resultado
            var result = await service.GetFormattedRepositoriesAsync();

            // Verifica se o resultado é conforme esperado
            Assert.NotNull(result);

            // Verifica se exatamente 5 repositórios são retornados
            Assert.Equal(5, result.Content.Items.Count);
        }
    }
}
