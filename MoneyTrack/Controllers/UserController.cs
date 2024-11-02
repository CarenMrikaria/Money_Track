using Microsoft.AspNetCore.Mvc;
using MoneyTrack.Models;
using MoneyTrack.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using MoneyTrack.Areas.Identity.Data;

namespace MoneyTrack.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly MoneyTrackContext _context;
        private readonly UserManager<MoneyTrackUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(MoneyTrackContext context, UserManager<MoneyTrackUser> userManager, RoleManager<IdentityRole> roleManager) {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.Skip(1).ToListAsync();

            return View(users);
        }


        [Authorize(Roles = "Admin")]
        public IActionResult AddData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddData(User user) 
        { 
            if (ModelState.IsValid) 
            { 
                _context.UserTable.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            } 
            return View(user); 
        }
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditData(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Fetch the roles for this user
            var userRoles = await _userManager.GetRolesAsync(user);

            // Fetch all roles available in the system
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();

            // Pass user roles and all roles to the ViewBag
            ViewBag.Roles = allRoles;
            ViewBag.UserRoles = userRoles;

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateData(string id, string email, string role)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Update the user's email
                user.Email = email;
                await _userManager.UpdateAsync(user);

                // Update the user's role
                var userRoles = await _userManager.GetRolesAsync(user);

                // Remove all existing roles
                if (userRoles.Any())
                {
                    await _userManager.RemoveFromRolesAsync(user, userRoles);
                }

                // Assign the new role
                if (!string.IsNullOrEmpty(role))
                {
                    await _userManager.AddToRoleAsync(user, role);
                }

                return RedirectToAction("Index");
            }

            return View(user);
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteData(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            return BadRequest("Unable to delete user.");
        }

    }
}
