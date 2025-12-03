using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces;
public interface ITmdbService
{
    Task<string> GetPopularMoviesAsync(int page = 1);
    Task<string> SearchMoviesAsync(string query, int page = 1);
    Task<string> GetMovieDetailsAsync(int movieId);
    Task<string> GetMovieCreditsAsync(int movieId);
    Task<string> GetMovieVideosAsync(int movieId);
}
