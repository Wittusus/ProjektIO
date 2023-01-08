using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjektIO.Data;
using ProjektIO.Entities;

namespace ProjektIO.Controllers
{
    [Authorize]
    public class HedgefundHistoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HedgefundHistoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: HedgefundHistories
        public async Task<IActionResult> Index()
        {
              return View(await _context.HedgefundsHistory.ToListAsync());
        }

        public async Task<IActionResult> ShowHistory(int id)
        {
            return View(await _context.HedgefundsHistory.Where(x => x.HedgefundId == id).ToListAsync());
        }

        // GET: HedgefundHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.HedgefundsHistory == null)
            {
                return NotFound();
            }

            var hedgefundHistory = await _context.HedgefundsHistory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hedgefundHistory == null)
            {
                return NotFound();
            }

            return View(hedgefundHistory);
        }

        // GET: HedgefundHistories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: HedgefundHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ReturnRate,ChangeDate")] HedgefundHistory hedgefundHistory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hedgefundHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(hedgefundHistory);
        }

        // GET: HedgefundHistories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.HedgefundsHistory == null)
            {
                return NotFound();
            }

            var hedgefundHistory = await _context.HedgefundsHistory.FindAsync(id);
            if (hedgefundHistory == null)
            {
                return NotFound();
            }
            return View(hedgefundHistory);
        }

        // POST: HedgefundHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ReturnRate,ChangeDate")] HedgefundHistory hedgefundHistory)
        {
            if (id != hedgefundHistory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hedgefundHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HedgefundHistoryExists(hedgefundHistory.Id))
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
            return View(hedgefundHistory);
        }

        // GET: HedgefundHistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.HedgefundsHistory == null)
            {
                return NotFound();
            }

            var hedgefundHistory = await _context.HedgefundsHistory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hedgefundHistory == null)
            {
                return NotFound();
            }

            return View(hedgefundHistory);
        }

        // POST: HedgefundHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.HedgefundsHistory == null)
            {
                return Problem("Entity set 'ApplicationDbContext.HedgefundsHistory'  is null.");
            }
            var hedgefundHistory = await _context.HedgefundsHistory.FindAsync(id);
            if (hedgefundHistory != null)
            {
                _context.HedgefundsHistory.Remove(hedgefundHistory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HedgefundHistoryExists(int id)
        {
          return _context.HedgefundsHistory.Any(e => e.Id == id);
        }
    }
}
