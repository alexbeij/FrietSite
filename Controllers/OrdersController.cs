using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FrietSite.Data;
using FrietSite.Models;

namespace FrietSite.Controllers
{
    public class OrdersController : Controller
    {
        private readonly Db _context;

        public OrdersController(Db context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            return View(await _context.Orders.Include(o => o.Products).ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Products)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            var products = _context.Products.ToList();  
            var viewModel = new Orderviewmodel
            {
                AvailableProducts = products
            };

            return View(viewModel);
        }

        // POST: Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Orderviewmodel viewModel)
        {
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    var key = state.Key;
                    var value = state.Value;

                    Console.WriteLine($"Key: {key}");

                    foreach (var error in value.Errors)
                    {
                        Console.WriteLine($"Error: {error.ErrorMessage}");
                    }
                }
            }


            if (ModelState.IsValid)
            {
                var selectedProducts = await _context.Products
                    .Where(p => viewModel.SelectedProductIds.Contains(p.Id))
                    .ToListAsync();

                var order = new Order
                {
                    Description = viewModel.Description,
                    Products = selectedProducts  
                };

                _context.Orders.Add(order);  
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

             
            viewModel.AvailableProducts = await _context.Products.ToListAsync();
            return View(viewModel);
        }


        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.Include(o => o.Products).FirstOrDefaultAsync(o => o.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            var viewModel = new Orderviewmodel
            {
                Id = order.Id,
                Description = order.Description,
                SelectedProductIds = order.Products.Select(p => p.Id).ToList(),
                AvailableProducts = await _context.Products.ToListAsync()
            };

            return View(viewModel);
        }

        // POST: Orders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Orderviewmodel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var order = await _context.Orders.Include(o => o.Products).FirstOrDefaultAsync(o => o.Id == id);
                    if (order == null)
                    {
                        return NotFound();
                    }

                    order.Description = viewModel.Description;


                    var selectedProducts = await _context.Products
                        .Where(p => viewModel.SelectedProductIds.Contains(p.Id))
                        .ToListAsync();

                    order.Products.Clear();
                    order.Products = selectedProducts;

                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(viewModel.Id))
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

            viewModel.AvailableProducts = await _context.Products.ToListAsync();
            return View(viewModel);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Products)
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
