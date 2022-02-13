namespace UrlShortener.Models
{
    /// <summary>
    ///     Shortened url analytics item
    /// </summary>
    public class ShortenedUrlAnalyticsItemResponse
    {
        /// <summary>
        ///     Redirect date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        ///     Client ip
        /// </summary>
        public string? Ip { get; set; }
    }
}
