using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mail;
using System.Text;
using UrlShortenerApi.Filters;
using UrlShortenerApi.Models.Data;
using UrlShortenerApi.Models.View;

namespace UrlShortenerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly MyDbContext _dbContext;
        private readonly IMapper _mapper;

        public AccountsController(MyDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // GET /accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountView>>> GetAccounts()
        {
            var accounts = await _dbContext.Accounts.Include(a => a.Users)
                .Include(a => a.FallBackUrls)
                .Include(a => a.UrlExclusions)
                .Include(a => a.CustomDomains)
                .ToListAsync();
            var accountsViewModels = _mapper.Map<IEnumerable<AccountView>>(accounts);
            return Ok(accountsViewModels);
        }

        // GET /accounts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var account = await _dbContext.Accounts.Include(a => a.Users)
                .Include(a => a.FallBackUrls)
                .Include(a => a.UrlExclusions)
                .Include(a => a.CustomDomains)
                .FirstOrDefaultAsync(a => a.ID == id);

            if (account == null)
            {
                return NotFound();
            }

            var accountViewModel = _mapper.Map<AccountView>(account);

            return Ok(accountViewModel);
        }

        // POST /accounts
        [HttpPost]
        public async Task<ActionResult<AccountView>> CreateAccount(AccountView account)
        {
            Account accountDataModel = _mapper.Map<Account>(account);

            // Check to ensure valid email address
            if (!HelperServices.IsValidEmail(account.EmailAddress))
            {
                return BadRequest("Please enter a valid email address");
            }

            // Check to Ensure Email address is unique
            var existingAccount = _dbContext.Accounts.FirstOrDefault(a => a.EmailAddress == account.EmailAddress);
            if (existingAccount != null)
            {
                return Conflict("An account with that email address already exists");
            }

            // Check to ensure acconut name is unique
            var existingAccountName = _dbContext.Accounts.FirstOrDefault(a => a.AccountName == account.AccountName);
            if (existingAccountName != null)
            {
                return Conflict("An account with that name already exists");
            }

            // Generate API Key
            byte[] inputBytes = Encoding.UTF8.GetBytes(HelperServices.GenerateRandomString(32));
            accountDataModel.ApiKey = Convert.ToBase64String(inputBytes);

            _dbContext.Accounts.Add(accountDataModel);
            await _dbContext.SaveChangesAsync();

            AccountView accountViewModel = _mapper.Map<AccountView>(accountDataModel);
            return CreatedAtAction(nameof(CreateAccount), accountViewModel);
        }


        // PUT /accounts/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(int id, AccountView account)
        {
            if (id != account.ID)
            {
                return BadRequest();
            }

            Account accountDataModel = _mapper.Map<Account>(account);
            _dbContext.Entry(account).State = EntityState.Modified;
            
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_dbContext.Accounts.Any(a => a.ID == id))
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

        // DELETE /accounts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _dbContext.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            _dbContext.Accounts.Remove(account);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }

}
