using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var balance = await _context.Transactions.Where(x => x.TargetUserId == userId)
            .Select(x => x.AmountOfTransaction)
            .SumAsync();
            ViewData["Balance"] = balance.ToString("0.00");

            if(User.IsInRole("Admin"))
            {
                var applicationDbContext2 = _context.Transactions.Include(t => t.CreationUser).Include(t => t.Hedgefund).Include(t => t.TargetUser)
                .OrderByDescending(x => x.CreationTime);
                return View(await applicationDbContext2.ToListAsync());
            }

            var applicationDbContext = _context.Transactions.Include(t => t.CreationUser).Include(t => t.Hedgefund).Include(t => t.TargetUser)
                .Where(x => x.CreationUserId == userId || x.TargetUserId == userId).OrderByDescending(x => x.CreationTime);
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
        public async Task<IActionResult> Create()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var balance = await _context.Transactions.Where(x => x.TargetUserId == userId)
                .Select(x => x.AmountOfTransaction)
                .SumAsync();
            ViewData["HedgefundId"] = new SelectList(_context.Hedgefunds, "Id", "Name");
            ViewData["TargetUserId"] = new SelectList(_context.Users, "Id", "UserName");
            ViewData["Balance"] = balance.ToString("0.00");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TargetUserId,TransactionType,AmountOfTransaction,HedgefundId")] Transactions transactions)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var balance = await _context.Transactions.Where(x => x.TargetUserId == userId)
                .Select(x => x.AmountOfTransaction)
                .SumAsync();


            if (!User.IsInRole("Admin"))
                transactions.TransactionType = TypeOfTransaction.Invest;

            if (transactions.TransactionType == TypeOfTransaction.Income)
            {
                transactions.CreationUserId = userId;
                transactions.TargetUserId = userId;
                transactions.CreationTime = DateTime.Now.AddSeconds(15);
                _context.Add(transactions);
                await _context.SaveChangesAsync();
            } else {

                if (balance <= transactions.AmountOfTransaction && transactions.TargetUserId == userId)
                    throw new Exception("Not enough money");
                var trans = new List<Transactions>();

            
                trans.Add(new Transactions
                {
                    CreationUserId = userId,
                    CreationTime = DateTime.Now,
                    AmountOfTransaction = transactions.AmountOfTransaction * -1,
                    TargetUserId = userId,
                    TransactionType = TypeOfTransaction.BlockCash
                });
            
                
                transactions.CreationUserId = userId;
                transactions.CreationTime = DateTime.Now.AddSeconds(15);
                trans.Add(transactions);
                _context.AddRange(trans);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
            
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
