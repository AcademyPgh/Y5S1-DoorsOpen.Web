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
    public class EventModelsController : Controller
    {
        private readonly SiteDbContext _context;

        public EventModelsController(SiteDbContext context)
        {
            _context = context;
        }

        // GET: EventModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.Events
                .OrderByDescending(m => m.IsActive)
                .ThenBy(m => m.StartDate)
                .ToListAsync());
        }

        // GET: EventModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventModel = await _context.Events
                .Include(m => m.Buildings)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventModel == null)
            {
                return NotFound();
            }

            return View(eventModel);
        }

        // GET: EventModels/Create
        public IActionResult Create()
        {
            var buildings = _context.Buildings.ToList();
            ViewData["buildings"] = buildings;

            return View();
        }

        // POST: EventModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDate,EndDate,IsActive")] EventModel eventModel, int[] selectedBuildings)
        {
            if (ModelState.IsValid)
            {                
                eventModel.Buildings = new List<BuildingModel>();
                // loop through the selected buildings Ids
                foreach (int bId in selectedBuildings)
                {
                    // retrieve a building from the db for the id
                    BuildingModel b = _context.Buildings.Where(b => b.Id == bId).FirstOrDefault();
                    // add the building to the list of buildings for this event
                    eventModel.Buildings.Add(b);
                }

                _context.Add(eventModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eventModel);
        }

        public async Task<IActionResult> SetActive(int id)
        {
            var eventModel = await _context.Events.FirstOrDefaultAsync(m => m.Id == id);
            eventModel.IsActive = true;

            var allOtherEvents = await _context.Events.Where(m => m.Id != id).ToListAsync();
            foreach (EventModel e in allOtherEvents)
            {
                e.IsActive = false;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: EventModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventModel = await _context.Events
            .Include(m => m.Buildings)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (eventModel == null)
            {
                return NotFound();
            }

            // get a list of building ids that belong to the event
            var checkedBuildings = eventModel.Buildings.Select(b => b.Id).ToList();
            ViewBag.checkedBuildings = checkedBuildings;

            // Get a list of all buildings in the db and pass it to the view
            var buildings = _context.Buildings.ToList();
            ViewData["buildings"] = buildings;

            return View(eventModel);
        }

        // POST: EventModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,EndDate,IsActive")] EventModel eventModel, int[] selectedBuildings)
        {

            // clear the relationship between the event being edited and the current buildings associated with it
            var eventToEdit = _context.Events
                .Where(m => m.Id == id)
                .Include(m => m.Buildings)
                .FirstOrDefault();
            eventToEdit.Buildings.Clear();

            if (id != eventModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // loop through the selected buildings Ids
                foreach (int bId in selectedBuildings)
                {
                    // retrieve a building from the db for the id
                    BuildingModel b = _context.Buildings.Where(b => b.Id == bId).FirstOrDefault();
                    // add the building to the list of buildings for this event
                    eventToEdit.Buildings.Add(b);
                }

                // update remaining information from the form
                eventToEdit.Name = eventModel.Name;
                eventToEdit.StartDate = eventModel.StartDate;
                eventToEdit.EndDate = eventModel.EndDate;
                eventToEdit.IsActive = eventModel.IsActive;
                // save all changes to database
                _context.SaveChanges();

                // redirect to the details view for this event
                return RedirectToAction("Details", new { id = id });
            }
            return View(eventModel);
        }

        // GET: EventModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventModel = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventModel == null)
            {
                return NotFound();
            }

            return View(eventModel);
        }

        // POST: EventModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventModel = await _context.Events.FindAsync(id);
            _context.Events.Remove(eventModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventModelExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
