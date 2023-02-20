using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CandleShop.Data;
using CandleShop.Models;
using CandleShop.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace CandleShop.Controllers
{
    public class CandlesController : Controller
    {
        private readonly CandleShopContext _context;

        public CandlesController(CandleShopContext context)
        {
            _context = context;
        }

        // GET: Candles
      //  [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Index(string candleCategory, string? candleBrand ,string searchString)
        {
            IQueryable<Candle> candles = _context.Candle.AsQueryable();
            IQueryable<string> categoryQuery = _context.Candle.OrderBy(m => m.Category).Select(m => m.Category).Distinct();
            IQueryable<string> brandQuery = _context.Brand.OrderBy(m => m.Id).Select(m => m.Name).Distinct();
          
            if (!string.IsNullOrEmpty(searchString))
            {
                candles = candles.Where(s => s.Name.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(candleCategory))
            {
                candles = candles.Where(x => x.Category == candleCategory);
            }
            if (!string.IsNullOrEmpty(candleBrand))
            {
                string name = candleBrand;
                var brand = _context.Brand.Where(x => x.Name == name).First();
                candles = candles.Where(x => x.Brand.Id == brand.Id);
            }
            candles = candles.Include(m => m.Users).ThenInclude(m => m.User);
            var vm = new CandlesFilterVM
            {
                Categories = new SelectList(await categoryQuery.ToListAsync()),
                Brands = new SelectList(await brandQuery.ToListAsync()),
                candles = await candles.ToListAsync()
            };
            return View(vm);
        }

        // GET: Candles/Details/5
       // [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Candle == null)
            {
                return NotFound();
            }

            var candle = await _context.Candle
                .Include(b => b.Brand)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (candle == null)
            {
                return NotFound();
            }

            return View(candle);
        }

        // GET: Candles/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["BrandId"] = new SelectList(_context.Brand, "Id", "Name");
            return View();
        }

        // POST: Candles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name,Category,Size,Price,Description,Picture,BrandId")] Candle candle)
        {
            if (ModelState.IsValid)
            {
                _context.Add(candle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BrandId"] = new SelectList(_context.Brand, "Id", "Name", candle.BrandId);
            return View(candle);
        }

        // GET: Candles/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Candle == null)
            {
                return NotFound();
            }

            var candle = await _context.Candle.FindAsync(id);
            if (candle == null)
            {
                return NotFound();
            }
            CandlePicture vm = new CandlePicture
            {
                candle = candle,
                pictureName = candle.Picture
            };
            ViewData["BrandId"] = new SelectList(_context.Brand, "Id", "Name", candle.BrandId);
            return View(vm);
        }

        // POST: Candle/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, CandlePicture vm)
        {
            if (id != vm.candle.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (vm.pictureFile != null)
                    {
                        string uniqueFileName = UploadedFile(vm);
                        vm.candle.Picture = uniqueFileName;
                    }
                    else
                    {
                        vm.candle.Picture = vm.pictureName;
                    }
                    _context.Update(vm.candle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CandleExists(vm.candle.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { id = vm.candle.Id }) ;
            }
            ViewData["BrandId"] = new SelectList(_context.Brand, "Id", "Name", vm.candle.BrandId);
            return View(vm);
        }

        // GET: Candles/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Candle == null)
            {
                return NotFound();
            }

            var candle = await _context.Candle
                .Include(b => b.Brand)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (candle == null)
            {
                return NotFound();
            }

            return View(candle);
        }

        // POST: Candles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Candle == null)
            {
                return Problem("Entity set 'CandleShopContext.Candle'  is null.");
            }
            var candle = await _context.Candle.FindAsync(id);
            if (candle != null)
            {
                _context.Candle.Remove(candle);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        private bool CandleExists(int id)
        {
          return (_context.Candle?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [Authorize(Roles = "Admin")]
        private string UploadedFile(CandlePicture vm)
        {
            string uniqueFileName = null;

            if (vm.pictureFile != null)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/pictures");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(vm.pictureFile.FileName);
                string fileNameWithPath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    vm.pictureFile.CopyTo(stream);
                }
            }
            return uniqueFileName;
        }
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> BrandsCandles(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var brand = await _context.Brand.FirstOrDefaultAsync(x => x.Id == id);
            ViewBag.Brand = brand.Name;
            IQueryable<Candle> candles = _context.Candle.Where(x => x.BrandId == id);
            await _context.SaveChangesAsync();
            return View(await candles.ToListAsync());
        }
    }
}
