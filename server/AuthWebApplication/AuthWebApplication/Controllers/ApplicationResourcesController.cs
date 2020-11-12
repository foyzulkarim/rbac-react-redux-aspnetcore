using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthWebApplication.Models;
using AuthWebApplication.Models.Db;

namespace AuthWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationResourcesController : ControllerBase
    {
        private readonly SecurityDbContext _context;

        public ApplicationResourcesController(SecurityDbContext context)
        {
            _context = context;
        }

        // GET: api/ApplicationResources
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationResource>>> GetResources()
        {
            return await _context.Resources.ToListAsync();
        }

        // GET: api/ApplicationResources/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationResource>> GetApplicationResource(string id)
        {
            var applicationResource = await _context.Resources.FindAsync(id);

            if (applicationResource == null)
            {
                return NotFound();
            }

            return applicationResource;
        }

        // PUT: api/ApplicationResources/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplicationResource(string id, ApplicationResource applicationResource)
        {
            if (id != applicationResource.Id)
            {
                return BadRequest();
            }

            _context.Entry(applicationResource).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationResourceExists(id))
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

        // POST: api/ApplicationResources
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ApplicationResource>> PostApplicationResource(ApplicationResource applicationResource)
        {
            _context.Resources.Add(applicationResource);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ApplicationResourceExists(applicationResource.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetApplicationResource", new { id = applicationResource.Id }, applicationResource);
        }

        // DELETE: api/ApplicationResources/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApplicationResource>> DeleteApplicationResource(string id)
        {
            var applicationResource = await _context.Resources.FindAsync(id);
            if (applicationResource == null)
            {
                return NotFound();
            }

            _context.Resources.Remove(applicationResource);
            await _context.SaveChangesAsync();

            return applicationResource;
        }

        private bool ApplicationResourceExists(string id)
        {
            return _context.Resources.Any(e => e.Id == id);
        }
    }
}
