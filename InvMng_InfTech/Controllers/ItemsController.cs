using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InvMng_InfTech.Data;
using InvMng_InfTech.Models;
using Microsoft.AspNetCore.Identity;
using InvMng_InfTech.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;

namespace InvMng_InfTech.Controllers
{

    [Authorize]
    //added its authorize element here so that when unregistered or unlogin users cant access this controller.
    public class ItemsController : Controller
    {
        private readonly AuthDbContext _context;
        public UserManager<ApplicationUser> _userManager;
        

        public ItemsController(AuthDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            this._userManager = userManager;
        }


        // Dashboard
        public IActionResult Dashboard()
        {
            ViewData["UserName"] = _userManager.GetUserName(this.User);
            ViewData["UserID"] = _userManager.GetUserId(this.User);


            var customers = _context.Customers.ToList();
            var parts = _context.Items.ToList();
            var orderItemsList = _context.OrderItems.ToList();
            var ordersList = _context.Order.ToList();


            //Most Order Customer
            var mostOrderId = orderItemsList
                .GroupBy(oi => oi.OrderId)
                .OrderByDescending(group => group.Count())
                .Select(group => group.Key)
                .FirstOrDefault();

            var mostOrderedItems = orderItemsList
               .Where(oi => oi.OrderId == mostOrderId)
               .ToList();

            var mostItemsOrder = ordersList
                .FirstOrDefault(o => o.OrderId == mostOrderId);


            //Latest Customer Order

            var latestOrderId = ordersList
                .OrderByDescending(d => d.OrderDate)
                .Select(d => d.OrderId)
                .FirstOrDefault();

            var latestOrder = ordersList
                .FirstOrDefault(o => o.OrderId == latestOrderId);

            var latestOrderItems = orderItemsList
                .Where(oi => oi.OrderId == latestOrderId)
                .ToList();


            var dvm = new DashboardViewModel
            {
                HighestStock = parts.OrderByDescending(x => x.Quantity).FirstOrDefault(), // Retrieve the item with the highest stock
                LowestStock = parts.OrderBy(i => i.Quantity).FirstOrDefault(), // Retrieve the item with the lowest stock
                LatestStock = parts.OrderByDescending(i => i.DateAdded).FirstOrDefault(),



                LatestCustomerOrder = latestOrder,
                LatestCustomerItems = latestOrderItems,
                CustomerWithMostItems = mostItemsOrder,
                CustomerWithMostOrderItems = mostOrderedItems

            };



            // Total Customers We Have
            dvm.TotalCustomers = customers.ToList().Count();


            // Total Sales
            dvm.TotalSales = ordersList.Sum(sum => sum.TotalAmount);

            // Total Parts Available
            dvm.TotalParts = parts.ToList().Count();

            // Get All the Brands Available
            dvm.TotalBrands = parts
                .Select(item => item.Brand)
                .Distinct().Count();

            // Get the Brand with Most Parts
            dvm.FavouriteBrand = parts
                .GroupBy(item => item.Brand)
                .OrderByDescending(group => group.Count())
                .Select(group => group.Key)
                .FirstOrDefault();


            // Get the Brand with Least Parts
            dvm.leastFavouriteBrand = parts
                .GroupBy(item => item.Brand)
                .OrderBy(group => group.Count())
                .Select(group => group.Key)
                .FirstOrDefault();


            return View(dvm);
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            ViewData["UserName"] = _userManager.GetUserName(this.User);
            ViewData["UserID"] = _userManager.GetUserId(this.User);
            return _context.Items != null ? 
                          View(await _context.Items.ToListAsync()) :
                          Problem("Entity set 'AuthDbContext.Items'  is null.");
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewData["UserName"] = _userManager.GetUserName(this.User);
            ViewData["UserID"] = _userManager.GetUserId(this.User);
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var items = await _context.Items
                .FirstOrDefaultAsync(m => m.Id == id);
            if (items == null)
            {
                return NotFound();
            }

            return View(items);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            ViewData["UserName"] = _userManager.GetUserName(this.User);
            ViewData["UserID"] = _userManager.GetUserId(this.User);
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Brand,PartName,Quantity,DateAdded,UserThatAdded")] Items items)
        {
            if (ModelState.IsValid)
            {
                bool partIdExists = await _context.Items.AnyAsync(i => i.PartName == items.PartName);
                if (partIdExists)
                {
                    ModelState.AddModelError("PartName", "The Part already Exist");
                    TempData["AlertMessage"] = "PartName >" + "The Part already Exist";
                    return RedirectToAction(nameof(Index));
                }

                _context.Add(items);
                await _context.SaveChangesAsync();
                TempData["AlertMessage"] = " Added a New Item";
                return RedirectToAction(nameof(Index));
            }
            return View(items);
        }



        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["UserName"] = _userManager.GetUserName(this.User);
            ViewData["UserID"] = _userManager.GetUserId(this.User);
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var items = await _context.Items.FindAsync(id);
            if (items == null)
            {
                return NotFound();
            }
            return View(items);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Brand,PartName,Quantity,DateAdded,UserThatAdded")] Items items)
        {

            ViewData["UserName"] = _userManager.GetUserName(this.User);
            ViewData["UserID"] = _userManager.GetUserId(this.User);
            if (id != items.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(items);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemsExists(items.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["AlertMessage"] = " Updated a Item";
                return RedirectToAction(nameof(Index));
            }
            return View(items);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Items == null)
            {
                return NotFound();
            }

            var items = await _context.Items
                .FirstOrDefaultAsync(m => m.Id == id);
            if (items == null)
            {
                return NotFound();
            }

            return View(items);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewData["UserName"] = _userManager.GetUserName(this.User);
            ViewData["UserID"] = _userManager.GetUserId(this.User);
            if (_context.Items == null)
            {
                return Problem("Entity set 'AuthDbContext.Items'  is null.");
            }
            var items = await _context.Items.FindAsync(id);
            if (items != null)
            {
                _context.Items.Remove(items);
            }
            
            await _context.SaveChangesAsync();
            TempData["AlertMessage"] = " Deleted a Item";
            return RedirectToAction(nameof(Index));
        }

        private bool ItemsExists(int id)
        {
          return (_context.Items?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
