using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DoorsOpen.Data;
using DoorsOpen.Models;

namespace DoorsOpen.Controllers
{
    public class BuildingModelsController : Controller
    {
        private readonly SiteDbContext _context;

        public BuildingModelsController(SiteDbContext context)
        {
            _context = context;
        }

        // GET: BuildingModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Buildings.ToListAsync());
        }

        // GET: BuildingModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buildingModel = await _context.Buildings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (buildingModel == null)
            {
                return NotFound();
            }

            return View(buildingModel);
        }

        // GET: BuildingModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BuildingModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Building,Address1,Address2,City,State,Zip,WheelchairAccessible,RestroomsAvailable,WheelchairAccessibleRestroom,PhotographyAllowed,StartTime,EndTime,Capacity,HistoricalOverview,VisitorExperience")] BuildingModel buildingModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(buildingModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(buildingModel);
        }

        // GET: BuildingModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buildingModel = await _context.Buildings.FindAsync(id);
            if (buildingModel == null)
            {
                return NotFound();
            }
            return View(buildingModel);
        }

        // POST: BuildingModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Building,Address1,Address2,City,State,Zip,WheelchairAccessible,RestroomsAvailable,WheelchairAccessibleRestroom,PhotographyAllowed,StartTime,EndTime,Capacity,HistoricalOverview,VisitorExperience")] BuildingModel buildingModel)
        {
            if (id != buildingModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(buildingModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuildingModelExists(buildingModel.Id))
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
            return View(buildingModel);
        }

        // GET: BuildingModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buildingModel = await _context.Buildings
                .FirstOrDefaultAsync(m => m.Id == id);
            if (buildingModel == null)
            {
                return NotFound();
            }

            return View(buildingModel);
        }

        // POST: BuildingModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var buildingModel = await _context.Buildings.FindAsync(id);
            _context.Buildings.Remove(buildingModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BuildingModelExists(int id)
        {
            return _context.Buildings.Any(e => e.Id == id);
        }
    }
}
