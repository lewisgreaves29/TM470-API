namespace UrlShortenerApi.Models.Data
{
    public class CustomDomain
    {
        public int ID { get; set; }
        public string Domain { get; set; }
        public string VerificationCode { get; set; }
        public string DomainCertificate { get; set; }
        public DateTime CertificateDate { get; set; }
        public bool Validated { get; set; }
        public int AccountId { get; set; }

        // Navigation property for the Account foreign key relationship
        public Account Account { get; set; } = null;
    }
}
