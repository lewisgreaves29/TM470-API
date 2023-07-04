using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using UrlShortenerApi.Models.View;

namespace UrlShortenerApi
{
    public static class HelperServices
    {
        public static UrlView CreateUrl(UrlView inputUrl, AccountView account)
        {
            inputUrl.LookupKey = GenerateRandomString(10);
            string domain;

            // Check if custom domain exists
            if (account.CustomDomains == null)
            {
                domain = "https://api.shorterurl.uk";
            }
            else
            {
                domain = account.CustomDomains.Domain.ToLower();
            }
            string newUrl = string.Concat(domain, "/");
            string fullUrl = string.Concat(newUrl, inputUrl.LookupKey);
            inputUrl.ShortenedUrl = fullUrl;

            return (inputUrl);
        }

        public static string GenerateRandomString(int length)
        {
            Random random = new Random();
            const string chars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            StringBuilder sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                int index = random.Next(chars.Length);
                sb.Append(chars[index]);
            }

            return sb.ToString();
        }
        public static bool IsValidEmail(string email)
        {
            string pattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
            Regex regex = new Regex(pattern);
            return regex.IsMatch(email);
        }

        public static bool CheckDnsRecord(string domainName, string verifiactionCode)
        {
            string txtRecord = string.Empty;

            try
            {
                var dnsQueryResult = Dns.GetHostEntry(domainName);
                foreach (var dnsRecord in dnsQueryResult?.AddressList)
                {
                    txtRecord += dnsRecord + ";";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error querying DNS TXT record for {domainName}: {ex.Message}");
            }

            if (txtRecord.Contains(verifiactionCode))
            {
                Console.WriteLine("Found!");
                return true;
            }
            else
            {
                Console.WriteLine("Not found.");
                return false;
            }
        }
    }
}
