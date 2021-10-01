using ShortenerUrl.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShortenerUrl.Interfaces
{
    public interface IShortenedUrlRepository
    {
        Task<List<ShortenedUrl>> GetAllItemsAsync();

        Task<string> GetShortenedUrlByRealUrlAsync(string url);

        Task<string> GetRealUrlByTokenAsync(string token);

        Task<ShortenedUrl> AddItemAsync(ShortenedUrl shortenedUrl);
    }
}
