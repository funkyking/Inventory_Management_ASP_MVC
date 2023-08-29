using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InvMng_InfTech.Data;
using InvMng_InfTech.Models.Location;

namespace InvMng_InfTech.Controllers
{
    public class LocationMastersController : Controller
    {
        private readonly AuthDbContext _context;

        public LocationMastersController(AuthDbContext context)
        {
            _context = context;
        }

        // GET: LocationMasters
        public async Task<IActionResult> Index()
        {
              return _context.LocationMaster != null ? 
                          View(await _context.LocationMaster.ToListAsync()) :
                          Problem("Entity set 'AuthDbContext.LocationMaster'  is null.");
        }

        // GET: LocationMasters/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.LocationMaster == null)
            {
                return NotFound();
            }

            var locationMaster = await _context.LocationMaster
                .FirstOrDefaultAsync(m => m.LocationID == id);
            if (locationMaster == null)
            {
                return NotFound();
            }

            return View(locationMaster);
        }

        // GET: LocationMasters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LocationMasters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LocationID,Location,Description,ModifiedDate")] LocationMaster locationMaster)
        {   
            if (ModelState.IsValid)
            {   locationMaster.ModifiedDate = DateTime.Now;
                _context.Add(locationMaster);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(locationMaster);
        }

        // GET: LocationMasters/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.LocationMaster == null)
            {
                return NotFound();
            }

            var locationMaster = await _context.LocationMaster.FindAsync(id);
            if (locationMaster == null)
            {
                return NotFound();
            }
            TempData["EditMode"] = true;
            return View("Create",locationMaster);
        }

        // POST: LocationMasters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id, [Bind("LocationID,Location,Description,ModifiedDate")] LocationMaster locationMaster)
        {
            if (id != locationMaster.LocationID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    locationMaster.ModifiedDate = DateTime.Now;
                    _context.Update(locationMaster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationMasterExists(locationMaster.LocationID))
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
            return View(locationMaster);
        }

        // GET: LocationMasters/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.LocationMaster == null)
            {
                return NotFound();
            }

            var locationMaster = await _context.LocationMaster
                .FirstOrDefaultAsync(m => m.LocationID == id);
            if (locationMaster == null)
            {
                return NotFound();
            }

            return View(locationMaster);
        }

        // POST: LocationMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
        {
            if (_context.LocationMaster == null)
            {
                return Problem("Entity set 'AuthDbContext.LocationMaster'  is null.");
            }
            var locationMaster = await _context.LocationMaster.FindAsync(id);
            if (locationMaster != null)
            {
                _context.LocationMaster.Remove(locationMaster);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocationMasterExists(Guid? id)
        {
          return (_context.LocationMaster?.Any(e => e.LocationID == id)).GetValueOrDefault();
        }
    }
}
