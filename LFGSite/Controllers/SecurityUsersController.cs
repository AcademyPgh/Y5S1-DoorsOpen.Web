using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LFGSite.Data;
using LFGSite.Models;
using Microsoft.AspNetCore.Authorization;

namespace LFGSite.Controllers
{
    [Authorize(Roles = "Administrator")]
    [Route("/admin/users/{action=Index}/{id?}")]
    public class SecurityUsersController : Controller
    {
        private readonly SiteDbContext _context;

        public SecurityUsersController(SiteDbContext context)
        {
            _context = context;
        }

        // GET: SecurityUsers
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: SecurityUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var securityUser = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (securityUser == null)
            {
                return NotFound();
            }

            return View(securityUser);
        }

        // GET: SecurityUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SecurityUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email")] SecurityUser securityUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(securityUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(securityUser);
        }

        // GET: SecurityUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var securityUser = await _context.User.FindAsync(id);
            if (securityUser == null)
            {
                return NotFound();
            }
            return View(securityUser);
        }

        // POST: SecurityUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email")] SecurityUser securityUser)
        {
            if (id != securityUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(securityUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SecurityUserExists(securityUser.Id))
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
            return View(securityUser);
        }

        // GET: SecurityUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var securityUser = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (securityUser == null)
            {
                return NotFound();
            }

            return View(securityUser);
        }

        // POST: SecurityUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var securityUser = await _context.User.FindAsync(id);
            _context.User.Remove(securityUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SecurityUserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
