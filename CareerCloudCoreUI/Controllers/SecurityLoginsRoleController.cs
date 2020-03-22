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
    public class SecurityLoginsRoleController : Controller
    {
        private readonly CareerCloudContext _context;

        public SecurityLoginsRoleController(CareerCloudContext context)
        {
            _context = context;
        }

        // GET: SecurityLoginsRole
        public async Task<IActionResult> Index()
        {
            var careerCloudContext = _context.SecurityLoginsRoles.Include(s => s.SecurityLogins).Include(s => s.SecurityRoles);
            return View(await careerCloudContext.ToListAsync());
        }

        // GET: SecurityLoginsRole/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var securityLoginsRolePoco = await _context.SecurityLoginsRoles
                .Include(s => s.SecurityLogins)
                .Include(s => s.SecurityRoles)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (securityLoginsRolePoco == null)
            {
                return NotFound();
            }

            return View(securityLoginsRolePoco);
        }

        // GET: SecurityLoginsRole/Create
        public IActionResult Create()
        {
            ViewData["Login"] = new SelectList(_context.SecurityLogins, "Id", "Id");
            ViewData["Id"] = new SelectList(_context.SecurityRoles, "Id", "Id");
            return View();
        }

        // POST: SecurityLoginsRole/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Login,Role,TimeStamp")] SecurityLoginsRolePoco securityLoginsRolePoco)
        {
            if (ModelState.IsValid)
            {
                securityLoginsRolePoco.Id = Guid.NewGuid();
                _context.Add(securityLoginsRolePoco);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Login"] = new SelectList(_context.SecurityLogins, "Id", "Id", securityLoginsRolePoco.Login);
            ViewData["Id"] = new SelectList(_context.SecurityRoles, "Id", "Id", securityLoginsRolePoco.Id);
            return View(securityLoginsRolePoco);
        }

        // GET: SecurityLoginsRole/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var securityLoginsRolePoco = await _context.SecurityLoginsRoles.FindAsync(id);
            if (securityLoginsRolePoco == null)
            {
                return NotFound();
            }
            ViewData["Login"] = new SelectList(_context.SecurityLogins, "Id", "Id", securityLoginsRolePoco.Login);
            ViewData["Id"] = new SelectList(_context.SecurityRoles, "Id", "Id", securityLoginsRolePoco.Id);
            return View(securityLoginsRolePoco);
        }

        // POST: SecurityLoginsRole/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Login,Role,TimeStamp")] SecurityLoginsRolePoco securityLoginsRolePoco)
        {
            if (id != securityLoginsRolePoco.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(securityLoginsRolePoco);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SecurityLoginsRolePocoExists(securityLoginsRolePoco.Id))
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
            ViewData["Login"] = new SelectList(_context.SecurityLogins, "Id", "Id", securityLoginsRolePoco.Login);
            ViewData["Id"] = new SelectList(_context.SecurityRoles, "Id", "Id", securityLoginsRolePoco.Id);
            return View(securityLoginsRolePoco);
        }

        // GET: SecurityLoginsRole/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var securityLoginsRolePoco = await _context.SecurityLoginsRoles
                .Include(s => s.SecurityLogins)
                .Include(s => s.SecurityRoles)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (securityLoginsRolePoco == null)
            {
                return NotFound();
            }

            return View(securityLoginsRolePoco);
        }

        // POST: SecurityLoginsRole/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var securityLoginsRolePoco = await _context.SecurityLoginsRoles.FindAsync(id);
            _context.SecurityLoginsRoles.Remove(securityLoginsRolePoco);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SecurityLoginsRolePocoExists(Guid id)
        {
            return _context.SecurityLoginsRoles.Any(e => e.Id == id);
        }
    }
}
