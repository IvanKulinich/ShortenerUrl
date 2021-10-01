using Microsoft.EntityFrameworkCore;
using ShortenerUrl.Entities;

namespace ShortenerUrl.Data
{
    public class ShortenerDbContext : DbContext
    {
        public ShortenerDbContext(DbContextOptions<ShortenerDbContext> options) : base(options)
        {
        }

        public DbSet<ShortenedUrl> ShortenedUrls { get; set; }
    }
}
