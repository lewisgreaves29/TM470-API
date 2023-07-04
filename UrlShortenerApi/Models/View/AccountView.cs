using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using UrlShortenerApi.Models.Data;

namespace UrlShortenerApi.Models.View
{
    public class AccountView
    {
        public int? ID { get; set; }
        public string AccountName { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ApiKey { get; set; }

        // Navigation properties for related entities
        public List<UserView>? Users { get; set; }
        public FallBackUrlsView? FallBackUrls { get; set; }
        public List<UrlExclusionView>? UrlExclusions { get; set; }
        public CustomDomainView? CustomDomains { get; set; }

        public List<UrlView>? Urls { get; set; }
    }
}
