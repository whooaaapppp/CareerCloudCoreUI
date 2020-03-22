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
    public class CompanyJobDescriptionController : Controller
    {
        private readonly CareerCloudContext _context;

        public CompanyJobDescriptionController(CareerCloudContext context)
        {
            _context = context;
        }

        // GET: CompanyJobDescription
        public async Task<IActionResult> Index()
        {
            var careerCloudContext = _context.CompanyJobsDescriptions.Include(c => c.CompanyJobs);
            return View(await careerCloudContext.ToListAsync());
        }

        // GET: CompanyJobDescription/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyJobDescriptionPoco = await _context.CompanyJobsDescriptions
                .Include(c => c.CompanyJobs)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (companyJobDescriptionPoco == null)
            {
                return NotFound();
            }

            return View(companyJobDescriptionPoco);
        }

        // GET: CompanyJobDescription/Create
        public IActionResult Create()
        {
            ViewData["Job"] = new SelectList(_context.CompanyJobs, "Id", "Id");
            return View();
        }

        // POST: CompanyJobDescription/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Job,JobName,JobDescriptions,TimeStamp")] CompanyJobDescriptionPoco companyJobDescriptionPoco)
        {
            if (ModelState.IsValid)
            {
                companyJobDescriptionPoco.Id = Guid.NewGuid();
                _context.Add(companyJobDescriptionPoco);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Job"] = new SelectList(_context.CompanyJobs, "Id", "Id", companyJobDescriptionPoco.Job);
            return View(companyJobDescriptionPoco);
        }

        // GET: CompanyJobDescription/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyJobDescriptionPoco = await _context.CompanyJobsDescriptions.FindAsync(id);
            if (companyJobDescriptionPoco == null)
            {
                return NotFound();
            }
            ViewData["Job"] = new SelectList(_context.CompanyJobs, "Id", "Id", companyJobDescriptionPoco.Job);
            return View(companyJobDescriptionPoco);
        }

        // POST: CompanyJobDescription/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Job,JobName,JobDescriptions,TimeStamp")] CompanyJobDescriptionPoco companyJobDescriptionPoco)
        {
            if (id != companyJobDescriptionPoco.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(companyJobDescriptionPoco);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyJobDescriptionPocoExists(companyJobDescriptionPoco.Id))
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
            ViewData["Job"] = new SelectList(_context.CompanyJobs, "Id", "Id", companyJobDescriptionPoco.Job);
            return View(companyJobDescriptionPoco);
        }

        // GET: CompanyJobDescription/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var companyJobDescriptionPoco = await _context.CompanyJobsDescriptions
                .Include(c => c.CompanyJobs)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (companyJobDescriptionPoco == null)
            {
                return NotFound();
            }

            return View(companyJobDescriptionPoco);
        }

        // POST: CompanyJobDescription/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var companyJobDescriptionPoco = await _context.CompanyJobsDescriptions.FindAsync(id);
            _context.CompanyJobsDescriptions.Remove(companyJobDescriptionPoco);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyJobDescriptionPocoExists(Guid id)
        {
            return _context.CompanyJobsDescriptions.Any(e => e.Id == id);
        }
    }
}
