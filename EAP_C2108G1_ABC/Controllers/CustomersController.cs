using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EAP_C2108G1_ABC.Data;
using EAP_C2108G1_ABC.Models;
using X.PagedList;

namespace EAP_C2108G1_ABC.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CustomerSet.Include(c => c.ClassEntity);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CustomerSet == null)
            {
                return NotFound();
            }

            var customer = await _context.CustomerSet
                .Include(c => c.ClassEntity)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            ViewData["ClassId"] = new SelectList(_context.ClassSet, "ClassId", "ClassName");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,FullName,Birthday,Address,Email,UserName,Password,ConfirmPassword,ClassId")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClassId"] = new SelectList(_context.ClassSet, "ClassId", "ClassName", customer.ClassId);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CustomerSet == null)
            {
                return NotFound();
            }

            var customer = await _context.CustomerSet.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["ClassId"] = new SelectList(_context.ClassSet, "ClassId", "ClassName", customer.ClassId);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,FullName,Birthday,Address,Email,UserName,Password,ConfirmPassword,ClassId")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
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
            ViewData["ClassId"] = new SelectList(_context.ClassSet, "ClassId", "ClassName", customer.ClassId);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CustomerSet == null)
            {
                return NotFound();
            }

            var customer = await _context.CustomerSet
                .Include(c => c.ClassEntity)
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CustomerSet == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CustomerSet'  is null.");
            }
            var customer = await _context.CustomerSet.FindAsync(id);
            if (customer != null)
            {
                _context.CustomerSet.Remove(customer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
          return (_context.CustomerSet?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }

        public IActionResult Index(string q, int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 5;
            List<Customer> customers = _context.CustomerSet.Include(e => e.ClassEntity).ToList();
            List<Customer> list = new List<Customer>();
            if (!string.IsNullOrEmpty(q))
            {
                ViewData["keyword"] = q;
                string[] each = q.Split(' ');
                if (each.Length > 0)
                {
                    foreach (var key in each)
                    {
                        customers.Where(d => d.FullName.ToLower().Contains(key.ToLower()))
                           .ToList().ForEach(d => list.Add(d));
                    }
                    customers = new List<Customer>();
                    list.ForEach(l => customers.Add(l));
                }
            }
            return View(customers.ToPagedList(pageNumber, pageSize));
        }

    }
}
