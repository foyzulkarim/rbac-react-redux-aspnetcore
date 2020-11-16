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
    public class ApplicationRolesController : ControllerBase
    {
        private readonly SecurityDbContext _context;

        public ApplicationRolesController(SecurityDbContext context)
        {
            _context = context;
        }

        // GET: api/ApplicationRoles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationRole>>> GetApplicationRoles()
        {
            List<ApplicationRole> roles = await _context.ApplicationRoles.ToListAsync();
            ApplicationRole applicationRole = roles.FirstOrDefault(x => x.Name == "SuperAdmin");
            roles.Remove(applicationRole);
            return roles;
        }

        // GET: api/ApplicationRoles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationRole>> GetApplicationRole(string id)
        {
            var applicationRole = await _context.ApplicationRoles.FindAsync(id);

            if (applicationRole == null)
            {
                return NotFound();
            }

            return applicationRole;
        }

        // PUT: api/ApplicationRoles/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplicationRole(string id, ApplicationRole applicationRole)
        {
            if (id != applicationRole.Id)
            {
                return BadRequest();
            }

            _context.Entry(applicationRole).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationRoleExists(id))
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

        // POST: api/ApplicationRoles
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ApplicationRole>> PostApplicationRole(ApplicationRole applicationRole)
        {
            _context.ApplicationRoles.Add(applicationRole);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ApplicationRoleExists(applicationRole.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetApplicationRole", new { id = applicationRole.Id }, applicationRole);
        }

        // DELETE: api/ApplicationRoles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApplicationRole>> DeleteApplicationRole(string id)
        {
            var applicationRole = await _context.ApplicationRoles.FindAsync(id);
            if (applicationRole == null)
            {
                return NotFound();
            }

            _context.ApplicationRoles.Remove(applicationRole);
            await _context.SaveChangesAsync();

            return applicationRole;
        }

        private bool ApplicationRoleExists(string id)
        {
            return _context.ApplicationRoles.Any(e => e.Id == id);
        }
    }
}
