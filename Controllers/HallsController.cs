using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FirstProjectHallBooking.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace FirstProjectHallBooking.Controllers
{
    public class HallsController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HallsController(ModelContext context,IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Halls
        public async Task<IActionResult> Index()
        {
            ViewBag.adminname = HttpContext.Session.GetString("Adminname");
            var modelContext = _context.Halls.Include(h => h.Address);
            return View(await modelContext.ToListAsync());
        }

        // GET: Halls/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hall = await _context.Halls
                .Include(h => h.Address)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hall == null)
            {
                return NotFound();
            }

            return View(hall);
        }

        // GET: Halls/Create
        public IActionResult Create()
        {
            ViewBag.adminname = HttpContext.Session.GetString("Adminname");
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id");
            return View();
        }

        // POST: Halls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,AddressId,Imagepath,ImageFile")] Hall hall)
        {
            ViewBag.adminname = HttpContext.Session.GetString("Adminname");
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string filename = Guid.NewGuid().ToString() + "_" + hall.ImageFile.FileName;
                string path = Path.Combine(wwwRootPath + "/Image/", filename);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await hall.ImageFile.CopyToAsync(fileStream);
                }
                hall.Imagepath = filename;
                _context.Add(hall);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", hall.AddressId);
            return View(hall);
        }

        // GET: Halls/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            ViewBag.adminname = HttpContext.Session.GetString("Adminname");
            if (id == null)
            {
                return NotFound();
            }

            var hall = await _context.Halls.FindAsync(id);
            if (hall == null)
            {
                return NotFound();
            }
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "City", hall.AddressId);
            return View(hall);
        }

        // POST: Halls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,Name,Description,Price,AddressId,Imagepath,ImageFile")] Hall hall)
        {
            ViewBag.adminname = HttpContext.Session.GetString("Adminname");
            if (id != hall.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (hall.ImageFile != null)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + hall.ImageFile.FileName;
                        string path = Path.Combine(wwwRootPath + "/Image/", fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await hall.ImageFile.CopyToAsync(fileStream);
                        }
                        hall.Imagepath = fileName;
                    }
                    else
                    {
                        var data = _context.Halls.AsNoTracking().Where(x => x.Id == hall.Id).FirstOrDefault();
                        string PersonalImagepath = data.Imagepath;
                        hall.Imagepath = PersonalImagepath;
                    }
                    _context.Update(hall);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HallExists(hall.Id))
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
            ViewData["AddressId"] = new SelectList(_context.Addresses, "Id", "Id", hall.AddressId);
            return View(hall);
        }

        // GET: Halls/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hall = await _context.Halls
                .Include(h => h.Address)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hall == null)
            {
                return NotFound();
            }

            return View(hall);
        }

        // POST: Halls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var hall = await _context.Halls.FindAsync(id);
            _context.Halls.Remove(hall);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HallExists(decimal id)
        {
            return _context.Halls.Any(e => e.Id == id);
        }
    }
}
