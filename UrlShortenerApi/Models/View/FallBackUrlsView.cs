using UrlShortenerApi.Models.Data;

namespace UrlShortenerApi.Models.View
{
    public class FallBackUrlsView
    {
        public int? ID { get; set; }
        public string FallBackUrl { get; set; }
        public int AccountId { get; set; }

    }
}
