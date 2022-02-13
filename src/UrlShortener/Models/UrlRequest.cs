using System.ComponentModel.DataAnnotations;
using UrlShortener.Validation;

namespace UrlShortener.Models;

/// <summary>
///     Request by ShortName
/// </summary>
public class UrlRequest
{
    /// <summary>
    ///     Shortened url path (http://host/<b>ShortName</b>)
    /// </summary>
    [Required]
    [MaxLength(64)]
    [ShortName]
    public string ShortName { get; set; } = string.Empty;
}
