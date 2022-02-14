using Microsoft.AspNetCore.Mvc;
using UrlShortener.DataProvider.Repositories;
using UrlShortener.Models;

namespace UrlShortener.Controllers;

/// <summary>
///     Urls Management API
/// </summary>
[ApiController]
[Route("Management/[controller]")]
public class UrlController : ControllerBase
{
    private readonly IUrlShortenerRepo _urlShortenerRepo;

    /// <summary>
    ///     Constructor
    /// </summary>
    public UrlController(IUrlShortenerRepo urlShortenerRepo)
    {
        _urlShortenerRepo = urlShortenerRepo;
    }

    /// <summary>
    ///     Returns info about a shortened url
    /// </summary>
    /// <param name="request"><see cref="UrlRequest"/></param>
    [HttpGet]
    [ProducesResponseType(typeof(GetUrlResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<GetUrlResponse>> GetUrl([FromQuery] UrlRequest request)
    {
        var result = await _urlShortenerRepo.GetUrl(request.ShortName);
        if (result == null)
        {
            return NotFound();
        }
        return new GetUrlResponse
        {
            Url = result,
            ShortName = request.ShortName,
            ShortenedUrl = GetShortenedUrl(request.ShortName)
        };
    }

    /// <summary>
    ///     Creates shortened url.
    ///     If url doesnt exist in the database - creates a new record.
    ///     If url exists in the database - returns previously created shortened url.
    ///     If url exists and different ShortName was entered - creates a new shortened url with a desired name replacing an old one.
    /// </summary>
    /// <param name="request"><see cref="CreateShortenedUrlRequest"/></param>
    [HttpPost]
    [ProducesResponseType(typeof(GetUrlResponse), 200)]
    [ProducesResponseType(typeof(GetUrlResponse), 201)]
    [ProducesResponseType(400)]
    public async Task<ActionResult<GetUrlResponse>> CreateShortenedUrl(CreateShortenedUrlRequest request)
    {
        if (request.ShortName != null)
        {
            var url = await _urlShortenerRepo.GetUrl(request.ShortName);
            if (url != null)
            {
                return BadRequest($"ShortName {request.ShortName} is in use");
            }
        }

        var result = await _urlShortenerRepo.CreateUrl(request.Url, request.ShortName);

        return new ObjectResult(new GetUrlResponse
        {
            Url = request.Url,
            ShortName = result.shortName,
            ShortenedUrl = GetShortenedUrl(result.shortName)
        })
        { StatusCode = result.isNew ? StatusCodes.Status201Created : StatusCodes.Status200OK };
    }

    /// <summary>
    ///     Deletes a url if exists
    /// </summary>
    [HttpDelete]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult> DeleteUrl([FromQuery] UrlRequest request)
    {
        var result = await _urlShortenerRepo.DeleteShortUrl(request.ShortName);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }

    /// <summary>
    ///     Returns analytics for last 24h
    /// </summary>
    /// <param name="request"><see cref="UrlRequest"/></param>
    [HttpGet(nameof(GetAnalytics))]
    [ProducesResponseType(typeof(ShortenedUrlAnalyticsResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<ActionResult<ShortenedUrlAnalyticsResponse>> GetAnalytics([FromQuery] UrlRequest request)
    {
        var url = await _urlShortenerRepo.GetUrl(request.ShortName);
        if (url == null)
        {
            return NotFound();
        }
        var result = await _urlShortenerRepo.ShortenedUrlAnalytics(request.ShortName, DateTime.Now.AddHours(-24));

        return new ShortenedUrlAnalyticsResponse
        {
            UrlInfo = new()
            {
                ShortName = request.ShortName,
                ShortenedUrl = GetShortenedUrl(request.ShortName),
                Url = url
            },
            AnayticsItems = result!.Select(x => new ShortenedUrlAnalyticsItemResponse
            {
                Date = x.Date,
                Ip = x.Ip
            }).ToArray()
        };
    }

    private string GetShortenedUrl(string shortName)
    {
        return $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/{shortName}";
    }
}
