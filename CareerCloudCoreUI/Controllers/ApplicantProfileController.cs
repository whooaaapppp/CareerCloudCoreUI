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
    public class ApplicantProfileController : Controller
    {
        private readonly CareerCloudContext _context;

        public ApplicantProfileController(CareerCloudContext context)
        {
            _context = context;
        }

        // GET: ApplicantProfile
        public async Task<IActionResult> Index()
        {
            var careerCloudContext = _context.ApplicantProfiles.Include(a => a.SecurityLogins).Include(a => a.SystemCountryCodes);
            return View(await careerCloudContext.ToListAsync());
        }

        // GET: ApplicantProfile/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicantProfilePoco = await _context.ApplicantProfiles
                .Include(a => a.SecurityLogins)
                .Include(a => a.SystemCountryCodes)
                .Include(a => a.ApplicantEducations)
                .Include(a => a.ApplicantResumes)
                .Include(a => a.ApplicantJobApplications)
                .Include(a => a.ApplicantWorkHistory)
                .Include(a => a.ApplicantSkills)


                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicantProfilePoco == null)
            {
                return NotFound();
            }

            return View(applicantProfilePoco);
        }

        // GET: ApplicantProfile/Create
        public IActionResult Create()
        {
            ViewData["Login"] = new SelectList(_context.SecurityLogins, "Id", "Id");
            ViewData["Country"] = new SelectList(_context.SystemCountryCodes, "Code", "Code");
            return View();
        }

        // POST: ApplicantProfile/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Login,Currency,CurrentSalary,CurrentRate,Country,Province,Street,City,PostalCode,TimeStamp")] ApplicantProfilePoco applicantProfilePoco)
        {
            if (ModelState.IsValid)
            {
                applicantProfilePoco.Id = Guid.NewGuid();
                _context.Add(applicantProfilePoco);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Login"] = new SelectList(_context.SecurityLogins, "Id", "Id", applicantProfilePoco.Login);
            ViewData["Country"] = new SelectList(_context.SystemCountryCodes, "Code", "Code", applicantProfilePoco.Country);
            return View(applicantProfilePoco);
        }

        // GET: ApplicantProfile/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicantProfilePoco = await _context.ApplicantProfiles.FindAsync(id);
            if (applicantProfilePoco == null)
            {
                return NotFound();
            }
            ViewData["Login"] = new SelectList(_context.SecurityLogins, "Id", "Id", applicantProfilePoco.Login);
            ViewData["Country"] = new SelectList(_context.SystemCountryCodes, "Code", "Code", applicantProfilePoco.Country);
            return View(applicantProfilePoco);
        }

        // POST: ApplicantProfile/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Login,Currency,CurrentSalary,CurrentRate,Country,Province,Street,City,PostalCode,TimeStamp")] ApplicantProfilePoco applicantProfilePoco)
        {
            if (id != applicantProfilePoco.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicantProfilePoco);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicantProfilePocoExists(applicantProfilePoco.Id))
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
            ViewData["Login"] = new SelectList(_context.SecurityLogins, "Id", "Id", applicantProfilePoco.Login);
            ViewData["Country"] = new SelectList(_context.SystemCountryCodes, "Code", "Code", applicantProfilePoco.Country);
            return View(applicantProfilePoco);
        }

        // GET: ApplicantProfile/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicantProfilePoco = await _context.ApplicantProfiles
                .Include(a => a.SecurityLogins)
                .Include(a => a.SystemCountryCodes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (applicantProfilePoco == null)
            {
                return NotFound();
            }

            return View(applicantProfilePoco);
        }

        // POST: ApplicantProfile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
        {
            var applicantProfilePoco = await _context.ApplicantProfiles.FindAsync(id);
            _context.ApplicantProfiles.Remove(applicantProfilePoco);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicantProfilePocoExists(Guid? id)
        {
            return _context.ApplicantProfiles.Any(e => e.Id == id);
        }
    }
}
