using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthWebApplication.Models;
using AuthWebApplication.Models.Db;
using AuthWebApplication.Models.ViewModels;

namespace AuthWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationPermissionsController : ControllerBase
    {
        private readonly SecurityDbContext _context;

        public ApplicationPermissionsController(SecurityDbContext context)
        {
            _context = context;
        }

        // GET: api/ApplicationPermissions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationPermissionViewModel>>> GetPermissions()
        {
            return await _context.Permissions.Include(x=>x.Role).Include(x=>x.Resource).Select(x=>new ApplicationPermissionViewModel(x)).ToListAsync();
        }

        // GET: api/ApplicationPermissions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationPermission>> GetApplicationPermission(string id)
        {
            var applicationPermission = await _context.Permissions.FindAsync(id);

            if (applicationPermission == null)
            {
                return NotFound();
            }

            return applicationPermission;
        }

        // PUT: api/ApplicationPermissions/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplicationPermission(string id, ApplicationPermission applicationPermission)
        {
            if (id != applicationPermission.Id)
            {
                return BadRequest();
            }

            _context.Entry(applicationPermission).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationPermissionExists(id))
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

        // POST: api/ApplicationPermissions
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<ApplicationPermission>> PostApplicationPermission(ApplicationPermission applicationPermission)
        {
            _context.Permissions.Add(applicationPermission);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ApplicationPermissionExists(applicationPermission.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetApplicationPermission", new { id = applicationPermission.Id }, applicationPermission);
        }

        // DELETE: api/ApplicationPermissions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApplicationPermission>> DeleteApplicationPermission(string id)
        {
            var applicationPermission = await _context.Permissions.FindAsync(id);
            if (applicationPermission == null)
            {
                return NotFound();
            }

            _context.Permissions.Remove(applicationPermission);
            await _context.SaveChangesAsync();

            return applicationPermission;
        }

        private bool ApplicationPermissionExists(string id)
        {
            return _context.Permissions.Any(e => e.Id == id);
        }
    }
}
