using API.Controllers;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace API.Tests
{
    public class MoviesControllerTests
    {
        private readonly Mock<IMovieService> _mockService;
        private readonly MoviesController _controller;

        public MoviesControllerTests()
        {
            _mockService = new Mock<IMovieService>();
            _controller = new MoviesController(_mockService.Object);
        }

        [Fact]
        public async Task Populares_RetornaOkComResultados()
        {
            // Arrange
            string jsonResponse = "{\"results\":[{\"id\":1,\"title\":\"Filme 1\"}]}";
            _mockService.Setup(s => s.GetPopularAsync(1)).ReturnsAsync(jsonResponse);

            // Act
            var result = await _controller.Populares(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var obj = Assert.IsType<JsonElement>(okResult.Value);
            Assert.True(obj.TryGetProperty("results", out _));
        }

        [Fact]
        public async Task Buscar_RetornaBadRequest_QuandoQueryVazia()
        {
            // Act
            var result = await _controller.Buscar("", 1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Buscar_RetornaOkComResultados()
        {
            // Arrange
            string jsonResponse = "{\"results\":[{\"id\":2,\"title\":\"Filme Teste\"}]}";
            _mockService.Setup(s => s.SearchAsync("teste", 1)).ReturnsAsync(jsonResponse);

            // Act
            var result = await _controller.Buscar("teste", 1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var obj = Assert.IsType<JsonElement>(okResult.Value);
            Assert.True(obj.TryGetProperty("results", out _));
        }

        [Fact]
        public async Task Detalhes_RetornaOk()
        {
            // Arrange
            string jsonResponse = "{\"id\":1,\"title\":\"Filme 1\"}";
            _mockService.Setup(s => s.GetDetailsAsync(1)).ReturnsAsync(jsonResponse);

            // Act
            var result = await _controller.Detalhes(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var obj = Assert.IsType<JsonElement>(okResult.Value);
            Assert.Equal("Filme 1", obj.GetProperty("title").GetString());
        }

        [Fact]
        public async Task Creditos_RetornaOk()
        {
            // Arrange
            string jsonResponse = "{\"cast\":[{\"name\":\"Ator 1\"}]}";
            _mockService.Setup(s => s.GetCreditsAsync(1)).ReturnsAsync(jsonResponse);

            // Act
            var result = await _controller.Creditos(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var obj = Assert.IsType<JsonElement>(okResult.Value);
            Assert.True(obj.TryGetProperty("cast", out _));
        }

        [Fact]
        public async Task Videos_RetornaOk()
        {
            // Arrange
            string jsonResponse = "{\"results\":[{\"key\":\"abcd1234\"}]}";
            _mockService.Setup(s => s.GetVideosAsync(1)).ReturnsAsync(jsonResponse);

            // Act
            var result = await _controller.Videos(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var obj = Assert.IsType<JsonElement>(okResult.Value);
            Assert.True(obj.TryGetProperty("results", out _));
        }
    }
}
