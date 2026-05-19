using Ahmed_Task.Enums;
using Ahmed_Task.Models;
using Ahmed_Task.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ahmed_Task.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //N+1 problem 
        //solve using joins
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();

            var result = new List<UserVM>();

            foreach (var user in users)
            {
                result.Add(new UserVM
                {
                    Id = user.Id,
                    UserName = user.UserName!,
                    Email = user.Email!,
                    PhoneNumber = user.PhoneNumber!,
                    IsLockedOut = user.LockoutEnd > DateTimeOffset.UtcNow,
                    Roles = await _userManager.GetRolesAsync(user)
                });
            }

            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);

            var result = new UserFormVM
            {
                Id = user.Id,
                SelectedRoles = userRoles.ToList(),
            };

            result.Roles = await _roleManager.Roles.Select(r => new SelectListItem
            {
                Value = r.Name!,
                Text = r.Name!
            }).ToListAsync();

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //handle the case when model state is invalid, return the view with selected list of roles.
        //handle if user added a role that doesn't exist in the system, return the view with error message.
        public async Task<IActionResult> Edit(UserFormVM model)
        {
            if (!ModelState.IsValid) {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
                return NotFound();
           
            var userRoles = await _userManager.GetRolesAsync(user);

            var rolesToAdd = model.SelectedRoles.Except(userRoles);
            var rolesToRemove = userRoles.Except(model.SelectedRoles);

            await _userManager.AddToRolesAsync(user, rolesToAdd);
            await _userManager.RemoveFromRolesAsync(user, rolesToRemove);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleLock(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "User not found"
                });
            }

            bool isLocked;

            if (user.LockoutEnd > DateTimeOffset.UtcNow)
            {
                user.LockoutEnd = DateTimeOffset.UtcNow;
                isLocked = false;
            }
            else
            {
                user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);
                isLocked = true;
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Failed to update user status"
                });
            }

            await _userManager.UpdateSecurityStampAsync(user);

            return Json(new
            {
                success = true,
                message = isLocked
                    ? "User locked successfully"
                    : "User unlocked successfully"
            });
        }
    }
}
