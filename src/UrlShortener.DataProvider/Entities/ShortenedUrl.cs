using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using UrlShortener.DataProvider.Entities.Base;

namespace UrlShortener.DataProvider.Entities;

[Index(nameof(Shortened), IsUnique =true)]
[Index(nameof(Url), IsUnique =true)]
public class ShortenedUrl: Entity
{
    [Required]
    public string Shortened { get; set; } = string.Empty;

    [Required]
    public string Url { get; set; } = string.Empty;
}
