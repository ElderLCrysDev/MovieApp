using API.Middleware;
using Application.Services;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.External;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

var builder = WebApplication.CreateBuilder(args);

//-----CONFIGURACOES DE LOG-----// 
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/movieapp-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

//-----CARREGAR CONFIG-----// 
var configuration = builder.Configuration;

//-----ADD SERVICES-----// 
builder.Services.AddControllers();

//-----DB CONTEXT-----// 
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Infrastructure")
    )
);

//-----REPOS-----// 
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILogRepository, LogRepository>();

//-----SERVICES-----// 
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<MovieService>();
builder.Services.AddScoped<IMovieService, MovieService>();

builder.Services.AddHttpClient<ITmdbService, TmdbService>(client =>
{
    client.BaseAddress = new Uri("https://api.themoviedb.org/3/");
});

//-----AUTH JWT-----// 
var key = configuration["Jwt:Key"] ?? throw new Exception("JWT KEY MISSING");
var _apiKey = configuration["Tmdb:ApiKey"] ?? string.Empty;

var issuer = configuration["Jwt:Issuer"] ?? "MovieApp";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = issuer,
        ValidAudience = issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };

    // Ajuste para Swagger: reconhece token enviado no header Authorization
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                context.Token = authHeader.Substring("Bearer ".Length).Trim();
            }
            return Task.CompletedTask;
        }
    };
});

//-----CONFIGURACAO SWAGGER-----// 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MovieApp API", Version = "v1" });

    //-----SETA BEARER PARA SWAGGER-----//
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter only your token here.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


//-----CORS-----//
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var app = builder.Build();

//-----MIGRATE & SEED-----// 
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    //-----SEEDA USUARIOS CASO NÃO EXISTAM-----//
    if (!db.Usuarios.Any())
    {
        db.Usuarios.Add(new Core.Entities.User { Nome = "GI", Senha = "nuCl3o", TipoUsuario = "Normal" });
        db.Usuarios.Add(new Core.Entities.User { Nome = "ADMIN", Senha = "l#gUin", TipoUsuario = "Administrador" });
        db.SaveChanges();
    }
}

//-----MIDDLEWARE PIPELINE-----// 
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSerilogRequestLogging();

app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseRouting();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<RequestLoggingMiddleware>();

app.MapControllers();

app.Run();
