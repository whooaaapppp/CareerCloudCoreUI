using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CareerCloud.EntityFrameworkDataAccess;
using CareerCloud.Pocos;

namespace CareerCloudCoreUI.Controllers
{
    public class CompanyLocationController : Controller
    {
        private readonly CareerCloudContext _context;

        public CompanyLocationController(CareerCloudContext context)
        {
            _context = context;
        }

        // GET: CompanyLocation
        public async Task<IActionResult> Index()
        {
            var careerCloudContext = _context.CompanyLocations.Include(c => c.CompanyProfiles);
            return View(await careerCloudContext.ToListAsync());
        }

        // GET: CompanyLocation/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyLocationPoco = await _context.CompanyLocations
                .Include(c => c.CompanyProfiles)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (companyLocationPoco == null)
            {
                return NotFound();
            }

            return View(companyLocationPoco);
        }

        // GET: CompanyLocation/Create
        public IActionResult Create()
        {
            ViewData["Company"] = new SelectList(_context.CompanyProfiles, "Id", "Id");
            return View();
        }

        // POST: CompanyLocation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Company,CountryCode,Province,Street,City,PostalCode,TimeStamp")] CompanyLocationPoco companyLocationPoco)
        {
            if (ModelState.IsValid)
            {
                companyLocationPoco.Id = Guid.NewGuid();
                _context.Add(companyLocationPoco);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Company"] = new SelectList(_context.CompanyProfiles, "Id", "Id", companyLocationPoco.Company);
            return View(companyLocationPoco);
        }

        // GET: CompanyLocation/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyLocationPoco = await _context.CompanyLocations.FindAsync(id);
            if (companyLocationPoco == null)
            {
                return NotFound();
            }
            ViewData["Company"] = new SelectList(_context.CompanyProfiles, "Id", "Id", companyLocationPoco.Company);
            return View(companyLocationPoco);
        }

        // POST: CompanyLocation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Company,CountryCode,Province,Street,City,PostalCode,TimeStamp")] CompanyLocationPoco companyLocationPoco)
        {
            if (id != companyLocationPoco.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(companyLocationPoco);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyLocationPocoExists(companyLocationPoco.Id))
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
            ViewData["Company"] = new SelectList(_context.CompanyProfiles, "Id", "Id", companyLocationPoco.Company);
            return View(companyLocationPoco);
        }

        // GET: CompanyLocation/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyLocationPoco = await _context.CompanyLocations
                .Include(c => c.CompanyProfiles)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (companyLocationPoco == null)
            {
                return NotFound();
            }

            return View(companyLocationPoco);
        }

        // POST: CompanyLocation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var companyLocationPoco = await _context.CompanyLocations.FindAsync(id);
            _context.CompanyLocations.Remove(companyLocationPoco);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyLocationPocoExists(Guid id)
        {
            return _context.CompanyLocations.Any(e => e.Id == id);
        }
    }
}
