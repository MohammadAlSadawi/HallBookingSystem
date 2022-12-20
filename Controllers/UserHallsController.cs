using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FirstProjectHallBooking.Models;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;

namespace FirstProjectHallBooking.Controllers
{
    public class UserHallsController : Controller
    {
        private readonly ModelContext _context;

        public UserHallsController(ModelContext context)
        {
            _context = context;
        }

        // GET: UserHalls
        public async Task<IActionResult> Index()
        {
            ViewBag.adminname = HttpContext.Session.GetString("Adminname");
            var modelContext = _context.UserHalls.Include(u => u.Category).Include(u => u.Hall).Include(u => u.User);
            return View(await modelContext.ToListAsync());
        }

        // GET: UserHalls/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userHall = await _context.UserHalls
                .Include(u => u.Category)
                .Include(u => u.Hall)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userHall == null)
            {
                return NotFound();
            }

            return View(userHall);
        }

        // GET: UserHalls/Create
        public IActionResult Create()
        {
            ViewBag.adminname = HttpContext.Session.GetString("Adminname");
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName");
            return View();
        }

        // POST: UserHalls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartDate,EndDate,HallId,UserId,CategoryId")] UserHall userHall)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userHall);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", userHall.CategoryId);
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Id", userHall.HallId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userHall.UserId);
            return View(userHall);
        }

        // GET: UserHalls/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userHall = await _context.UserHalls.FindAsync(id);
            if (userHall == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", userHall.CategoryId);
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Name", userHall.HallId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FirstName", userHall.UserId);
            return View(userHall);
        }

        // POST: UserHalls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Id,StartDate,EndDate,HallId,UserId,CategoryId")] UserHall userHall)
        {
            if (id != userHall.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userHall);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserHallExists(userHall.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", userHall.CategoryId);
            ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Id", userHall.HallId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userHall.UserId);
            return View(userHall);
        }

        // GET: UserHalls/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userHall = await _context.UserHalls
                .Include(u => u.Category)
                .Include(u => u.Hall)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userHall == null)
            {
                return NotFound();
            }

            return View(userHall);
        }

        // POST: UserHalls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var userHall = await _context.UserHalls.FindAsync(id);
            _context.UserHalls.Remove(userHall);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserHallExists(decimal id)
        {
            return _context.UserHalls.Any(e => e.Id == id);
        }
       
       
        public async Task<IActionResult> ApproveUserHall(decimal id)
        {
            var userHall= await _context.UserHalls.FindAsync(id);
            if (id != userHall.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    userHall.IsApproved = true;
                    _context.Update(userHall);
                    await _context.SaveChangesAsync();
                    await SendPasswrodLinkEmail(userHall.UserId, "Approved");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserHallExists(userHall.Id))
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

            return RedirectToAction(nameof(Index));
        }
        
       
        public async Task<IActionResult> RejectUserHall(decimal id)
        {
            var userHall = await _context.UserHalls.FindAsync(id);

            if (id != userHall.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    userHall.IsApproved = false;
                    _context.Update(userHall);
                    await _context.SaveChangesAsync();
                    await SendRejectEmail(userHall.UserId, "Reject");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserHallExists(userHall.Id))
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

            return RedirectToAction(nameof(Index));
        }
        [NonAction]
        public async Task SendPasswrodLinkEmail(decimal? id,string status)
        {
            var userPass =await _context.Users.FindAsync(id);


            MimeMessage message = new MimeMessage();
            MailboxAddress from = new MailboxAddress("Hall Booking System", "201810352@std-zuj.edu.jo");
            message.From.Add(from);
            MailboxAddress to = new MailboxAddress("User", "mohammadalsadawi079@gmail.com");
            message.To.Add(to);
            message.Subject = "Status of Reservation";
            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody =
            "<p style=\"color:pink\">Reservation Accept</p>" + "Regards" + "<p>Thank You</p>";
            message.Body = bodyBuilder.ToMessageBody();
            using (var clinte = new SmtpClient())
            {
                clinte.Connect("smtp.office365.com",587, SecureSocketOptions.StartTls);
                clinte.Authenticate("201810352@std-zuj.edu.jo", "mohammad.sadawi98");
                clinte.Send(message);
                clinte.Disconnect(true);
            }

        }
        public async Task SendRejectEmail(decimal? id, string status)
        {
            var userPass = await _context.Users.FindAsync(id);


            MimeMessage message = new MimeMessage();
            MailboxAddress from = new MailboxAddress("Hall Booking System", "201810352@std-zuj.edu.jo");
            message.From.Add(from);
            MailboxAddress to = new MailboxAddress("User", "mohammadalsadawi079@gmail.com");
            message.To.Add(to);
            message.Subject = "Status of Reservation";
            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody =
            "<p style=\"color:pink\">Reservation Reject </p>" + "Sorry" + "<p>Thank You try again </p>";
            message.Body = bodyBuilder.ToMessageBody();
            using (var clinte = new SmtpClient())
            {
                clinte.Connect("smtp.office365.com", 587, SecureSocketOptions.StartTls);
                clinte.Authenticate("201810352@std-zuj.edu.jo", "mohammad.sadawi98");
                clinte.Send(message);
                clinte.Disconnect(true);
            }

        }
    }
}
