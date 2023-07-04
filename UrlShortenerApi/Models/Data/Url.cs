using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UrlShortenerApi.Models.Data
{
    public class Url
    {
        
        public string OriginalUrl { get; set; }
        [Key]
        public string? LookupKey { get; set; }

        public string? ShortenedUrl { get; set; }
        public int AccountId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Account Account { get; set; } = null;
    }
}
