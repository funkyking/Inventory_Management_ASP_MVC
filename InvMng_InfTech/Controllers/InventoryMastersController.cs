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
    public class InventoryMastersController : Controller
    {
        private readonly AuthDbContext _context;

        public InventoryMastersController(AuthDbContext context)
        {
            _context = context;
        }

        // GET: InventoryMasters
        public async Task<IActionResult> Index()
        {
              return _context.InventoryMaster != null ? 
                          View(await _context.InventoryMaster.ToListAsync()) :
                          Problem("Entity set 'AuthDbContext.InventoryMaster'  is null.");
        }

        // GET: InventoryMasters/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.InventoryMaster == null)
            {
                return NotFound();
            }

            var inventoryMaster = await _context.InventoryMaster
                .FirstOrDefaultAsync(m => m.ID == id);
            if (inventoryMaster == null)
            {
                return NotFound();
            }

            return View(inventoryMaster);
        }

        // GET: InventoryMasters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InventoryMasters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,PartNumber,PartName,Brand,StockNew,StockUsed,Modified,Location,SubLocation")] InventoryMaster inventoryMaster)
        {
            if (ModelState.IsValid)
            {
                inventoryMaster.ID = Guid.NewGuid();
                _context.Add(inventoryMaster);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(inventoryMaster);
        }

        // GET: InventoryMasters/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.InventoryMaster == null)
            {
                return NotFound();
            }

            var inventoryMaster = await _context.InventoryMaster.FindAsync(id);
            if (inventoryMaster == null)
            {
                return NotFound();
            }
            return View(inventoryMaster);
        }

        // POST: InventoryMasters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,PartNumber,PartName,Brand,StockNew,StockUsed,Modified,Location,SubLocation")] InventoryMaster inventoryMaster)
        {
            if (id != inventoryMaster.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(inventoryMaster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryMasterExists(inventoryMaster.ID))
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
            return View(inventoryMaster);
        }

        // GET: InventoryMasters/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.InventoryMaster == null)
            {
                return NotFound();
            }

            var inventoryMaster = await _context.InventoryMaster
                .FirstOrDefaultAsync(m => m.ID == id);
            if (inventoryMaster == null)
            {
                return NotFound();
            }

            return View(inventoryMaster);
        }

        // POST: InventoryMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.InventoryMaster == null)
            {
                return Problem("Entity set 'AuthDbContext.InventoryMaster'  is null.");
            }
            var inventoryMaster = await _context.InventoryMaster.FindAsync(id);
            if (inventoryMaster != null)
            {
                _context.InventoryMaster.Remove(inventoryMaster);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryMasterExists(Guid id)
        {
          return (_context.InventoryMaster?.Any(e => e.ID == id)).GetValueOrDefault();
        }



        #region Suggestive Text
        
        public IActionResult GetLocations(string term)
        {
            var locations = _context.LocationMaster
                .Where(l => l.Location.Contains(term))
                .Select(l => l.Location)
                .Distinct().ToList();

            return Json(locations);
        }

        public IActionResult GetSubLocations(string term)
        {
            var subLocations = _context.SubLocationMaster
                .Where(sl => sl.SubLocation.Contains(term))
                .Select(sl => sl.SubLocation)
                .Distinct().ToList();

            return Json(subLocations);
        }

        public IActionResult GetBrandName(string term)
        {
            var Brands = _context.InventoryMaster
                .Where(bs => bs.Brand.Contains(term))
                .Select(bs => bs.Brand)
                .Distinct().ToList();

            return Json(Brands);
        }

        public IActionResult GeneratePartNumber()
        {

            var nmbr = "0";
            var random = new Random();
            nmbr = random.Next(1, int.MaxValue).ToString();

            while (_context.InventoryMaster.Any(nb => nb.PartNumber == nmbr))
            {
                nmbr = random.Next(1, int.MaxValue).ToString();
            }
            return Json(nmbr);

        }

        public IActionResult GetPartName(string term)
        {
            var partNames = _context.InventoryMaster
                .Where(pn => pn.PartName.Contains(term))
                .Select(pn => pn.PartName)
                .Distinct().ToList();

            return Json(partNames);
        }

        #endregion


        #region Stock In and Out

        // GET: InventoryMasters/Create
        public IActionResult StockIn()
        {
            return View();
        }

        #endregion




    }
}
