using FirstProjectHallBooking.Models;
using FirstProjectHallBooking.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProjectHallBooking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        [BindProperty]
        public HomeVM homeVM { get; set; }

        public HomeController(ILogger<HomeController> logger , ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            ViewBag.userid = HttpContext.Session.GetInt32("ClientId");
            ViewBag.UserName= HttpContext.Session.GetString("Clientname");
            homeVM = new HomeVM
            {
                halls = _context.Halls.Include(x => x.Address).ToList(),
                home = _context.Homes.FirstOrDefault(),
                UserHall = new UserHall(),
                Categories = _context.Categories.ToList(),
                visas = _context.Visas.ToList(),
                tests =new Testimonial()

            };
            //var home = _context.Homes.ToList().FirstOrDefault();
            //ViewBag.Home = home.Image1;
            //ViewBag.desc=home.Description;
            //var address = _context.Addresses.ToList();
            //var hall = _context.Halls.Include(x=>x.Address).ToList();
            return View(homeVM);
        }
        public IActionResult StartIndex()
        {
            //ViewBag.userid = HttpContext.Session.GetInt32("ClientId");
            //ViewBag.UserName = HttpContext.Session.GetString("Clientname");
            homeVM = new HomeVM
            {
                halls = _context.Halls.Include(x => x.Address).ToList(),
                home = _context.Homes.FirstOrDefault(),
                UserHall = new UserHall(),
                Categories = _context.Categories.ToList(),
                visas = _context.Visas.ToList(),
                testimonials = _context.Testimonials.Include(x=>x.User).Where(x=>x.State==true).ToList()
                

            };
            //var home = _context.Homes.ToList().FirstOrDefault();
            //ViewBag.Home = home.Image1;
            //ViewBag.desc=home.Description;
            //var address = _context.Addresses.ToList();
            //var hall = _context.Halls.Include(x=>x.Address).ToList();
            return View(homeVM);
        }
        public IActionResult Contact()
        {
            ViewBag.userid = HttpContext.Session.GetInt32("ClientId");
            ViewBag.UserName = HttpContext.Session.GetString("Clientname");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact([Bind("Id,Name,Subject,Message,Email")] Contactu contactu)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contactu);
        }
        public IActionResult AboutUs()
        {
            ViewBag.userid = HttpContext.Session.GetInt32("ClientId");
            ViewBag.UserName = HttpContext.Session.GetString("Clientname");
            homeVM = new HomeVM
            {
                halls = _context.Halls.Include(x => x.Address).ToList(),
                home = _context.Homes.FirstOrDefault(),
                Aboutu=_context.Aboutus.FirstOrDefault()
                //users = _context.UserHalls.Include(x => x.User).Include(x => x.Hall).ToList()

            };
            return View(homeVM);
        }
        public IActionResult Search()
        {
            ViewBag.userid = HttpContext.Session.GetInt32("ClientId");
            ViewBag.UserName = HttpContext.Session.GetString("Clientname");
            homeVM = new HomeVM
            {
                halls = _context.Halls.Include(x => x.Address).ToList(),
                home = _context.Homes.FirstOrDefault(),
                UserHall = new UserHall(),
                Categories = _context.Categories.ToList(),
                visas = _context.Visas.ToList(),
                tests = new Testimonial()
            };
            return View(homeVM);
        }
        [HttpPost]
        public IActionResult Search(string name,string address)
        {
            ViewBag.userid = HttpContext.Session.GetInt32("ClientId");
            ViewBag.UserName = HttpContext.Session.GetString("Clientname");

            if (name == null && address == null)
            {
                homeVM = new HomeVM
                {
                    halls = _context.Halls.Include(x => x.Address).ToList(),
                    home = _context.Homes.FirstOrDefault(),
                    UserHall = new UserHall(),
                    Categories = _context.Categories.ToList(),
                    visas = _context.Visas.ToList(),
                    tests = new Testimonial()
                };
                return View(homeVM);
            }
            else if (name != null && address == null)
            {
                homeVM = new HomeVM
                {
                    halls = _context.Halls.Where(x => x.Name == name).Include(x => x.Address).ToList(),
                    home = _context.Homes.FirstOrDefault(),
                    UserHall = new UserHall(),
                    Categories = _context.Categories.ToList(),
                    visas = _context.Visas.ToList(),
                    tests = new Testimonial()
                };
                
                return View(homeVM);
            }
            else if (name == null && address != null)
            {
                homeVM = new HomeVM
                {
                    halls = _context.Halls.Where(x => x.Address.City == address).Include(x => x.Address).ToList(),
                    home = _context.Homes.FirstOrDefault(),
                    UserHall = new UserHall(),
                    Categories = _context.Categories.ToList(),
                    visas = _context.Visas.ToList(),
                    tests = new Testimonial()
                };
                
                return View(homeVM);
            }
            else
            {
                homeVM = new HomeVM
                {
                    halls = _context.Halls.Where(x => x.Address.City == address && x.Name == name).Include(x => x.Address).ToList(),
                    home = _context.Homes.FirstOrDefault(),
                    UserHall = new UserHall(),
                    Categories = _context.Categories.ToList(),
                    visas = _context.Visas.ToList(),
                    tests = new Testimonial()
                };
               
                return View(homeVM);

            }



        }
        public async Task<IActionResult> AddTestimonials([Bind("Id,Feedback,UserId")] Testimonial tests)
        {
            ViewBag.adminname = HttpContext.Session.GetString("Adminname");
            if (ModelState.IsValid)
            {
                _context.Add(tests);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tests);   
        }


            public async Task<IActionResult> Reservation([Bind("Id,StartDate,EndDate,HallId,UserId,CategoryId")] UserHall userHall)
        {
            if (ModelState.IsValid)
            {
                userHall.UserId = HttpContext.Session.GetInt32("ClientId");
                _context.Add(userHall);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", userHall.CategoryId);
            //ViewData["HallId"] = new SelectList(_context.Halls, "Id", "Id", userHall.HallId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userHall.UserId);
            return View(userHall);
        }
        public IActionResult Test()
        {
            return View();

        }
        public async Task<IActionResult> Profile(decimal? id)
        {
            ViewBag.UserName = HttpContext.Session.GetString("Clientname");
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(decimal id, [Bind("Id,FirstName,LastName,Email,Phone,Address,ImagePath,ImageFile")] User user)
        {
            ViewBag.UserName = HttpContext.Session.GetString("Clientname");
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (user.ImageFile != null)
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;
                        string fileName = Guid.NewGuid().ToString() + "_" + user.ImageFile.FileName;
                        string path = Path.Combine(wwwRootPath + "/Image/", fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await user.ImageFile.CopyToAsync(fileStream);
                        }
                        user.ImagePath = fileName;
                    }


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
                return RedirectToAction(nameof(Profile));
            }
            return View(user);
        }
        private bool UserExists(decimal id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
        public IActionResult Visa()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Visa(decimal cardNumber, decimal balnce , decimal cvv)
        {

            var visa = _context.Visas.Where(x => x.CardNumber == cardNumber && x.Balnce == balnce && x.Cvv==cvv).FirstOrDefault();
            if (visa != null)
            {
                ModelState.AddModelError("", "card successful");
                return View();
            }

            ModelState.AddModelError("", "sorry check your card ");
            return View();


        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
