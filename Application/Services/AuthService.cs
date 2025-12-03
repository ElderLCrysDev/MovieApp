using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Application.Services;

public class AuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IConfiguration _config;

    public AuthService(IUserRepository userRepo, IConfiguration config)
    {
        _userRepo = userRepo;
        _config = config;
    }

    public async Task<string?> AuthenticateAsync(string usuario, string senha)
    {
        var user = await _userRepo.GetByNameAsync(usuario);
        if (user == null) return null;

        //-----SENHA: EM AMBIENTES DE PRODUCAO, COMPARACAO DEVE SER FEITA SEMPRE COM USO DE HASH-----//
        if (user.Senha != senha) return null;

        var key = _config["Jwt:Key"] ?? throw new Exception("JWT KEY MISSING");
        var issuer = _config["Jwt:Issuer"] ?? "MovieApp";

        var claims = new List<Claim>
        {
            new Claim("id", user.Id.ToString()),
            new Claim("nome", user.Nome),
            new Claim("tipo", user.TipoUsuario)
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(4),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

