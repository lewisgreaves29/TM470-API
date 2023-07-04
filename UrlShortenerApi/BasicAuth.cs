using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Text;
using System.Threading.Tasks;

namespace UrlShortenerApi.Filters
{
    public class BasicAuthFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authHeader = context.HttpContext.Request.Headers["Authorization"].ToString();
            if (authHeader != null && authHeader.StartsWith("Basic "))
            {
                // Get the API key from the account
                var apiKey = "your_api_key_here";

                var encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();
                var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));
                var username = decodedUsernamePassword.Split(':', 2)[0];
                var password = decodedUsernamePassword.Split(':', 2)[1];

                if (apiKey == password)
                {
                    // Authorized, do nothing
                    return;
                }
            }

            // Not authorized
            context.HttpContext.Response.Headers["WWW-Authenticate"] = "Basic realm=\"UrlShortenerApi\"";
            context.Result = new UnauthorizedResult();
        }
    }
}