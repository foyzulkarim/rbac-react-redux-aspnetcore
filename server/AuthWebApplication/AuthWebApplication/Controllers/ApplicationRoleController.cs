using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthWebApplication.Models;
using AuthWebApplication.Models.Db;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationRoleController : ControllerBase
    {
        private SecurityDbContext db;

        public ApplicationRoleController(SecurityDbContext db)
        {
            this.db = db;
        }

        // GET: api/<ApplicationRoleController>
        [HttpGet]
        public IActionResult Get()
        {
            var roles = db.ApplicationRoles.ToList();
            return Ok(roles);
        }

        // GET api/<ApplicationRoleController>/5
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var role = db.ApplicationRoles.Find(id);
            if (role != null)
            {
                return Ok(role);
            }

            return NotFound();
        }

        // POST api/<ApplicationRoleController>
        [HttpPost]
        public IActionResult Post([FromBody] ApplicationRole role)
        {
            if (string.IsNullOrWhiteSpace(role.Name))
            {
                return BadRequest();
            }

            var entry = db.ApplicationRoles.Add(role);
            db.SaveChanges();
            return Ok();
        }

        // PUT api/<ApplicationRoleController>/5
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] string value)
        {
            var role = db.ApplicationRoles.Find(id);
            if (role!=null)
            {
                role.Name = value;
                db.SaveChanges();
                return Ok();
            }

            return NotFound();
        }

        // DELETE api/<ApplicationRoleController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var role = db.ApplicationRoles.Find(id);
            if (role != null)
            {
                db.ApplicationRoles.Remove(role);
                db.SaveChanges();
                return Ok();
            }

            return NotFound();
        }
    }
}
