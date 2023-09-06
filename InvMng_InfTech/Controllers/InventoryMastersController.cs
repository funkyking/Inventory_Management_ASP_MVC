using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InvMng_InfTech.Data;
using InvMng_InfTech.Models.Masters;
using System.Diagnostics;
using InvMng_InfTech.Migrations;
using PartsMaster = InvMng_InfTech.Models.Masters.PartsMaster;
using System.Drawing.Drawing2D;

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
            var Brands = _context.PartsMaster
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

        public IActionResult GetPartName(string term, string brand)
       {

            var partNames = new List<string?>();

            if (brand == "" || brand == null)
            {
                partNames = _context.PartsMaster
                    .Where(pn => pn.PartName.Contains(term))
                    .Select(pn => pn.PartName)
                    .Distinct().ToList();
            }
            else
            {
                // Query your database to get part names based on the provided term and brand
                partNames = _context.PartsMaster
                    .Where(p => p.Brand == brand && p.PartName.Contains(term))
                    .Select(p => p.PartName)
                    .Distinct()
                    .ToList();
            }
            return Json(partNames);

        }

        public IActionResult GetPartNumber(string term)
        {
            var partNumber = _context.PartsMaster
                .Where(pn => pn.PartName == term)
                .Select(pn => pn.PartNumber).First();
            return Json(partNumber);
                
        }

        public IActionResult GetSupplierName(string term) 
        {
            var supplierName = _context.SupplyMaster
                .Where(s => s.SupplierName.Contains(term))
                .Select(s => s.SupplierName).ToList();

            return Json(supplierName);
        }


        #endregion


        #region Stock In and Out (logMaster)

        // GET: InventoryMasters/Create
        public IActionResult StockUpdate()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStock([Bind("ID,PartNumber,PartName,Brand,StockNew,StockUsed,Modified,Location,SubLocation")] LogMaster logmaster)
        {

            var _partID = _context.InventoryMaster
                .Where(p => p.PartNumber == logmaster.PartNumber)
                .Select(p => p.ID).FirstOrDefault();

            var _supplierID = _context.SupplyMaster
                .Where(s => s.SupplierName == logmaster.Supplier || s.SupplierName.Contains(logmaster.Supplier))
                .Select(s => s.ID).FirstOrDefault();

            var _LocationID = _context.LocationMaster
               .Where(l => l.Location == logmaster.Location || l.Location.Contains(logmaster.Location))
               .Select(l => l.LocationID).FirstOrDefault();

            var _SubLocationID = _context.SubLocationMaster
               .Where(l => l.SubLocation == logmaster.SubLocation || l.SubLocation.Contains(logmaster.SubLocation))
               .Select(l => l.SubLocationID).FirstOrDefault();

            if (ModelState.IsValid)
            {
                logmaster.ID = Guid.NewGuid();
                logmaster.LogDate = DateTime.Now;
                logmaster.PartID = _partID;
                logmaster.SupplierID = _supplierID;
                logmaster.SubLocationID = _SubLocationID;
                _context.Add(logmaster);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(logmaster);
        }


        #endregion


        #region Parts Master


        public async Task<IActionResult> PartsIndex()
        {
            var parts = await _context.PartsMaster.ToListAsync();

            return _context.InventoryMaster != null ?
                        View("~/Views/InventoryMasters/Parts/Index.cshtml", parts) :
                        Problem("Entity set 'AuthDbContext.PartsMaster' is null.");
        }

        // Show Create Page
        public IActionResult PartsCreate()
        {
            TempData["EditMode"] = true;
            return View("~/Views/InventoryMasters/Parts/Create.cshtml");
        }

        // Creates a new Part
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PartsCreate([Bind("PartID, Brand, PartNumber, PartName, Description, MinNew, MinUsed, Bin")] PartsMaster partmaster)
        {
            if (ModelState.IsValid)
            {
                if (_context.PartsMaster.Any(b => b.PartName == partmaster.PartName && b.PartNumber == partmaster.PartNumber))
                {
                    // Brand, PartName, or PartNumber already exist
                    return Json(new { success = false, message = "The combination of PartName and PartNumber already exists." });
                }

                // If no duplicates, proceed to save the part
                partmaster.PartID = Guid.NewGuid();
                partmaster.Modified = DateTime.Now;
                _context.Add(partmaster);
                await _context.SaveChangesAsync();

                // Return a success response
                return Json(new { success = true });
            }

            // If ModelState is invalid, return a response with validation errors
            var validationErrors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return Json(new { success = false, errors = validationErrors });
        }


        public async Task<IActionResult> PartsEdit(Guid? id)
        {

            if (id == null || _context.PartsMaster == null)
            {
                return NotFound();
            }

            var partsmaster = await _context.PartsMaster.FindAsync(id);
            if (partsmaster == null)
            {
                return NotFound();
            }

            TempData["EditMode"] = true;
            return View("~/Views/InventoryMasters/Parts/Create.cshtml", partsmaster);
            //return View("~/Views/InventoryMasters/Parts/Edit.cshtml", partsmaster);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PartsEdit(Guid id, [Bind("PartID, Brand, PartNumber, PartName, Description, MinNew, MinUsed, Bin")] PartsMaster partsMaster)
        {
            if (id != partsMaster.PartID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    try
                    {

                        // Check if PartNumber is changed
                        var isPartNumberChanged = _context.PartsMaster.Any(p => p.PartID == id && p.PartNumber != partsMaster.PartNumber);

                        if (isPartNumberChanged && _context.PartsMaster.Any(b => b.PartNumber == partsMaster.PartNumber))
                        {
                            // PartNumber already exists
                            return Json(new { success = false, message = "PartNumber already exists." });
                        }

                        // Check if Brand and PartName combination already exists
                        if (_context.PartsMaster.Any(pn => pn.PartID != id && pn.Brand == partsMaster.Brand && pn.PartName == partsMaster.PartName))
                        {
                            // Brand and PartName combination already exists in a different record
                            return Json(new { success = false, message = "Brand and PartName combination already exists in a different record." });
                        }


                        partsMaster.Modified = DateTime.Now;
                        _context.Update(partsMaster);
                        await _context.SaveChangesAsync();

                        return Json(new { success = true });
                    }
                    catch (Exception ex)
                    {

                        return Json(new { success = false, errors = "An error occurred while saving the part." });
                    }
                    
                }
                catch (DbUpdateConcurrencyException)
                {

                    if (!InventoryMasterExists(partsMaster.PartID))
                    { return NotFound();}
                    else
                    { throw;}
                }
                //return RedirectToAction(nameof(Index));
            }
            //return View();
            // If ModelState is invalid, return a response with validation errors
            var validationErrors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return Json(new { success = false, errors = validationErrors });
        }



        public IActionResult PartsDelete()
        {
            return View("~/Views/InventoryMasters/Parts/Delete.cshtml");
        }




        #endregion




    }
}
