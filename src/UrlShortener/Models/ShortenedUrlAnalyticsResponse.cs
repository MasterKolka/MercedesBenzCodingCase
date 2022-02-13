namespace UrlShortener.Models
{
    /// <summary>
    ///     Analytics response
    /// </summary>
    public class ShortenedUrlAnalyticsResponse
    {
        /// <summary>
        ///     <see cref="GetUrlResponse"/>
        /// </summary>
        public GetUrlResponse UrlInfo { get; set; } = new();

        /// <summary>
        ///     <see cref="ShortenedUrlAnalyticsItemResponse"/>
        /// </summary>
        public ICollection<ShortenedUrlAnalyticsItemResponse> AnayticsItems { get; set; } = Array.Empty<ShortenedUrlAnalyticsItemResponse>();
    }
}
