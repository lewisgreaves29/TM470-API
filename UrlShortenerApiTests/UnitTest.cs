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
        public void CreateUrl_Test()
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

        [TestCase("test@test.com", true)]
        [TestCase("test.test@test.com", true)]
        [TestCase("test@test", false)]
        [TestCase("test", false)]
        public void IsValidEmail_Test(string email, bool expected)
        {

            var result = HelperServices.IsValidEmail(email);


            Assert.AreEqual(expected, result);
        }

        [Test]
        public void FindAllUrls_Test()
        {

            string text = "Your Best Super Market parcel is out for delivery. If you'd like more information, please click on the tracking link: https://react.sorted.com/tracking/track-order?customer_id=cs_612658752036853&shipment_id=sp_692770110189947390 If you would Like to order more please visit https://www.bestsupermarket.com";

            List<string> result = FindUrls.FindAllUrls(text);

            Assert.Contains("https://react.sorted.com/tracking/track-order?customer_id=cs_612658752036853&shipment_id=sp_692770110189947390", result);
            Assert.Contains("https://www.bestsupermarket.com", result);
        }

        [Test]
        public void FindAllUrls_Empty_Test()
        {

            string text = "This text does not contain any URLs.";

            List<string> result = FindUrls.FindAllUrls(text);

            Assert.AreEqual(0, result.Count);
        }
    }
}
}