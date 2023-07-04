using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortenerApi.Models.Data;
using UrlShortenerApi.Models.View;

namespace UrlShortenerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UrlExclusionsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly MyDbContext _dbContext;

        public UrlExclusionsController(MyDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // GET /urlexclusions
        [HttpGet("{accountid}")]
        public async Task<ActionResult<IEnumerable<UrlExclusionView>>> GetUrlExclusions(int accountid)
        {
            var urlExclusions = await _dbContext.UrlExclusions.Where(c => c.AccountId == accountid).ToListAsync(); ;
            var urlExclusionsViewModels = _mapper.Map<IEnumerable<UrlExclusionView>>(urlExclusions);
            return Ok(urlExclusionsViewModels);
        }

        // GET /urlexclusions/{id}
        [HttpGet("{accountid}/{id}")]
        public async Task<ActionResult<UrlExclusionView>> GetUrlExclusion(int accountid, int id)
        {
            var urlExclusion = await _dbContext.UrlExclusions.Include(u => u.Account)
                .FirstOrDefaultAsync(u => u.ID == id);
            if (urlExclusion == null)
            {
                return NotFound();
            }
            var urlExclusionViewModel = _mapper.Map<UrlExclusionView>(urlExclusion);
            return Ok(urlExclusionViewModel);
        }

        // POST /urlexclusions
        [HttpPost]
        public async Task<ActionResult<UrlExclusionView>> CreateUrlExclusion(UrlExclusionView urlExclusion)
        {
            UrlExclusion urlExclusionDataModel = _mapper.Map<UrlExclusion>(urlExclusion);
            _dbContext.UrlExclusions.Add(urlExclusionDataModel);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUrlExclusion), new { id = urlExclusion.ID }, urlExclusion);
        }

        // PUT /urlexclusions/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUrlExclusion(int id, UrlExclusionView urlExclusion)
        {
            if (id != urlExclusion.ID)
            {
                return BadRequest();
            }
            UrlExclusion urlExclusionDataModel = _mapper.Map<UrlExclusion>(urlExclusion);
            _dbContext.Entry(urlExclusionDataModel).State = EntityState.Modified;
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_dbContext.UrlExclusions.Any(u => u.ID == id))
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

        // DELETE /urlexclusions/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUrlExclusion(int id)
        {
            var urlExclusion = await _dbContext.UrlExclusions.FindAsync(id);
            if (urlExclusion == null)
            {
                return NotFound();
            }
            _dbContext.UrlExclusions.Remove(urlExclusion);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }

}
