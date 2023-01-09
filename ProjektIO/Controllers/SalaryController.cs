using Microsoft.AspNetCore.Mvc;
using ProjektIO.Data;

namespace ProjektIO.Controllers
{
    public class SalaryController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SalaryController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
