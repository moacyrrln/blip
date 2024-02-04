using Xunit;
using Moq;
using GitHubRepoApi3.Services;
using GitHubRepoApi3.Controller;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GitHubRepoApi3.Tests
{
    public class GitHubControllerTests
    {
        [Fact]
        public async Task Get_ReturnsExpectedActionResult()
        {
            // Cria um mock para IGitHubService
            var mockService = new Mock<IGitHubService>();
            mockService.Setup(service => service.GetFormattedRepositoriesAsync())
                       .ReturnsAsync(new ApiResponse()); // Substitua ApiResponse pela resposta esperada do método

            // Cria uma instância do GitHubController com o serviço mockado
            var controller = new GitHubController(mockService.Object);

            // Executa o método Get do Controller
            var result = await controller.Get();

            // Verifica se o resultado é do tipo esperado (por exemplo, OkObjectResult)
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
