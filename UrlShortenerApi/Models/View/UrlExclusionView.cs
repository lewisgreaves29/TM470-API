using UrlShortenerApi.Models.Data;

namespace UrlShortenerApi.Models.View
{
    public class UrlExclusionView
    {
        public int? ID { get; set; }
        public string ExcludedUrl { get; set; }
        public int AccountId { get; set; }

    }
}
