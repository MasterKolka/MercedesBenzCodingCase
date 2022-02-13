using System.ComponentModel.DataAnnotations;
using UrlShortener.Validation;

namespace UrlShortener.Models;

/// <summary>
///     Create shortened url request
/// </summary>
public class CreateShortenedUrlRequest
{
    /// <summary>
    ///     Url to shorten
    /// </summary>
    [Required]
    [Url]
    public string Url { get; set; } = string.Empty;

    /// <summary>
    ///     Desired short name (optional)
    /// </summary>
    [MaxLength(64)]
    [ShortName]
    public string? ShortName { get; set; }
}
