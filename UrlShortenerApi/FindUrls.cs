using System.Text.RegularExpressions;

namespace UrlShortenerApi
{
    public class FindUrls
    {
        public static List<string> FindAllUrls(string text)
        {
            // Regular expression pattern to match URLs
            string pattern = @"((http|https):\/\/)?[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,}(\/\S*)?";

            // Compile the regular expression
            Regex regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            // Find all matches in the text
            MatchCollection matches = regex.Matches(text);

            // Create a list to store the matched URLs
            List<string> urls = new List<string>();

            // Loop over the matches and add them to the list
            foreach (Match match in matches)
            {
                urls.Add(match.Value);
            }

            // Return the list of URLs
            return urls;
        }
    }
}
