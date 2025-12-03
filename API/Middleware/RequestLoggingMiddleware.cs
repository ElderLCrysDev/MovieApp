using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace API.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ILogRepository logRepo)
    {
        var path = context.Request.Path.ToString();
        int? userId = null;
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var idClaim = context.User.Claims.FirstOrDefault(c => c.Type == "id");
            if (idClaim != null && int.TryParse(idClaim.Value, out int parsedId))
                userId = parsedId;
        }

        var entry = new LogEntry
        {
            IdUsuario = userId,
            EndpointRequisicao = path,
            DataHoraRequisicao = DateTime.UtcNow,
            ObteveSucesso = false
        };

        try
        {
            await _next(context);
            entry.ObteveSucesso = context.Response.StatusCode < 400;
        }
        catch
        {
            entry.ObteveSucesso = false;
            throw;
        }
        finally
        {
            await logRepo.AddAsync(entry);
        }
    }
}
