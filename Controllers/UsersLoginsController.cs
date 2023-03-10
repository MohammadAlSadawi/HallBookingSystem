using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FirstProjectHallBooking.Models;

namespace FirstProjectHallBooking.Controllers
{
    public class UsersLoginsController : Controller
    {
        private readonly ModelContext _context;

        public UsersLoginsController(ModelContext context)
        {
            _context = context;
        }

        // GET: UsersLogins
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.UsersLogins.Include(u => u.Role).Include(u => u.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: UsersLogins/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usersLogin = await _context.UsersLogins
                .Include(u => u.Role)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usersLogin == null)
            {
                return NotFound();
            }

            return View(usersLogin);
        }

        // GET: UsersLogins/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Rolees, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: UsersLogins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserName,Password,RoleId,UserId")] UsersLogin usersLogin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usersLogin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Rolees, "Id", "Id", usersLogin.RoleId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", usersLogin.UserId);
            return View(usersLogin);
        }

        // GET: UsersLogins/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usersLogin = await _context.UsersLogins.FindAsync(id);
            if (usersLogin == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Rolees, "Id", "Id", usersLogin.RoleId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", usersLogin.UserId);
            return View(usersLogin);
        }

        // POST: UsersLogins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,UserName,Password,RoleId,UserId")] UsersLogin usersLogin)
        {
            if (id != usersLogin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usersLogin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersLoginExists(usersLogin.Id))
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
            ViewData["RoleId"] = new SelectList(_context.Rolees, "Id", "Id", usersLogin.RoleId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", usersLogin.UserId);
            return View(usersLogin);
        }

        // GET: UsersLogins/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usersLogin = await _context.UsersLogins
                .Include(u => u.Role)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usersLogin == null)
            {
                return NotFound();
            }

            return View(usersLogin);
        }

        // POST: UsersLogins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var usersLogin = await _context.UsersLogins.FindAsync(id);
            _context.UsersLogins.Remove(usersLogin);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersLoginExists(decimal id)
        {
            return _context.UsersLogins.Any(e => e.Id == id);
        }
    }
}
