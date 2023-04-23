using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TMRDataManager.Library.Models;
using TMRDataManager.Library.DataAccess;
using System.Linq;
using TRM_Api.Data;
using TRM_Api.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TRM_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserData _userData;
        private readonly ILogger<UserController> _logger;

        public UserController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IUserData userData, ILogger<UserController> logger)
        {
            _context = context;
            _userManager = userManager;
            _userData = userData;
            _logger = logger;
        }

        [HttpGet]
        public UserModel GetById()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            return _userData.GetUserById(userId).First();
        }

        public record UserRegistrationModel(
            string FirstName,
            string LastName,
            string EmailAddress,
            string Password);

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserRegistrationModel user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(user.EmailAddress);
                if (existingUser is null)
                {
                    IdentityUser newUser = new()
                    {
                        Email = user.EmailAddress,
                        EmailConfirmed = true,
                        UserName = user.EmailAddress
                    };

                    IdentityResult result = await _userManager.CreateAsync(newUser, user.Password);

                    if (result.Succeeded)
                    {
                        existingUser = await _userManager.FindByEmailAsync(user.EmailAddress);
                        if (existingUser is null)
                        {
                            return BadRequest();
                        }


                        UserModel u = new()
                        {
                            Id = existingUser.Id,
                            FirstName = user.FirstName, 
                            LastName = user.LastName,
                            EmailAddress = user.EmailAddress
                        };
                        _userData.CreateUser(u);
                        return Ok();
                    }
                }
            }

            return BadRequest();
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("Admin/GetAllUsers")]
        public List<ApplicationUserModel> GetAllUsers()
        {
            List<ApplicationUserModel> output = new List<ApplicationUserModel>();

            var users = _context.Users.ToList();

            var userRoles = from ur in _context.UserRoles
                            join r in _context.Roles on ur.RoleId equals r.Id
                            select new { ur.UserId, ur.RoleId, r.Name };

            foreach (var user in users)
            {
                ApplicationUserModel u = new ApplicationUserModel
                {
                    Id = user.Id,
                    Email = user.Email
                };

                u.Roles = userRoles.Where(x => x.UserId == u.Id).ToDictionary(key => key.RoleId, val => val.Name);

                output.Add(u);
            }

            return output;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("Admin/GetAllRoles")]
        public Dictionary<string, string> GetAllRoles()
        {

            var roles = _context.Roles.ToDictionary(x => x.Id, x => x.Name);

            return roles;

        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Admin/AddRole")]
        public async Task AddRole(UserRolePairModel pairing)
        {
            string loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(pairing.UserId);

            _logger.LogInformation("Admin {Admin} added user {user} to role {Role}", loggedInUserId, user.Id, pairing.RoleName);

            await _userManager.AddToRoleAsync(user, pairing.RoleName);
        }


        // post man kan bara stoppa in en sak tror jag
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Admin/RemoveRole")]
        public async Task RemoveRole(UserRolePairModel pairing)
        {
            string loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _userManager.FindByIdAsync(pairing.UserId);

            _logger.LogInformation("Admin {Admin} remove user {User} from role {Role}", loggedInUserId, user.Id, pairing.RoleName);

            await _userManager.RemoveFromRoleAsync(user, pairing.RoleName);
        }
    }
}
