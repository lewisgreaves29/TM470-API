using UrlShortenerApi.Models.View;

namespace UrlShortenerApi.Models.Data
{
    public class FallBackUrls
    {
        public int ID { get; set; }
        public string FallBackUrl { get; set; }
        public int AccountId { get; set; }

        // Navigation property for the Account foreign key relationship
        public Account Account { get; set; } = null;
    }
}
