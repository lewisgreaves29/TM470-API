namespace UrlShortenerApi.Models.Data
{
    public class UrlExclusion
    {
        public int ID { get; set; }
        public string ExcludedUrl { get; set; }
        public int AccountId { get; set; }

        // Navigation property for the Account foreign key relationship
        public Account Account { get; set; } = null;
    }
}
