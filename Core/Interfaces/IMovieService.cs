using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IMovieService
    {
        Task<string> GetPopularAsync(int page = 1);
        Task<string> SearchAsync(string query, int page = 1);
        Task<string> GetDetailsAsync(int id);
        Task<string> GetCreditsAsync(int id);
        Task<string> GetVideosAsync(int id);
    }
}
