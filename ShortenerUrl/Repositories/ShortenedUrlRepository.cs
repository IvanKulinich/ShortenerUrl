using Microsoft.EntityFrameworkCore;
using ShortenerUrl.Data;
using ShortenerUrl.Entities;
using ShortenerUrl.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShortenerUrl.Repositories
{
    public class ShortenedUrlRepository : IShortenedUrlRepository
    {
        private readonly ShortenerDbContext _shortenerDbContext;

        public ShortenedUrlRepository(ShortenerDbContext shortenerDbContext)
        {
            _shortenerDbContext = shortenerDbContext;
        }

        public async Task<List<ShortenedUrl>> GetAllItemsAsync()
        {
            return await _shortenerDbContext.ShortenedUrls.ToListAsync();
        }

        public async Task<string> GetShortenedUrlByRealUrlAsync(string url)
        {
            return await _shortenerDbContext.ShortenedUrls
                .Where(x => x.URL == url)
                .Select(x => x.ShortenedURL)
                .FirstOrDefaultAsync();
        }

        public async Task<string> GetRealUrlByTokenAsync(string token)
        {
            return await _shortenerDbContext.ShortenedUrls
                .Where(x => x.Token == token)
                .Select(x => x.URL)
                .FirstOrDefaultAsync();
        }

        public async Task<ShortenedUrl> AddItemAsync(ShortenedUrl shortenedUrl)
        {
            var newShortenedUrl = _shortenerDbContext.ShortenedUrls.Add(shortenedUrl);
            await _shortenerDbContext.SaveChangesAsync();

            return newShortenedUrl.Entity;
        }
    }
}
