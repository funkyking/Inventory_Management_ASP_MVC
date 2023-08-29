using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InvMng_InfTech.Data;
using InvMng_InfTech.Models.Masters;

namespace InvMng_InfTech.Controllers
{
    public class SupplyMastersController : Controller
    {
        private readonly AuthDbContext _context;

        public SupplyMastersController(AuthDbContext context)
        {
            _context = context;
        }

        // GET: SupplyMasters
        public async Task<IActionResult> Index()
        {
              return _context.SupplyMaster != null ? 
                          View(await _context.SupplyMaster.ToListAsync()) :
                          Problem("Entity set 'AuthDbContext.SupplyMaster'  is null.");
        }

       

        // GET: SupplyMasters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SupplyMasters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,SupplierName,Representative,Department,Position,ContactNo,Email,State,City,Postcode,Address,Website,Industry,Modified")] SupplyMaster supplyMaster)
        {
            if (ModelState.IsValid)
            {
                supplyMaster.ID = Guid.NewGuid();
                supplyMaster.Modified = DateTime.Now; 
                _context.Add(supplyMaster);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            TempData["EditMode"] = false;
            return View(supplyMaster);
        }


        // GET: SupplyMasters/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.SupplyMaster == null)
            {
                return NotFound();
            }

            var supplyMaster = await _context.SupplyMaster
                .FirstOrDefaultAsync(m => m.ID == id);
            if (supplyMaster == null)
            {
                return NotFound();
            }

            return View(supplyMaster);
        }


        // GET: SupplyMasters/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.SupplyMaster == null)
            {
                return NotFound();
            }

            var supplyMaster = await _context.SupplyMaster.FindAsync(id);
            if (supplyMaster == null)
            {
                return NotFound();
            }
            else
            {
                supplyMaster.Modified = DateTime.Now;

                // Set TempData value to indicate that this is an edit operation
                TempData["EditMode"] = true;
                return View("Create", supplyMaster);
                //return View(supplyMaster);
            }
            
        }

        // POST: SupplyMasters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,SupplierName,Representative,Department,Position,ContactNo,Email,State,City,Postcode,Address,Website,Industry,Modified")] SupplyMaster supplyMaster)
        {
            if (id != supplyMaster.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    supplyMaster.Modified = DateTime.Now;
                    _context.Update(supplyMaster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplyMasterExists(supplyMaster.ID))
                    {
                        return NotFound();
                    }
                    else
                    { throw; }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(supplyMaster);
        }

        // GET: SupplyMasters/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.SupplyMaster == null)
            {
                return NotFound();
            }

            var supplyMaster = await _context.SupplyMaster
                .FirstOrDefaultAsync(m => m.ID == id);
            if (supplyMaster == null)
            {
                return NotFound();
            }

            return View(supplyMaster);
        }

        // POST: SupplyMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.SupplyMaster == null)
            {
                return Problem("Entity set 'AuthDbContext.SupplyMaster'  is null.");
            }
            var supplyMaster = await _context.SupplyMaster.FindAsync(id);
            if (supplyMaster != null)
            {
                _context.SupplyMaster.Remove(supplyMaster);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupplyMasterExists(Guid id)
        {
          return (_context.SupplyMaster?.Any(e => e.ID == id)).GetValueOrDefault();
        }



    }
}
