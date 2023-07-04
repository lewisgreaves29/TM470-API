using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UrlShortenerApi.Models.Data;

namespace UrlShortenerApi.Models.View
{
    public class UserView
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required]
        public int AccountId { get; set; }
        [PasswordPropertyText]
        public string UserPassword { get; set; }

    }
}
