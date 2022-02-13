using Microsoft.EntityFrameworkCore;
using UrlShortener.DataProvider.Entities;
using UrlShortener.DataProvider.Helpers;

namespace UrlShortener.DataProvider.Repositories;

public class UrlShortenerRepo : IUrlShortenerRepo
{
    private readonly UrlShortenerContext context;

    public UrlShortenerRepo(UrlShortenerContext context)
    {
        this.context = context;
    }

    public async Task<string?> GetUrl(string shortName)
    {
        return (await context.ShortenedUrls.FirstOrDefaultAsync(y => y.Shortened == shortName))?.Url;
    }

    public async Task UrlClicked(string shortName, string? ip)
    {
        var shorenedUrl = context.ShortenedUrls.FirstOrDefault(x => x.Shortened == shortName);
        if (shorenedUrl == null)
        {
            return;
        }

        // storing info about what clicked, when and by whom
        context.ShortenedUrlAnalytics.Add(new()
        {
            ShortenedUrl = shorenedUrl,
            Date = DateTime.Now,
            Ip = ip
        });

        await context.SaveChangesAsync();
    }

    public async Task<ICollection<ShortenedUrlAnalytics>?> ShortenedUrlAnalytics(string shortName, DateTime after)
    {
        var shorenedUrl = context.ShortenedUrls.FirstOrDefault(x => x.Shortened == shortName);
        if (shorenedUrl == null)
        {
            return null;
        }

        // returning analytics data after some date
        return await context.ShortenedUrlAnalytics.Where(x => x.ShortenedUrl.Id == shorenedUrl.Id && x.Date > after).ToArrayAsync();
    }

    public async Task<string> CreateUrl(string url, string? shortName = null)
    {
        var normalizedUrl = NormalizeUrl(url);
        var existingItem = await context.ShortenedUrls.FirstOrDefaultAsync(x => x.Url == normalizedUrl);

        // if url exists with the same short name (or shortName param is null) then just return shortened version
        if (existingItem != null && (shortName == null || shortName == existingItem.Shortened))
        {
            return existingItem.Shortened;
        }

        string shortened = "";
        if (shortName != null)
        {
            // reassigning a url to another shortened version
            if (existingItem != null && shortName != existingItem.Shortened)
            {
                RemoveShortenedUrl(existingItem);
            }

            shortened = shortName;

            if (await context.ShortenedUrls.AnyAsync(x => x.Shortened == shortName))
            {
                throw new Exception("Shortened name is in use");
            }
            // if shortname is intersecting with a hashes range
            if (HashAlphabetHelper.ShortenedUrlRegex.IsMatch(shortName))
            {
                // converting a desired name to the number and searching for a range containing that value
                var number = HashAlphabetHelper.HashStringToNumber(shortName);
                var range = await context.HashingSpace.FirstOrDefaultAsync(x => x.RangeStart <= number && x.RangeEnd >= number);
                if (range == null)
                    throw new Exception("Range is missing, unexpected"); // shouldn't happen if there're no bugs

                // range contains only one number => removing it
                if (range.RangeStart == range.RangeEnd)
                {
                    context.HashingSpace.Remove(range);
                }
                // reducing range from the beginning
                if (range.RangeStart == number)
                {
                    range.RangeStart += 1;
                }
                // reducing range from the end
                else if (range.RangeEnd == number)
                {
                    range.RangeEnd -= 1;
                }
                // spltting range
                else
                {
                    context.HashingSpace.Add(new() { RangeStart = number + 1, RangeEnd = range.RangeEnd });

                    range.RangeEnd = number - 1;
                }
            }

            context.ShortenedUrls.Add(new() { Shortened = shortName, Url = normalizedUrl });
        }
        else
        {
            // searching for any range to take one value from it
            var range = await context.HashingSpace.FirstOrDefaultAsync();
            if (range == null)
            {
                throw new Exception("Hashing space is full");
            }

            var nextIndex = range.RangeStart;

            // range contains only one number => removing it
            if (range.RangeStart == range.RangeEnd)
            {
                context.HashingSpace.Remove(range);
            }
            // reducing range from the beginning
            else
            {
                range.RangeStart += 1;
            }

            // converting a number to the hash and storing it in the database
            shortened = HashAlphabetHelper.NumberToHashString(nextIndex);
            context.ShortenedUrls.Add(new() { Shortened = shortened, Url = normalizedUrl });
        }
        await context.SaveChangesAsync();

        return shortened;
    }

    public async Task<bool> DeleteShortUrl(string shortName)
    {
        var itemToRemove = await context.ShortenedUrls.FirstOrDefaultAsync(x => x.Shortened == shortName);
        if (itemToRemove == null)
        {
            return false;
        }

        RemoveShortenedUrl(itemToRemove);
        await context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    ///     Removes uel record + frees a hash space. Requires context.SaveChanges after this method to save results
    /// </summary>
    private void RemoveShortenedUrl(ShortenedUrl item)
    {
        // if shortname is intersecting with a hashes range, returning this value to the hashing space
        // could be optimized by adding a range zipping logic.
        if (HashAlphabetHelper.ShortenedUrlRegex.IsMatch(item.Shortened))
        {
            var number = HashAlphabetHelper.HashStringToNumber(item.Shortened);
            context.HashingSpace.Add(new() { RangeStart = number, RangeEnd = number });
        }

        context.ShortenedUrls.Remove(item);
    }

    private string NormalizeUrl(string url)
    {
        return new Uri(url).ToString();
    }
}
