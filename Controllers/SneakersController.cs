using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ecommerce_csharp.Data;
using ecommerce_csharp.Models;

namespace ecommerce_csharp.Controllers
{
    public class SneakersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SneakersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sneakers
        public async Task<IActionResult> Index(string gender)
        {
            var sneakers = _context.Sneakers.AsQueryable();

            if (!string.IsNullOrEmpty(gender))
            {
                sneakers = sneakers.Where(s => s.Gender == gender);
            }

            return View(await sneakers.ToListAsync());
        }

        // GET: Sneakers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sneaker = await _context.Sneakers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sneaker == null)
            {
                return NotFound();
            }

            return View(sneaker);
        }

        // GET: Sneakers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sneakers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sneaker sneaker, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    sneaker.ImageUrl = "/images/" + uniqueFileName;
                }

                _context.Add(sneaker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(sneaker);
        }

        // GET: Sneakers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sneaker = await _context.Sneakers.FindAsync(id);
            if (sneaker == null)
            {
                return NotFound();
            }
            return View(sneaker);
        }

        // POST: Sneakers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Brand,Price,StockQuantity,ImageUrl,Gender")] Sneaker sneaker)
        {
            if (id != sneaker.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sneaker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SneakerExists(sneaker.Id))
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
            return View(sneaker);
        }

        // GET: Sneakers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sneaker = await _context.Sneakers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sneaker == null)
            {
                return NotFound();
            }

            return View(sneaker);
        }

        // POST: Sneakers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sneaker = await _context.Sneakers.FindAsync(id);
            if (sneaker != null)
            {
                _context.Sneakers.Remove(sneaker);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SneakerExists(int id)
        {
            return _context.Sneakers.Any(e => e.Id == id);
        }
    }
}
