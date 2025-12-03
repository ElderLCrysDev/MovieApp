using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.Extensions.Configuration;
using Core.Interfaces;

namespace Infrastructure.External
{
    public class TmdbService : ITmdbService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;

        public TmdbService(HttpClient http, IConfiguration config)
        {
            _http = http;

            //-----PEGA VARIAVEL DO AMBIENTE-----//
            _apiKey = Environment.GetEnvironmentVariable("TMDB_API_KEY") ?? string.Empty;

            //-----CASO VAZIA, OBTEM KEY DA APPSETTINGS-----//
            if (string.IsNullOrEmpty(_apiKey))
            {
                _apiKey = config["Tmdb:ApiKey"]
                          ?? throw new Exception("TMDB_API_KEY não definida nem no appsettings.json!");
            }

            if (_http.BaseAddress == null)
                _http.BaseAddress = new Uri("https://api.themoviedb.org/3/");
        }

        private string BuildUrl(string path, IDictionary<string, string>? query = null)
        {
            var q = $"api_key={_apiKey}";
            if (query != null)
            {
                foreach (var kv in query)
                {
                    q += $"&{kv.Key}={Uri.EscapeDataString(kv.Value)}";
                }
            }

            var trimmedPath = path.StartsWith("/") ? path : "/" + path;
            return $"{_http.BaseAddress?.ToString().TrimEnd('/')}{trimmedPath}?{q}";
        }

        public async Task<string> GetPopularMoviesAsync(int page = 1)
        {
            var url = BuildUrl("/movie/popular", new Dictionary<string, string> { { "page", page.ToString() } });
            var resp = await _http.GetAsync(url);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadAsStringAsync();
        }

        public async Task<string> SearchMoviesAsync(string query, int page = 1)
        {
            var url = BuildUrl("/search/movie", new Dictionary<string, string> { { "query", query }, { "page", page.ToString() } });
            var resp = await _http.GetAsync(url);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadAsStringAsync();
        }

        public async Task<string> GetMovieDetailsAsync(int movieId)
        {
            var url = BuildUrl($"/movie/{movieId}");
            var resp = await _http.GetAsync(url);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadAsStringAsync();
        }

        public async Task<string> GetMovieCreditsAsync(int movieId)
        {
            var url = BuildUrl($"/movie/{movieId}/credits");
            var resp = await _http.GetAsync(url);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadAsStringAsync();
        }

        public async Task<string> GetMovieVideosAsync(int movieId)
        {
            var url = BuildUrl($"/movie/{movieId}/videos");
            var resp = await _http.GetAsync(url);
            resp.EnsureSuccessStatusCode();
            return await resp.Content.ReadAsStringAsync();
        }
    }
}
