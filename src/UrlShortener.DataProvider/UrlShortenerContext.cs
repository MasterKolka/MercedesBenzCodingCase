using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using UrlShortener.DataProvider.Entities;
using UrlShortener.DataProvider.Repositories;

namespace UrlShortener.DataProvider;

public class UrlShortenerContext: DbContext
{
    public DbSet<HashingSpaceItem> HashingSpace { get; set; }
    public DbSet<ShortenedUrl> ShortenedUrls { get; set; }
    public DbSet<ShortenedUrlAnalytics> ShortenedUrlAnalytics { get; set; }

    public string DbPath { get; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public UrlShortenerContext()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        var folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        DbPath = Path.Join(folder, "UrlShortener.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}

public static class UrlShortenerContextExtensions
{
    public static IServiceCollection AddUrlShortenerContext(this IServiceCollection services)
    {
        return services.AddDbContext<UrlShortenerContext>()
            .AddScoped<IUrlShortenerRepo, UrlShortenerRepo>();
    }
}
