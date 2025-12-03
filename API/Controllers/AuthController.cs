using Microsoft.AspNetCore.Mvc;
using Application.Services;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _auth;

    public AuthController(AuthService auth)
    {
        _auth = auth;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest req)
    {
        //-----VALIDA SE DADOS BATEM COM OS DA BASE-----//
        var token = await _auth.AuthenticateAsync(req.Usuario, req.Senha);
        if (token == null) return Unauthorized(new { message = "Credenciais inválidas" });
        return Ok(new { token });
    }
}

public record LoginRequest(string Usuario, string Senha);
