namespace UrlShortener.Models
{
    /// <summary>
    ///     Shortened url info
    /// </summary>
    public class GetUrlResponse
    {
        /// <summary>
        ///     Full url which was shortened
        /// </summary>
        public string Url { get; set; } = string.Empty;
        /// <summary>
        ///     Short url
        /// </summary>
        public string ShortenedUrl { get; set; } = string.Empty;
        /// <summary>
        ///     Shortened url path (http://host/<b>ShortName</b>)
        /// </summary>
        public string ShortName { get; set; } = string.Empty;
    }
}
