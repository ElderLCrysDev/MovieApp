using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace API.Tests
{
    //-----TESTE CLASS-----//
    public class AuthTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public AuthTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        //-----CAPTURAR TOKEN DO LOGIN-----//
        private class LoginResponse
        {
            public string Token { get; set; } = string.Empty;
        }

        [Fact]
        public async Task GetMovies_WithoutToken_ReturnsUnauthorized()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/movies/populares");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetMovies_WithValidToken_ReturnsOk()
        {
            var client = _factory.CreateClient();

            //-----SIMULA LOGIN PARA OBTER JWT-----//
            var loginResponse = await client.PostAsJsonAsync("/api/auth/login", new
            {
                usuario = "ADMIN",
                senha = "l#gUin"
            });

            loginResponse.EnsureSuccessStatusCode();

            var loginData = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
            Assert.NotNull(loginData);
            Assert.False(string.IsNullOrEmpty(loginData?.Token));

            //-----ADICIONA TOKEN NA HEADER-----//
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginData!.Token);

            var response = await client.GetAsync("/api/movies/populares");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
