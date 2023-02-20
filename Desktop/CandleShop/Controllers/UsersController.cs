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
using CandleShop.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;

namespace CandleShop.Controllers
{
    public class UsersController : Controller
    {
        private readonly CandleShopContext _context;
        private readonly UserManager<CandleShopUser> _userManager;
        private readonly SignInManager<CandleShopUser> _signInManager;
        public UsersController(CandleShopContext context, UserManager<CandleShopUser> userManager, SignInManager<CandleShopUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Users
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
              return _context.User != null ? 
                          View(await _context.User.ToListAsync()) :
                          Problem("Entity set 'CandleShopContext.User'  is null.");
        }

        // GET: Users/Details/5
      //  [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }
            var pom = _context.Users.Where(x => x.user_ID == id).FirstOrDefault();
            if (pom != null)
            {
                ViewBag.Email = pom.Email;
            }
            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            UserEditPicture vm = new UserEditPicture
            {
                user = user,
                pictureName = user.ProfilePicture
            };

            return View(vm);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,ProfilePicture,user")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            UserEditPicture vm = new UserEditPicture
            {
                user = user,
                pictureName = user.ProfilePicture
            };
            return View(vm);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Edit(int id, UserEditPicture vm)
        {
            if (id != vm.user.Id)
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
                        vm.user.ProfilePicture = uniqueFileName;
                    }
                    else
                    {
                        vm.user.ProfilePicture = vm.pictureName;
                    }
                    _context.Update(vm.user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(vm.user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", new {id = id});
            }
            return View(vm);
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            var pom = _context.Users.Where(x => x.user_ID == id).FirstOrDefault();
            if (pom != null)
            {
                ViewBag.Email = pom.Email;
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.User == null)
            {
                return Problem("Entity set 'CandleShopContext.User'  is null.");
            }
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        private bool UserExists(int id)
        {
          return (_context.User?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        [Authorize(Roles = "User")]
        public async Task<IActionResult> OrderMoreCandles(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _context.User.Where(x => x.Id == id).Include(x => x.Candles).First();
            if (user == null)
            {
                return NotFound();
            }
            var candles = _context.Candle.AsEnumerable();
            candles = candles.OrderBy(s => s.Name);
            MoreCandlesVM vm = new MoreCandlesVM
            {
                user = user,
                candleList = new MultiSelectList(candles, "Id", "Name"),
                selectedCandles = user.Candles.Select(x => x.CandleId)
            };
            ViewBag.Message = vm.user.FullName;
            return View(vm);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> OrderMoreCandles(int id, MoreCandlesVM vm)
        {
            if (id != vm.user.Id)
            {
                return NotFound();
            }


                
                   // _context.Update(vm.user);
                    //await _context.SaveChangesAsync();
                    IEnumerable<int> listCandles = vm.selectedCandles;
                        IQueryable<Order> toBeRemoved = _context.Order.Where(s => !listCandles.Contains(s.CandleId) && s.UserId == id);
                        _context.Order.RemoveRange(toBeRemoved);
                        IEnumerable<int> exist = _context.Order.Where(x => listCandles.Contains(x.CandleId) && x.UserId == id).Select(x => x.CandleId);
                        IEnumerable<int> newEn = listCandles.Where(x => !exist.Contains(x));

                        foreach (int candle in newEn)
                            _context.Add(new Order { Status="Pending Approval", CandleId = candle, UserId = id, Location=vm.location });

            await _context.SaveChangesAsync();
            return RedirectToAction("Orders","Orders", new {id=id});
        }
        [Authorize(Roles = "User")]
        private string UploadedFile(UserEditPicture vm)
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
        public async Task<IActionResult> CreateAcc(int? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View();
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAcc(int id, [Bind("Id,FirstName,LastName,ProfilePicture,user")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index","Home");
            }
            return View(user);
        }
        [Authorize(Roles = "User")]
        public async Task<IActionResult> OrderACandle(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var candle =await  _context.Candle.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (candle == null)
            {
                return NotFound();
            }
           // var candle = _context.Candle.Where(x => x.Id == CandleId).First();
            var userIdentity = _userManager.GetUserAsync(User).Result.user_ID;
            var user = await _context.User.Where(x => x.Id == userIdentity).FirstOrDefaultAsync();
            CandleOrder vm = new CandleOrder
            {
                user = user,
                candleId = candle.Id
            };
            ViewBag.User = vm.user.FullName;
            ViewBag.Candle = candle.Name;
            ViewBag.Price = candle.Price;
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> OrderACandle(int id, CandleOrder vm)
        {

            _context.Add(new Order { Status = "Pending Approval", CandleId =id, UserId = vm.user.Id, Location = vm.location });
            await _context.SaveChangesAsync();
            return RedirectToAction("Orders", "Orders", new { id = vm.user.Id });
        }
    }
}

