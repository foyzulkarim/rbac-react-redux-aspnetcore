using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthWebApplication.Models;
using AuthWebApplication.Models.Db;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace AuthWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SecurityDbContext _context;
        private UserManager<ApplicationUser> userManager;

        public UsersController(SecurityDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> GetApplicationUser(string id)
        {
            var applicationUser = await _context.Users.FindAsync(id);

            if (applicationUser == null)
            {
                return NotFound();
            }

            return applicationUser;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplicationUser(string id, ApplicationUser applicationUser)
        {
            if (id != applicationUser.Id)
            {
                return BadRequest();
            }

            bool modelIsInvalid = await ValidateUpdateModel(applicationUser);
            if (modelIsInvalid)
            {
                return BadRequest(ModelState);
            }

            var user = await this.userManager.FindByIdAsync(id);
            user.UserName = applicationUser.UserName;
            user.Email = applicationUser.Email;
            user.FirstName = applicationUser.FirstName;
            user.LastName = applicationUser.LastName;
            user.PhoneNumber = applicationUser.PhoneNumber;
            user.IsActive = applicationUser.IsActive;

            var updateUserResult = await this.userManager.UpdateAsync(user);
            if (!updateUserResult.Succeeded)
            {
                IActionResult actionResult = GetErrorResult(updateUserResult);
                return actionResult;
            }

            if (!string.IsNullOrEmpty(applicationUser.Password) && !string.IsNullOrEmpty(applicationUser.NewPassword))
            {
                var passwordUpdateResult = await this.userManager.ChangePasswordAsync(user, applicationUser.Password, applicationUser.NewPassword);
                if (!passwordUpdateResult.Succeeded)
                {
                    IActionResult actionResult = GetErrorResult(passwordUpdateResult);
                    return actionResult;
                }
            }

            return CreatedAtAction("GetApplicationUser", new { id = applicationUser.Id }, applicationUser);
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostApplicationUser(ApplicationUser applicationUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool modelIsInvalid = await ValidateModel(applicationUser);
            if (modelIsInvalid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                UserName = applicationUser.UserName,
                Email = applicationUser.Email,
                FirstName = applicationUser.FirstName,
                LastName = applicationUser.LastName,
                PhoneNumber = applicationUser.PhoneNumber,
                IsActive = true,
            };

            IdentityResult result = await userManager.CreateAsync(user, applicationUser.Password);

            if (!result.Succeeded)
            {
                IActionResult actionResult = GetErrorResult(result);
                return actionResult;
            }

            return CreatedAtAction("GetApplicationUser", new { id = applicationUser.Id }, applicationUser);
        }

        private async Task<bool> ValidateModel(ApplicationUser applicationUser)
        {
            bool isInvalid = false;
            if (await this.userManager.Users.AnyAsync(x => x.PhoneNumber == applicationUser.PhoneNumber))
            {
                isInvalid = true;
                ModelState.AddModelError("phoneNumber", "Duplicate Phone number");
            }

            if (await this.userManager.Users.AnyAsync(x => x.UserName == applicationUser.UserName))
            {
                isInvalid = true;
                ModelState.AddModelError("userName", "Duplicate User name");
            }

            if (await this.userManager.Users.AnyAsync(x => x.Email == applicationUser.Email))
            {
                isInvalid = true;
                ModelState.AddModelError("email", "Duplicate Email");
            }

            return isInvalid;
        }

        private async Task<bool> ValidateUpdateModel(ApplicationUser applicationUser)
        {
            bool isInvalid = false;
            if (await this.userManager.Users.AnyAsync(x => x.Id != applicationUser.Id && x.PhoneNumber == applicationUser.PhoneNumber))
            {
                isInvalid = true;
                ModelState.AddModelError("phoneNumber", "Duplicate Phone number");
            }

            if (await this.userManager.Users.AnyAsync(x => x.Id != applicationUser.Id && x.UserName == applicationUser.UserName))
            {
                isInvalid = true;
                ModelState.AddModelError("userName", "Duplicate User name");
            }

            if (await this.userManager.Users.AnyAsync(x => x.Id != applicationUser.Id && x.Email == applicationUser.Email))
            {
                isInvalid = true;
                ModelState.AddModelError("email", "Duplicate Email");
            }

            if (!(string.IsNullOrEmpty(applicationUser.Password) && string.IsNullOrEmpty(applicationUser.NewPassword)))
            {
                isInvalid = true;
                ModelState.AddModelError("password","Current password and New password both must be provided to change the password");
            }

            return isInvalid;
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplicationUser(string id)
        {
            var applicationUser = await _context.Users.FindAsync(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            _context.Users.Remove(applicationUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.Phone,
                IsActive = false,
            };

            bool modelIsInvalid = await ValidateModel(user);
            if (modelIsInvalid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        private IActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return this.BadRequest();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
