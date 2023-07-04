using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortenerApi.Models.Data;
using UrlShortenerApi.Models.View;

namespace UrlShortenerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomDomainsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly MyDbContext _dbContext;

        public CustomDomainsController(MyDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // GET /customdomains
        [HttpGet("{accountid}")]
        public async Task<ActionResult<IEnumerable<CustomDomainView>>> GetCustomDomains(int accountid)
        {
            var customDomains = await _dbContext.CustomDomains.Where(c => c.AccountId == accountid).ToListAsync(); ;
            var customDomainsViewModels = _mapper.Map<IEnumerable<CustomDomainView>>(customDomains);

            return Ok(customDomainsViewModels);
        }

        // GET /customdomains/accountid/{id}
        [HttpGet("{accountid}/{id}")]
        public async Task<ActionResult<CustomDomainView>> GetCustomDomain(int accountid, int id)
        {
            var customDomain = await _dbContext.CustomDomains.FirstOrDefaultAsync(cd => cd.ID == id && cd.AccountId == accountid);

            if (customDomain == null)
            {
                return NotFound();
            }

            var customDomainViewModel = _mapper.Map<CustomDomainView>(customDomain);

            return Ok(customDomainViewModel);
        }

        // POST /customdomains
        [HttpPost]
        public async Task<ActionResult<CustomDomainView>> CreateCustomDomain(CustomDomainView customDomain)
        {
            CustomDomain customDomainDataModel = _mapper.Map<CustomDomain>(customDomain);

            // Check for Duplicates
            var getAccountCustomDomain = await _dbContext.CustomDomains.FirstOrDefaultAsync(cd =>  cd.AccountId == customDomain.AccountId);
            if (getAccountCustomDomain != null)
            {
                return BadRequest("A domain already exists for that account");
            }

            var getCustomDomain = await _dbContext.CustomDomains.FirstOrDefaultAsync(cd => cd.Domain == customDomain.Domain);
            if (getCustomDomain != null)
            {
                return BadRequest("The entered Domain is not globally unique");
            }

            // Generate Verification code and set validated to false
            customDomainDataModel.VerificationCode = HelperServices.GenerateRandomString(32);
            customDomainDataModel.Validated = false;


            _dbContext.CustomDomains.Add(customDomainDataModel);
            await _dbContext.SaveChangesAsync();
            CustomDomainView customDomainReturnModel = _mapper.Map<CustomDomainView>(customDomainDataModel);
            return CreatedAtAction(nameof(CreateCustomDomain), customDomainReturnModel);
        }

        // PUT /customdomains/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomDomain(int id, CustomDomainView customDomain)
        {
            if (id != customDomain.ID)
            {
                return BadRequest();
            }

            CustomDomain customDomainDataModel = _mapper.Map<CustomDomain>(customDomain);
            _dbContext.Entry(customDomainDataModel).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_dbContext.CustomDomains.Any(c => c.ID == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE /customdomains/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomDomain(int id)
        {
            var customDomain = await _dbContext.CustomDomains.FindAsync(id);

            if (customDomain == null)
            {
                return NotFound();
            }

            _dbContext.CustomDomains.Remove(customDomain);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        // GET /customdomains/verify/{id}
        [HttpGet("/verify/{id}")]
        public async Task<IActionResult> VerifyCustomDomain(int id)
        {
            var customDomain = await _dbContext.CustomDomains.FirstOrDefaultAsync(cd => cd.ID == id);
            string domain = "_apiverification." + customDomain.Domain;
            HelperServices.CheckDnsRecord(domain, customDomain.VerificationCode.ToString());

            // At this stage we would add it to Azure

             // Add Custom Domain to the Azure WebApp

            // Add Certificate to the Keyvault

            // Bind certificate to the Custom Domain object inAzure

            if(HelperServices.CheckDnsRecord(domain, customDomain.VerificationCode.ToString()))
            {
                return Ok();
            }
            else
            {
                return BadRequest("Failed to validate domain");
            }
            
        }
    }

}
