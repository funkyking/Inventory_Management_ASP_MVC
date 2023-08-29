using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InvMng_InfTech.Data;
using InvMng_InfTech.Models.Order;
using InvMng_InfTech.Models.Order.Customers;
using NuGet.Protocol;
using System.Text.Json;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;

namespace InvMng_InfTech.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AuthDbContext _context;

        public OrdersController(AuthDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var authDbContext = _context.Order.Include(o => o.Customer);
            return View(await authDbContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Customer)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            //ViewData["CustomerId"] = new SelectList(_context.Set<Customer>(), "CustomerId", "CustomerId");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,OrderDate,TotalAmount,CustomerId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["CustomerId"] = new SelectList(_context.Set<Customer>(), "CustomerId", "CustomerId", order.Customer.CustomerId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            //ViewData["CustomerId"] = new SelectList(_context.Set<Customer>(), "CustomerId", "CustomerId", order.CustomerId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,OrderDate,TotalAmount,CustomerId")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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
            ViewData["CustomerId"] = new SelectList(_context.Set<Customer>(), "CustomerId", "CustomerId", order.CustomerId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);

        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            // Delete associated orderItems
            var orderItems = await _context.OrderItems
                .Where(oi => oi.OrderId == id).ToListAsync();
            _context.OrderItems.RemoveRange(orderItems);

            // Delete the Order Iteself
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();

            // Redirect to action
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
          return (_context.Order?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }



        #region Customer

        // New action to check customer existence
        public async Task<IActionResult> CheckCustomerExistence(Customer customer)
        {
            // Perform a case-insensitive search for the customer by name
            // cant use 'StringComparison.OrdinalIgnoreCase' because EF Core error
            var existingCustomer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Name.ToLower() == customer.Name.ToLower());

            if (existingCustomer != null)
            {
                // Customer exists, return JSON response with exists=true and customerId
                return Json(new { exists = true, customerId = existingCustomer.CustomerId });
            }
            else
            {
                // Customer does not exist, return JSON response with exists=false
                return Json(new { exists = false });
            }
        }

        // New action to create a customer
        [HttpPost]
        public async Task<IActionResult> CreateNewCustomer(string name, string email)
        {
            try
            {
                // Validate customer name and email fields
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
                {
                    return Json(new { success = false });
                }

                // Convert the input name to lowercase and uppercase for case-insensitive search
                var lowerName = name.ToLower();
                var upperName = name.ToUpper();

                // Check if the customer already exists with the same name (case-insensitive search)
                var existingCustomer = await _context.Customers
                    .FirstOrDefaultAsync(c => c.Name.ToLower() == lowerName || c.Name.ToUpper() == upperName);

                if (existingCustomer != null)
                {
                    // Customer with the same name already exists, return failure
                    return Json(new { success = false });
                }

                // Create a new customer object
                var newCustomer = new Customer
                {
                    Name = name,
                    Email = email
                    // You can add other properties here if needed
                };

                // Add the new customer to the database
                _context.Customers.Add(newCustomer);
                await _context.SaveChangesAsync();

                // Return success with the new customer's ID
                return Json(new { success = true, customerId = newCustomer.CustomerId });
            }
            catch
            {
                // Error occurred during customer creation
                return Json(new { success = false });
            }
        }


        public IActionResult GetCustomerList(string searchText)
        {
            //Fetch a list of Customers
            var customers = _context.Customers
                .Where(c => c.Name.ToLower().Contains(searchText))
                .Select(c => new { c.Name })
                .ToList();

            return Json(customers);
        }

        // Action to fetch all customers from the database
        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            var customers = _context.Customers.Select(c => new { c.Name }).ToList();
            return Json(customers);
        }

        #endregion


        #region Adding Items

        [HttpGet]
        public IActionResult GetBrandList()
        {
            var brands = _context.Items.Select(i => i.Brand).Distinct().ToList();
            return Json(brands);
        }

        [HttpGet]
        public IActionResult GetPartIdList(string brand)
        {
            var partIds = _context.Items
                .Where(i => i.Brand == brand)
                .Select(i => i.PartName)
                .Distinct()
                .ToList();

            return Json(partIds);
        }

        [HttpGet]
        public IActionResult GetRemainingQuantity(string brand, string partId)
        {
            var qty = _context.Items
                .Where(i => i.PartName == partId && i.Brand == brand)
                .Select(i => i.Quantity)
                .FirstOrDefault();

            return Json(qty);
        }


        #endregion


        
        [HttpPost]
        public IActionResult CreateOrderItems([FromBody] List<OrderItem> orderItems)
        {
            try
            {
                // Add the list of OrderItems to the context and save changes
                _context.OrderItems.AddRange(orderItems);
                _context.SaveChanges();

                return Json(new { success = true, orderItems });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }
        }


        [HttpPost]
        public IActionResult CreateOrder([FromBody] JsonElement orderData)
        {
            try
            {
                decimal totalAmount = orderData.GetProperty("totalAmount").GetDecimal();
                int customerId = orderData.GetProperty("customerId").GetInt32();
                JsonElement orderItemsJson = orderData.GetProperty("orderItems");
                List<OrderItem> orderItems = JsonSerializer.Deserialize<List<OrderItem>>(orderItemsJson.GetRawText());

                // Create a new Order instance
                var newOrder = new Order
                {
                    OrderDate = DateTime.Now,
                    TotalAmount = totalAmount,
                    CustomerId = customerId
                };

                // Add the new Order to the context and save changes to get the generated OrderId
                _context.Order.Add(newOrder);
                _context.SaveChanges();

                return Json(new { success = true, orderId = newOrder.OrderId });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error in CreateOrder"}); //$"An error occurred: {ex.Message}" 
            }
        }


        [HttpPost]
        public IActionResult UpdateOrderItemsWithOrderId([FromBody] List<OrderItem> orderItems)
        {
            try
            {
                foreach (var orderItem in orderItems)
                {
                    var existingOrderItem = _context.OrderItems.FirstOrDefault(oi => oi.OrderItemId == orderItem.OrderItemId);
                    if (existingOrderItem != null)
                    {
                        existingOrderItem.OrderId = orderItem.OrderId;
                        
                    }
                }

                _context.SaveChanges();

                return Json(new { success = true});
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error In UpdateOrder" }); //$"An error occurred: {ex.Message}" 
            }
        }









    }
}
