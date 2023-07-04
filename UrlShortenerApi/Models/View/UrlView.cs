using Microsoft.AspNetCore.Mvc;
using UrlShortenerApi.Models.Data;

namespace UrlShortenerApi.Models.View
{
    public class UrlView
    {
        public UrlView(string originalUrl, int accountId, string shortenedUrl, string lookupKey)
        {
            this.OriginalUrl = originalUrl;
            this.AccountId = accountId;
            this.ShortenedUrl = shortenedUrl;
            this.LookupKey = lookupKey;
        }

        public string OriginalUrl { get; set; }
        public string? LookupKey { get; set; }

        public string? ShortenedUrl { get; set; }
        public int AccountId { get; set; }

    }
}
