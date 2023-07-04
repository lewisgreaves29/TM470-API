using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortenerApi.Models.Data;
using UrlShortenerApi.Models.View;

namespace UrlShortenerApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FallBackUrlsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly MyDbContext _dbContext;

        public FallBackUrlsController(MyDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        // GET /fallbackurls
        [HttpGet("{accountid}")]
        public async Task<ActionResult<IEnumerable<FallBackUrlsView>>> GetFallBackUrls(int accountid)
        {
            var fallBackUrls = await _dbContext.FallBackUrls.Where(c => c.AccountId == accountid).ToListAsync();
            var fallBackUrlsViewModels = _mapper.Map<IEnumerable<FallBackUrlsView>>(fallBackUrls);

            return Ok(fallBackUrlsViewModels);
        }

        // GET /fallbackurls/{id}
        [HttpGet("{accountid}/{id}")]
        public async Task<ActionResult<FallBackUrlsView>> GetFallBackUrl(int accountid, int id)
        {
            var fallBackUrl = await _dbContext.FallBackUrls.FirstOrDefaultAsync(cd => cd.ID == id && cd.AccountId == accountid);

            if (fallBackUrl == null)
            {
                return NotFound();
            }

            var fallBackUrlViewModel = _mapper.Map<IEnumerable<FallBackUrlsView>>(fallBackUrl);

            return Ok(fallBackUrlViewModel);
        }

        // POST /fallbackurls
        [HttpPost]
        public async Task<ActionResult<FallBackUrlsView>> CreateFallBackUrl(FallBackUrlsView fallBackUrl)
        {
            FallBackUrls fallBackUrlDataModel = _mapper.Map<FallBackUrls>(fallBackUrl);
            _dbContext.FallBackUrls.Add(fallBackUrlDataModel);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFallBackUrl), new { id = fallBackUrl.ID }, fallBackUrl);
        }

        // PUT /fallbackurls/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFallBackUrl(int id, FallBackUrlsView fallBackUrl)
        {
            if (id != fallBackUrl.ID)
            {
                return BadRequest();
            }

            FallBackUrls fallBackUrlsDataModel = _mapper.Map<FallBackUrls>(fallBackUrl);
            _dbContext.Entry(fallBackUrlsDataModel).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_dbContext.FallBackUrls.Any(f => f.ID == id))
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

        // DELETE /fallbackurls/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFallBackUrl(int id)
        {
            var fallBackUrl = await _dbContext.FallBackUrls.FindAsync(id);

            if (fallBackUrl == null)
            {
                return NotFound();
            }

            _dbContext.FallBackUrls.Remove(fallBackUrl);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
