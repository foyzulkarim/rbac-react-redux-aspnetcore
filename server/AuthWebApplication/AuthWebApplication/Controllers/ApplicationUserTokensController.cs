using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthWebApplication.Models;
using AuthWebApplication.Models.Db;
using AuthWebApplication.Services;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace AuthWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserTokensController : ControllerBase
    {
        private readonly SecurityDbContext _context;
        private readonly RedisService redisService;

        public ApplicationUserTokensController(SecurityDbContext context, RedisService redisService)
        {
            _context = context;
            this.redisService = redisService;
        }

        // GET: api/ApplicationUserTokens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUserToken>>> GetApplicationUserTokens()
        {
            return await _context.ApplicationUserTokens.ToListAsync();
        }

        [HttpGet]
        [Route("Search")]
        public async Task<ActionResult<List<object>>> SearchTokens(string keyword)
        {
            var redisValues = await redisService.SearchRedisKeys(keyword);

            return Ok(redisValues);
        }

        // GET: api/ApplicationUserTokens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUserToken>> GetApplicationUserToken(string id)
        {
            var applicationUserToken = await _context.ApplicationUserTokens.FindAsync(id);

            if (applicationUserToken == null)
            {
                return NotFound();
            }

            return applicationUserToken;
        }

        // PUT: api/ApplicationUserTokens/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplicationUserToken(string id, ApplicationUserToken applicationUserToken)
        {
            if (id != applicationUserToken.UserId)
            {
                return BadRequest();
            }

            _context.Entry(applicationUserToken).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationUserTokenExists(id))
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

        // POST: api/ApplicationUserTokens
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ApplicationUserToken>> PostApplicationUserToken(ApplicationUserToken applicationUserToken)
        {
            _context.ApplicationUserTokens.Add(applicationUserToken);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ApplicationUserTokenExists(applicationUserToken.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetApplicationUserToken", new { id = applicationUserToken.UserId }, applicationUserToken);
        }

        // DELETE: api/ApplicationUserTokens/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplicationUserToken(string id)
        {
            var applicationUserToken = await _context.ApplicationUserTokens.FindAsync(id);
            if (applicationUserToken == null)
            {
                return NotFound();
            }

            _context.ApplicationUserTokens.Remove(applicationUserToken);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApplicationUserTokenExists(string id)
        {
            return _context.ApplicationUserTokens.Any(e => e.UserId == id);
        }
    }
}
