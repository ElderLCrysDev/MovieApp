using API.Middleware;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace API.Tests
{
    public class RequestLoggingMiddlewareTests
    {
        [Fact]
        public async Task Middleware_GravaLog_Sucesso()
        {
            // Arrange
            var mockLogRepo = new Mock<ILogRepository>();
            mockLogRepo.Setup(r => r.AddAsync(It.IsAny<LogEntry>()))
                       .Returns(Task.CompletedTask)
                       .Verifiable();

            RequestDelegate next = (HttpContext ctx) =>
            {
                ctx.Response.StatusCode = 200;
                return Task.CompletedTask;
            };

            var middleware = new RequestLoggingMiddleware(next);

            var context = new DefaultHttpContext();
            context.Request.Path = "/teste";

            // Act
            await middleware.InvokeAsync(context, mockLogRepo.Object);

            // Assert
            mockLogRepo.Verify(r => r.AddAsync(It.Is<LogEntry>(l =>
                l.EndpointRequisicao == "/teste" &&
                l.ObteveSucesso == true
            )), Times.Once);
        }

        [Fact]
        public async Task Middleware_GravaLog_Erro()
        {
            // Arrange
            var mockLogRepo = new Mock<ILogRepository>();
            mockLogRepo.Setup(r => r.AddAsync(It.IsAny<LogEntry>()))
                       .Returns(Task.CompletedTask)
                       .Verifiable();

            RequestDelegate next = (HttpContext ctx) =>
            {
                ctx.Response.StatusCode = 500;
                throw new System.Exception("Erro simulado");
            };

            var middleware = new RequestLoggingMiddleware(next);

            var context = new DefaultHttpContext();
            context.Request.Path = "/erro";

            // Act & Assert
            await Assert.ThrowsAsync<System.Exception>(() => middleware.InvokeAsync(context, mockLogRepo.Object));

            mockLogRepo.Verify(r => r.AddAsync(It.Is<LogEntry>(l =>
                l.EndpointRequisicao == "/erro" &&
                l.ObteveSucesso == false
            )), Times.Once);
        }
    }
}
