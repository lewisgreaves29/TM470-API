using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto;
using UrlShortenerApi.Models.Data;
using UrlShortenerApi.Models.View;
using AutoMapper;
using UrlShortenerApi.Profiles;

namespace UrlShortenerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly MyDbContext _dbContext;

        public UsersController(MyDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // GET /users
        [HttpGet("{accountid}")]
        public async Task<ActionResult<IEnumerable<UserView>>> GetUsers(int accountid)
        {
            var users = await _dbContext.Users.Where(c => c.AccountId == accountid).ToListAsync(); ;

            // Map the User objects to UserViewModel objects using AutoMapper
            var userViewModels = _mapper.Map<IEnumerable<UserView>>(users);

            return Ok(userViewModels);
        }

        // GET /users/{id}
        [HttpGet("{accountid}/{id}")]
        public async Task<ActionResult<UserView>> GetUser(int accountid, int id)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(cd => cd.ID == id && cd.AccountId == accountid); ;

            if (user == null)
            {
                return NotFound();
            }

            // Map the User object to a UserViewModel object using AutoMapper
            var userViewModel = _mapper.Map<UserView>(user);

            return Ok(userViewModel);
        }

        // POST /users
        [HttpPost]
        public async Task<ActionResult<UserView>> CreateUser(UserView user)
        {
            User userDataModel = _mapper.Map<User>(user);
            _dbContext.Users.Add(userDataModel);
            await _dbContext.SaveChangesAsync();


            return CreatedAtAction(nameof(GetUser), new { id = user.ID }, user);
        }

        // PUT /users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserView user)
        {
            if (id != user.ID)
            {
                return BadRequest();
            }
            User userDataModel = _mapper.Map<User>(user);
            _dbContext.Entry(userDataModel).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
  
                throw;
            }

            return NoContent();
        }

        // DELETE /users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }

}
