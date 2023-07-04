using UrlShortenerApi;
using UrlShortenerApi.Models.Data;
using UrlShortenerApi.Models.View;

namespace UrlShortenerApiTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CreateUrl_ShouldReturnUrlWithShortenedUrl_WhenCalledWithValidData()
        {
            // Arrange
            var inputUrl = new UrlView { OriginalUrl = "https://originalurl.com" };
            var account = new AccountView { CustomDomains = null };

            // Act
            var result = HelperServices.CreateUrl(inputUrl, account);

            // Assert
            Assert.IsNotNull(result.ShortenedUrl);
            Assert.IsTrue(result.ShortenedUrl.StartsWith("https://api.shorterurl.uk"));
        }

        [Test]
        public void CreateUrl_ShouldUseCustomDomain_WhenAccountHasCustomDomain()
        {
            // Arrange
            var inputUrl = new UrlView { OriginalUrl = "https://originalurl.com" };
            var account = new AccountView { CustomDomains = new CustomDomain { Domain = "https://customdomain.com" } };

            // Act
            var result = HelperServices.CreateUrl(inputUrl, account);

            // Assert
            Assert.IsNotNull(result.ShortenedUrl);
            Assert.IsTrue(result.ShortenedUrl.StartsWith("https://customdomain.com"));
        }

        [TestCase("test@example.com", true)]
        [TestCase("test.test@example.com", true)]
        [TestCase("test@example", false)]
        [TestCase("test", false)]
        public void IsValidEmail_ShouldValidateEmailAddressesCorrectly(string email, bool expected)
        {
            // Act
            var result = HelperServices.IsValidEmail(email);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void FindAllUrls_ShouldFindAllUrlsInText()
        {
            // Arrange
            string text = "Check out my website at http://example.com. You can also visit my blog at https://blog.example.com/posts/123.";

            // Act
            List<string> result = FindUrls.FindAllUrls(text);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.Contains("http://example.com", result);
            Assert.Contains("https://blog.example.com/posts/123", result);
        }

        [Test]
        public void FindAllUrls_ShouldReturnEmptyList_WhenNoUrlsInText()
        {
            // Arrange
            string text = "This text does not contain any URLs.";

            // Act
            List<string> result = FindUrls.FindAllUrls(text);

            // Assert
            Assert.AreEqual(0, result.Count);
        }
    }
}
}