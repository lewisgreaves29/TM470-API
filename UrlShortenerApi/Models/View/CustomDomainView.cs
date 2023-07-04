using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using UrlShortenerApi.Models.Data;

namespace UrlShortenerApi.Models.View
{
    public class CustomDomainView
    {
        public int ID { get; set; }
        public string Domain { get; set; }
        public string? VerificationCode { get; set; }
        public string DomainCertificate { get; set; }
        public DateTime CertificateDate { get; set; }
        public bool? Validated { get; set; }
        public int AccountId { get; set; }

    }
}
