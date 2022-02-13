using UrlShortener.DataProvider.Repositories;

namespace UrlShortener.Middlewares;

/// <summary>
///     Middleware to handle shortened urls redirection
/// </summary>
public class ShortenedUrlRedirectorMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    ///     Constructor
    /// </summary>
    public ShortenedUrlRedirectorMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    ///     Middleware requests processor
    /// </summary>
    public async Task Invoke(HttpContext context, IUrlShortenerRepo urlShortenerRepo)
    {
        // if method is GET and there is only one slash in the path => try to redirect
        if (context.Request.Method == HttpMethod.Get.ToString()
            && context.Request.Path.HasValue && context.Request.Path.Value.Count(x => x == '/') == 1)
        {
            // removing slash from the path
            var shortened = context.Request.Path.Value.Substring(1);

            var url = await urlShortenerRepo.GetUrl(shortened);
            if (url != null)
            {
                // analytics
                await urlShortenerRepo.UrlClicked(shortened, context.Connection.RemoteIpAddress?.ToString());
                context.Response.Redirect(url);
            }
            else
            {
                context.Response.StatusCode = 404;
            }
        }
        else
        {
            await _next(context);
        }
    }
}

/// <summary>
///     Extension
/// </summary>
public static class ShortenedUrlRedirectorMiddlewareExtensions
{
    /// <summary>
    ///     Extension method used to add the middleware to the HTTP request pipeline.
    /// </summary>
    public static IApplicationBuilder UseShortenedUrlRedirectorMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ShortenedUrlRedirectorMiddleware>();
    }
}
