using Microsoft.AspNetCore.Mvc;
using UrlShortenerApi.Models.Data;

namespace UrlShortenerApi.Models.View
{
    public class UrlInputView
    {
        public string OriginalUrl { get; set; }

        public int AccountId { get; set; }

    }
}
