﻿using System;
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
using Microsoft.VisualBasic;

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

        [HttpGet]
        public async Task<IActionResult> _ExistingStockCheck(string PartNumber)
        {

            var status = false;
            if (await _context.InventoryMaster
                .AnyAsync(p => p.PartNumber == PartNumber))
            {
                status = true;
            }
            return Json(status);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStock([Bind("ID,PartNumber,PartName,Brand,stockInOut,StockNew,StockUsed,Modified,Location,SubLocation,Supplier, remark")] LogMaster logmaster)
        {

            var _part = await _context.PartsMaster
                .FirstOrDefaultAsync(id => id.PartNumber == logmaster.PartNumber);

            if (ModelState.IsValid)
            {

                logmaster.ID = Guid.NewGuid();
                logmaster.LogDate = DateTime.Now;
                logmaster.PartID = _part?.PartID;

                if (_context.InventoryMaster.Any(pid => pid.ID == _part.PartID))
                {
                    var partrow = await _context.InventoryMaster
                            .FirstOrDefaultAsync(m => m.ID == _part.PartID);

                    //Need to add logic to pass the supplier assosciated
                    //logmaster.supplier = 


                    var _Location = await _context.LocationMaster
                        .FirstOrDefaultAsync(l => l.Location == partrow.Location || l.Location.Contains(partrow.Location));

                    var _SubLocation = await _context.SubLocationMaster
                        .FirstOrDefaultAsync(l => l.SubLocation == partrow.SubLocation || l.SubLocation.Contains(partrow.SubLocation));


                    if (logmaster.StockNew != 0 & logmaster.StockNew != null)
                    {
                        logmaster.StockInOut = "Stock In";
                    }

                    if (logmaster.StockUsed != 0 && logmaster.StockUsed != null)
                    {
                        logmaster.StockInOut = "Stock Out";
                    }


                    // Stock In
                    if (logmaster.StockInOut.Contains("In"))
                    {
                        if (partrow.StockNew == null) { partrow.StockNew = 0; }
                        partrow.StockNew += logmaster.StockNew;
                    }

                    // Stock out
                    if (logmaster.StockInOut.Contains("Out"))
                    {
                        if (partrow.StockUsed == null) { partrow.StockUsed = 0; }
                        partrow.StockUsed += logmaster.StockUsed;
                        if (partrow.StockNew == null) { partrow.StockNew = 0; }
                        partrow.StockNew -= logmaster.StockUsed;
                    }
                    partrow.Modified = DateTime.Now;
                    _context.Update(partrow);
                    await _context.SaveChangesAsync();


                    logmaster.Location = partrow.Location;
                    logmaster.SubLocation = partrow.SubLocation;
                    logmaster.StockNew = partrow.StockNew;
                    logmaster.StockUsed = partrow.StockUsed;
                    logmaster.LogDate = DateTime.Now;
                }
                else
                {
                    var _supplier = await _context.SupplyMaster
                        .FirstOrDefaultAsync(s => s.SupplierName == logmaster.Supplier || s.SupplierName.Contains(logmaster.Supplier));

                    var _Location = await _context.LocationMaster
                        .FirstOrDefaultAsync(l => l.Location == logmaster.Location || l.Location.Contains(logmaster.Location));

                    var _SubLocation = await _context.SubLocationMaster
                        .FirstOrDefaultAsync(l => l.SubLocation == logmaster.SubLocation || l.SubLocation.Contains(logmaster.SubLocation));

                    
                    logmaster.SupplierID = _supplier?.ID;
                    logmaster.LocationID = _Location?.LocationID;
                    logmaster.SubLocationID = _SubLocation?.SubLocationID;
                   
                    //create new row in inventory master and add the logmaster values.
                    var newInvRow = new InventoryMaster
                    {
                        ID = _part.PartID,
                        PartNumber = _part.PartNumber,
                        PartName = _part.PartName,
                        Brand = _part.Brand,
                        Location = logmaster.Location,
                        SubLocation = logmaster.SubLocation,
                        StockNew = logmaster.StockNew,
                        StockUsed = logmaster.StockUsed,
                        Modified = DateTime.Now
                    };
                    _context.Add(newInvRow);
                    await _context.SaveChangesAsync();

                }

                _context.Add(logmaster);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(logmaster);
        }


        [HttpGet]
        public async Task<IActionResult> GetLogData()
        {
            var data = await _context.LogMaster
                .OrderByDescending(l => l.LogDate)
                .Select(l => new
                {
                    Name = l.PartName,
                    Type = l.StockInOut == "Stock In" ? "In" : "Out",
                    Time = l.LogDate.ToString("dd/MM HH:mm")
                }).Take(5).ToListAsync();

            return Json(data);
        }


        [HttpGet]
        public async Task<IActionResult> SelectedRowDetails(string partNumber)
        {

            var data = await _context.PartsMaster
                .Where(p => p.PartNumber == partNumber)
                .Select(p => new
                {
                    brand= p.Brand,
                    name = p.PartName,
                    number = p.PartNumber
                }).FirstOrDefaultAsync();

            return Json(data);   
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
            TempData["EditMode"] = false;
            return View("~/Views/InventoryMasters/Parts/Create.cshtml");
        }

        // Creates a new Part
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PartsCreate([Bind("PartID, Brand, PartNumber, PartName, Description, MinNew, MinUsed, Bin")] PartsMaster partmaster)
        {
            if (ModelState.IsValid)
            {

                if (partmaster.Brand == null || partmaster.PartName == null || partmaster.PartNumber == null)
                {
                    return Json(new { success = false, message = "Incomplete Input (Main)" });
                }

                if (_context.PartsMaster.Any(pn => pn.PartNumber == partmaster.PartNumber))
                {
                    return Json(new { success = false, message = "This Part Number is Already Used. Please Select another Number." });
                }

                if (_context.PartsMaster.Any(b => b.Brand == partmaster.Brand && b.PartName == partmaster.PartName || b.PartName == partmaster.PartNumber))
                {
                    return Json(new { success = false, message = "This Brand already Contains the Part." });
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
