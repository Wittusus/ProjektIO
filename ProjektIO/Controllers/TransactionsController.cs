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
    public class TransactionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TransactionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Transactions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Transactions.Include(t => t.CreationUser).Include(t => t.Hedgefund).Include(t => t.TargetUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Transactions/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transactions = await _context.Transactions
                .Include(t => t.CreationUser)
                .Include(t => t.Hedgefund)
                .Include(t => t.TargetUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transactions == null)
            {
                return NotFound();
            }

            return View(transactions);
        }

        // GET: Transactions/Create
        public IActionResult Create()
        {
            ViewData["CreationUserId"] = new SelectList(_context.Users, "Id", "UserName");
            ViewData["HedgefundId"] = new SelectList(_context.Hedgefunds, "Id", "Name");
            ViewData["TargetUserId"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CreationTime,CreationUserId,TargetUserId,TransactionType,AmountOfTransaction,HedgefundId,ReturnTimeOfInvestment")] Transactions transactions)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transactions);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreationUserId"] = new SelectList(_context.Users, "Id", "Id", transactions.CreationUserId);
            ViewData["HedgefundId"] = new SelectList(_context.Hedgefunds, "Id", "Id", transactions.HedgefundId);
            ViewData["TargetUserId"] = new SelectList(_context.Users, "Id", "Id", transactions.TargetUserId);
            return View(transactions);
        }

        // GET: Transactions/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transactions = await _context.Transactions.FindAsync(id);
            if (transactions == null)
            {
                return NotFound();
            }
            ViewData["CreationUserId"] = new SelectList(_context.Users, "Id", "Id", transactions.CreationUserId);
            ViewData["HedgefundId"] = new SelectList(_context.Hedgefunds, "Id", "Id", transactions.HedgefundId);
            ViewData["TargetUserId"] = new SelectList(_context.Users, "Id", "Id", transactions.TargetUserId);
            return View(transactions);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,CreationTime,CreationUserId,TargetUserId,TransactionType,AmountOfTransaction,HedgefundId,ReturnTimeOfInvestment")] Transactions transactions)
        {
            if (id != transactions.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transactions);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionsExists(transactions.Id))
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
            ViewData["CreationUserId"] = new SelectList(_context.Users, "Id", "Id", transactions.CreationUserId);
            ViewData["HedgefundId"] = new SelectList(_context.Hedgefunds, "Id", "Id", transactions.HedgefundId);
            ViewData["TargetUserId"] = new SelectList(_context.Users, "Id", "Id", transactions.TargetUserId);
            return View(transactions);
        }

        // GET: Transactions/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transactions = await _context.Transactions
                .Include(t => t.CreationUser)
                .Include(t => t.Hedgefund)
                .Include(t => t.TargetUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transactions == null)
            {
                return NotFound();
            }

            return View(transactions);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Transactions == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Transactions'  is null.");
            }
            var transactions = await _context.Transactions.FindAsync(id);
            if (transactions != null)
            {
                _context.Transactions.Remove(transactions);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionsExists(long id)
        {
          return _context.Transactions.Any(e => e.Id == id);
        }
    }
}
