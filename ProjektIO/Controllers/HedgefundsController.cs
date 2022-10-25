using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjektIO.Data;
using ProjektIO.Entities;

namespace ProjektIO.Controllers
{
    public class HedgefundsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HedgefundsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Hedgefunds
        public async Task<IActionResult> Index()
        {
              return View(await _context.Hedgefunds.OrderByDescending(x => x.RequiredSalary).ToListAsync());
        }

        // GET: Hedgefunds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Hedgefunds == null)
            {
                return NotFound();
            }

            var hedgefund = await _context.Hedgefunds
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hedgefund == null)
            {
                return NotFound();
            }

            return View(hedgefund);
        }

        // GET: Hedgefunds/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hedgefunds/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,RequiredSalary,CreationDate")] Hedgefund hedgefund)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hedgefund);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hedgefund);
        }

        // GET: Hedgefunds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Hedgefunds == null)
            {
                return NotFound();
            }

            var hedgefund = await _context.Hedgefunds.FindAsync(id);
            if (hedgefund == null)
            {
                return NotFound();
            }
            return View(hedgefund);
        }

        // POST: Hedgefunds/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,RequiredSalary,CreationDate")] Hedgefund hedgefund)
        {
            if (id != hedgefund.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hedgefund);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HedgefundExists(hedgefund.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(hedgefund);
        }

        // GET: Hedgefunds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Hedgefunds == null)
            {
                return NotFound();
            }

            var hedgefund = await _context.Hedgefunds
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hedgefund == null)
            {
                return NotFound();
            }

            return View(hedgefund);
        }

        // POST: Hedgefunds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Hedgefunds == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Hedgefunds'  is null.");
            }
            var hedgefund = await _context.Hedgefunds.FindAsync(id);
            if (hedgefund != null)
            {
                _context.Hedgefunds.Remove(hedgefund);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HedgefundExists(int id)
        {
          return _context.Hedgefunds.Any(e => e.Id == id);
        }
    }
}
