using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace UrlShortenerApi.Models.Data
{
    public class User
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime CreatedDate { get; set; }
        [ForeignKey("Account")]
        public int AccountId { get; set; }
        public string UserPassword { get; set; }

        // Navigation property for the Account foreign key relationship
        [HiddenInput(DisplayValue = false)]
        public Account Account { get; set; } = null;
    }
}
