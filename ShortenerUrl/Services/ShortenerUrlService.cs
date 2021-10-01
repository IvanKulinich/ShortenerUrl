using ShortenerUrl.Entities;
using ShortenerUrl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShortenerUrl.Services
{
    public class ShortenerUrlService : IShortenerUrlService
    {
        private readonly IShortenedUrlRepository _shortenedUrlRepository;

        public ShortenerUrlService(IShortenedUrlRepository shortenedUrlRepository)
        {
            _shortenedUrlRepository = shortenedUrlRepository;
        }

        public async Task<List<ShortenedUrl>> GetAllItemsAsync()
        {
            return await _shortenedUrlRepository.GetAllItemsAsync();
        }

        public async Task<string> GetShortenedUrlByRealUrlAsync(string url)
        {
            return await _shortenedUrlRepository.GetShortenedUrlByRealUrlAsync(url);
        }

        public async Task<string> GetRealUrlByTokenAsync(string token)
        {
            return await _shortenedUrlRepository.GetRealUrlByTokenAsync(token);
        }

        public async Task<string> CreateShortenedUrlAsync(string url)
        {
            var urls = await _shortenedUrlRepository.GetAllItemsAsync();

            string token = GenerateToken();
            while (urls.Exists(u => u.Token == token))
            {
                token = GenerateToken();
            }

            if (urls.Exists(u => u.URL == url))
            {
                throw new Exception("URL already exists");
            }

            var newShortenedUrl = await _shortenedUrlRepository.AddItemAsync(new ShortenedUrl()
            {
                Token = token,
                URL = url,
                ShortenedURL = "https://" + token
            });

            return newShortenedUrl.ShortenedURL;
        }

        private string GenerateToken()
        {
            string urlsafe = string.Empty;
            Enumerable.Range(48, 75)
              .Where(i => i < 58 || i > 64 && i < 91 || i > 96)
              .OrderBy(o => new Random().Next())
              .ToList()
              .ForEach(i => urlsafe += Convert.ToChar(i));

            return urlsafe.Substring(new Random().Next(0, urlsafe.Length), new Random().Next(2, 6));
        }
    }
}
