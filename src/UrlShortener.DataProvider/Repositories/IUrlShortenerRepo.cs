using UrlShortener.DataProvider.Entities;

namespace UrlShortener.DataProvider.Repositories;

public interface IUrlShortenerRepo
{
    Task<string?> GetUrl(string shortName);
    Task UrlClicked(string shortName, string? ip);
    Task<ICollection<ShortenedUrlAnalytics>?> ShortenedUrlAnalytics(string shortName, DateTime after);
    Task<(string shortName, bool isNew)> CreateUrl(string url, string? shortName = null);
    Task<bool> DeleteShortUrl(string url);
}
