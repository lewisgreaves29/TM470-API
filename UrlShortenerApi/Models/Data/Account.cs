using System.ComponentModel.DataAnnotations;

namespace UrlShortenerApi.Models.Data
{
    public class Account
    {
        public int ID { get; set; }
        public string AccountName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ApiKey { get; set; }

        public List<User> Users { get; set; }
        public FallBackUrls FallBackUrls { get; set; }
        public List<UrlExclusion> UrlExclusions { get; set; }
        public CustomDomain CustomDomains { get; set; }
        public List<Url> Urls { get; set; }
    }
}
