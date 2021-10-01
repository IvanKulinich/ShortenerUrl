using ShortenerUrl.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShortenerUrl.Interfaces
{
    public interface IShortenerUrlService
    {
        Task<List<ShortenedUrl>> GetAllItemsAsync();

        Task<string> CreateShortenedUrlAsync(string url);

        Task<string> GetShortenedUrlByRealUrlAsync(string url);

        Task<string> GetRealUrlByTokenAsync(string token);
    }
}
