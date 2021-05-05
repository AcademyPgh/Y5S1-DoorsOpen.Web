using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoorsOpen.Data;
using DoorsOpen.Models;
using Microsoft.AspNetCore.Authorization;

namespace DoorsOpen.Controllers
{
    [Authorize]
    [Route("/admin/groups/{action=Index}/{id?}")]
    public class SecurityGroupsController : Controller
    {
        private readonly SiteDbContext _context;

        public SecurityGroupsController(SiteDbContext context)
        {
            _context = context;
        }

        // GET: SecurityGroups
        public async Task<IActionResult> Index()
        {
            return View(await _context.Groups.ToListAsync());
        }

        // GET: SecurityGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var securityGroup = await _context.Groups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (securityGroup == null)
            {
                return NotFound();
            }

            return View(securityGroup);
        }

        // GET: SecurityGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SecurityGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] SecurityGroup securityGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(securityGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(securityGroup);
        }

        // GET: SecurityGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var securityGroup = await _context.Groups.FindAsync(id);
            if (securityGroup == null)
            {
                return NotFound();
            }
            return View(securityGroup);
        }

        // POST: SecurityGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] SecurityGroup securityGroup)
        {
            if (id != securityGroup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(securityGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SecurityGroupExists(securityGroup.Id))
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
            return View(securityGroup);
        }

        // GET: SecurityGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var securityGroup = await _context.Groups
                .FirstOrDefaultAsync(m => m.Id == id);
            if (securityGroup == null)
            {
                return NotFound();
            }

            return View(securityGroup);
        }

        // POST: SecurityGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var securityGroup = await _context.Groups.FindAsync(id);
            _context.Groups.Remove(securityGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SecurityGroupExists(int id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }
    }
}
