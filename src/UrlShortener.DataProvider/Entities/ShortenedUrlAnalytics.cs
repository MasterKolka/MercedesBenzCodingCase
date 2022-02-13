using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using UrlShortener.DataProvider.Entities.Base;

namespace UrlShortener.DataProvider.Entities;

[Index(nameof(Date))]
public class ShortenedUrlAnalytics: Entity
{
    [Required]
    public DateTime Date { get; set; }

    public string? Ip { get; set; }

    [Required]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public virtual ShortenedUrl ShortenedUrl { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
