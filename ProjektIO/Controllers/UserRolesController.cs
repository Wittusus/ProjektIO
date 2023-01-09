using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjektIO.Data;
using ProjektIO.Entities;

namespace ProjektIO.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserRolesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserRolesController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager= userManager;
            _context = context;
        }

        // GET: UserRoles
        public async Task<IActionResult> Index()
        {
            var rol = await _context.Roles.FirstOrDefaultAsync();
            var ghi = await _roleManager.GetRoleNameAsync(rol);
            return View(await _context.UserRolesIndex.ToListAsync());
        }

        // GET: UserRoles/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            return View();
        }

        // POST: UserRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,RoleId")] IdentityUserRole<string> userRoles)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(userRoles.UserId);
                var role = await _roleManager.FindByIdAsync(userRoles.RoleId);
                await _userManager.AddToRoleAsync(user, role.Name);
                return RedirectToAction(nameof(Index));
            }
            return View(userRoles);
        }

        // GET: UserRoles/Delete/5
        public async Task<IActionResult> Delete(string id, string roleid)
        {
            if (id == null || _context.UserRoles == null)
            {
                return NotFound();
            }

            var userRoles = await _context.UserRoles
                .FirstOrDefaultAsync(m => m.UserId == id && m.RoleId == roleid);
            if (userRoles == null)
            {
                return NotFound();
            }

            return View(userRoles);
        }

        // POST: UserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id, string roleid)
        {
            if (_context.UserRoles == null)
            {
                return Problem("Entity set 'ApplicationDbContext.UserRoles'  is null.");
            }
            var userRoles = await _context.UserRoles.FirstOrDefaultAsync(m => m.UserId == id && m.RoleId == roleid);
            if (userRoles != null)
            {
                _context.UserRoles.Remove(userRoles);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserRolesExists(string id)
        {
          return _context.UserRoles.Any(e => e.UserId == id);
        }
    }
}
