using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using ProjektIO.Data;
using ProjektIO.Entities;
using System.Security.Claims;

namespace ProjektIO.Areas.Identity.Pages.Account.Manage
{
    public class SalaryModel : PageModel
    {
        private readonly ApplicationDbContext _context;


        public SalaryModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var result = await _context.Salaries.FirstOrDefaultAsync(x => x.UserId == UserId);
            if (result != null) { ViewData["Salary"] = result.Salary.ToString("0.00"); }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(decimal salary)
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var currentUserSalary = await _context.Salaries.FirstOrDefaultAsync(x => x.UserId == UserId);

            if(currentUserSalary is null)
            {
                _context.Salaries.Add(new Salaries { Salary = salary, UserId = UserId });                
            } else
            {
                currentUserSalary.Salary = salary;
                _context.Update(currentUserSalary);
            }
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}
