using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using UrlShortenerApi.Models.Data;
using UrlShortenerApi.Models.View;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Http.Extensions;
using System.Collections.Generic;

namespace UrlShortenerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShortenerController : ControllerBase
    {
        private readonly MyDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShortenerController(MyDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }


        // GET /healthcheck
        [HttpGet("/")]
        public async Task<ActionResult> healthcheck()
        {
            return Ok();
        }

        // GET /redirect
        [HttpGet("/{id}")]
        public async Task<ActionResult> UrlRedirectAsync(string id)
        {
            // Attempt to retrieve orginal URL from Database
            Url url = await _dbContext.Urls.FindAsync(id);

            // Check that URL exists
            if (url == null)
            {
                // Try find account from Custom Domain
                string currentUrl = _httpContextAccessor.HttpContext.Request.GetEncodedUrl();
                int delimiterPosition = currentUrl.IndexOf(id);
                string baseUrl = currentUrl.Substring(0, (delimiterPosition - 1)).Trim();
                CustomDomain customDomain = _dbContext.CustomDomains.Where(c => c.Domain == baseUrl).FirstOrDefault();
                if(customDomain != null)
                {
                    Account account = await _dbContext.Accounts.Include(a => a.FallBackUrls)
                        .FirstOrDefaultAsync(a => a.ID == customDomain.AccountId);
                    if (account.FallBackUrls.FallBackUrl == null)
                    {
                        return NotFound();
                    }
                    // Return the User to the Fall Back Urls
                    return Redirect(account.FallBackUrls.FallBackUrl);
                }
                
                return NotFound();
            }
            // Redirect to Original URL
            return Redirect(url.OriginalUrl);
        }


        // POST /basic
        [HttpPost("/basic")]
        public async Task<ActionResult<Url>> CreateBasicLink(UrlView url)
        {
            var account = await _dbContext.Accounts.Include(a => a.Users)
                .Include(a => a.FallBackUrls)
                .Include(a => a.UrlExclusions)
                .Include(a => a.CustomDomains)
                .FirstOrDefaultAsync(a => a.ID == url.AccountId);
            if (account == null)
            {
                return NotFound();
            }

            // Map the User object to a UserViewModel object using AutoMapper
            AccountView accountViewModel = _mapper.Map<AccountView>(account);
            
            // Check Url is not in the exclusions list
            if(accountViewModel.UrlExclusions != null)
            {
                if(accountViewModel.UrlExclusions.Any(e => e.ExcludedUrl == url.OriginalUrl))
                {
                    return BadRequest("Url is in the excluded list");
                }
            }
            
            UrlView newUrlViewModel = HelperServices.CreateUrl(url, accountViewModel);
            // Add to Database
            Models.Data.Url urlDataModel = _mapper.Map<Models.Data.Url>(newUrlViewModel);
            _dbContext.Urls.Add(urlDataModel);
            await _dbContext.SaveChangesAsync();

            return Ok(newUrlViewModel.ShortenedUrl);
        }

        // POST /bulk
        [HttpPost("/bulk")]
        public async Task<ActionResult<List<UrlView>>> CreateBulkLink(List<UrlView> InputUrls)
        {
            var account = await _dbContext.Accounts.Include(a => a.Users)
                .Include(a => a.FallBackUrls)
                .Include(a => a.UrlExclusions)
                .Include(a => a.CustomDomains)
                .FirstOrDefaultAsync(a => a.ID == InputUrls.First().AccountId);

            if (account == null)
            {
                return NotFound();
            }
            AccountView accountViewModel = _mapper.Map<AccountView>(account);
            List<UrlView> newUrls = new List<UrlView>();

            foreach (UrlView url in InputUrls)
            {
                // Check if url is in exclusion list
                if (accountViewModel.UrlExclusions != null)
                {
                    if (!accountViewModel.UrlExclusions.Any(e => e.ExcludedUrl == url.OriginalUrl))
                    {
                        UrlView newUrlViewModel = HelperServices.CreateUrl(url, accountViewModel);

                        // Add to Database
                        Models.Data.Url urlDataModel = _mapper.Map<Models.Data.Url>(newUrlViewModel);
                        _dbContext.Urls.Add(urlDataModel);
                        await _dbContext.SaveChangesAsync();
                        newUrls.Add(newUrlViewModel);
                    }
                    else
                    {
                        url.ShortenedUrl = url.OriginalUrl;

                        newUrls.Add(url);
                    }
                }
                
            }

            return Ok(newUrls);
        }

        // POST /intelligent
        [HttpPost("/{accountId}/intelligent")]
        public async Task<ActionResult<String>> CreateIntelligentLink(int accountId,String inputString)
        {
            var account = await _dbContext.Accounts.Include(a => a.Users)
                .Include(a => a.FallBackUrls)
                .Include(a => a.UrlExclusions)
                .Include(a => a.CustomDomains)
                .FirstOrDefaultAsync(a => a.ID == accountId);
            if (account == null)
            {
                return NotFound();
            }

            AccountView accountViewModel = _mapper.Map<AccountView>(account);
            List<string> formattedUrls = FindUrls.FindAllUrls(inputString);
            List<UrlView> newUrls = new List<UrlView>();

            foreach (string url in formattedUrls)
            {
                UrlView newUrlViewModel = new UrlView(url, accountId, null, null);
                if (accountViewModel.UrlExclusions != null)
                {
                    if (!accountViewModel.UrlExclusions.Any(e => e.ExcludedUrl == newUrlViewModel.OriginalUrl))
                    {
                        newUrlViewModel = HelperServices.CreateUrl(newUrlViewModel, accountViewModel);

                        // Add to Database
                        Models.Data.Url urlDataModel = _mapper.Map<Models.Data.Url>(newUrlViewModel);
                        _dbContext.Urls.Add(urlDataModel);
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        newUrlViewModel.ShortenedUrl = url;
                    }
                }
                newUrls.Add(newUrlViewModel);
            }

            // Update orginal string
            foreach(UrlView newUrl in newUrls)
            {
                inputString = inputString.Replace(newUrl.OriginalUrl, newUrl.ShortenedUrl);
            }

            return Ok(inputString);
        }

        // DELETE /accounts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLink(int id)
        {
            return NoContent();
        }

    }

}
