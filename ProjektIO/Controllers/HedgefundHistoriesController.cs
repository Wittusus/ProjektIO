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
using ProjektIO.Extensions;
using ProjektIO.Models;

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
            var histories = await _context.HedgefundsHistory.Where(x => x.HedgefundId == id).ToListAsync();
            List<LineChartData> chartData = GetChartData(histories);
            chartData = chartData.OrderByDescending(x => x.xValue).ToList();
            var viewModel = new HedgeFundHistoriesGetViewModel() { HedgefundHistories = histories, ChartDatas = chartData };
            return View(viewModel);
        }

        private static List<LineChartData> GetChartData(List<HedgefundHistory> histories)
        {
            var chartData = new List<LineChartData>
            {
                /*
                new LineChartData { xValue = new DateTime(2005, 01, 01), yValue = 21, yValue1 = 28 },
                new LineChartData { xValue = new DateTime(2006, 01, 01), yValue = 24, yValue1 = 44 },
                new LineChartData { xValue = new DateTime(2007, 01, 01), yValue = 36, yValue1 = 48 },
                new LineChartData { xValue = new DateTime(2008, 01, 01), yValue = 38, yValue1 = 50 },
                new LineChartData { xValue = new DateTime(2009, 01, 01), yValue = 54, yValue1 = 66 },
                new LineChartData { xValue = new DateTime(2010, 01, 01), yValue = 57, yValue1 = 78 },
                new LineChartData { xValue = new DateTime(2011, 01, 01), yValue = 70, yValue1 = 84 },
                */
            };
            foreach (var item in histories)
            {
                var outputItem = new LineChartData { xValue = item.ChangeDate, yValue = item.ReturnRate };
                chartData.Add(outputItem);
            }
            var returnRates = histories.Select(x => x.ReturnRate).ToList();
            var lastRate = returnRates.LastOrDefault();
            var dev = returnRates.StdDev();
            for (int i = 1; i <= 5; i++)
            {
                lastRate = lastRate + dev;
                var outputItem = new LineChartData { xValue = DateTime.Now.AddDays(i), yValue = lastRate };
                chartData.Add(outputItem);
            }

            return chartData;
        }

        //GET: Compare two charts
        public async Task<IActionResult> CompareCharts()
        {
            var hedgefunds = await _context.Hedgefunds.ToListAsync();
           
            return View(hedgefunds);
        }

        public async Task<IActionResult> CompareChartsDetails(long hedgefundOneId, long hedgefundTwoId)
        {
            var hedge1 = await _context.Hedgefunds.Where(x => x.Id == hedgefundOneId).FirstOrDefaultAsync();
            var hedge2 = await _context.Hedgefunds.Where(x => x.Id == hedgefundTwoId).FirstOrDefaultAsync();
            var historiesOne = await _context.HedgefundsHistory.Where(x => x.HedgefundId == hedgefundOneId).ToListAsync();
            var historiesTwo = await _context.HedgefundsHistory.Where(x => x.HedgefundId == hedgefundTwoId).ToListAsync();
            List<LineChartData> chartDataForOne = GetChartData(historiesOne).OrderByDescending(x => x.xValue).ToList();
            List<LineChartData> chartDataForTwo = GetChartData(historiesTwo).OrderByDescending(x => x.xValue).ToList();

            var model = new ChartDetailsViewModel()
            {
                HedgefundOneChartData= chartDataForOne,
                HedgefundTwoChartData= chartDataForTwo,
                HedgefundOneName = hedge1.Name,
                HedgefundTwoName = hedge2.Name
            };
            return View(model);
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
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: HedgefundHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
