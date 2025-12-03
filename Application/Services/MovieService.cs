using System.Threading.Tasks;
using Core.Interfaces;

namespace Application.Services
{
    public class MovieService : IMovieService
    {
        private readonly ITmdbService _tmdb;

        public MovieService(ITmdbService tmdb)
        {
            _tmdb = tmdb;
        }

        public async Task<string> GetPopularAsync(int page = 1) => await _tmdb.GetPopularMoviesAsync(page);
        public async Task<string> SearchAsync(string query, int page = 1) => await _tmdb.SearchMoviesAsync(query, page);
        public async Task<string> GetDetailsAsync(int id) => await _tmdb.GetMovieDetailsAsync(id);
        public async Task<string> GetCreditsAsync(int id) => await _tmdb.GetMovieCreditsAsync(id);
        public async Task<string> GetVideosAsync(int id) => await _tmdb.GetMovieVideosAsync(id);
    }
}
