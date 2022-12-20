using FirstProjectHallBooking.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProjectHallBooking.Controllers
{
    public class AdminController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AdminController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            ViewBag.users = _context.Users.Count();
            ViewBag.hall1=_context.Halls.Count();
            
            ViewBag.hall = _context.UserHalls.Count(x => x.IsApproved == true);
            ViewBag.adminId = HttpContext.Session.GetInt32("AdminId");
          
            ViewBag.adminname = HttpContext.Session.GetString("Adminname");
            var users = _context.Users.ToList();
            var userhall=_context.UserHalls.Include(x=>x.User).Include(h=>h.Hall).ToList();
            var halls=_context.Halls.Include(x=>x.Address).ToList();
            var address = _context.Addresses.Include(x => x.Halls).ToList();
            var modelContext = _context.UserHalls.Include(u => u.User).Include(p => p.Hall);
            var modelContext1= from uh in userhall
                              join u in users on uh.UserId equals u.Id
                              join h in halls on uh.HallId equals h.Id
                              join a in address on h.AddressId equals a.Id
                              select new Join { user = u, hall = h, userhall = uh, address = a };
            var result = Tuple.Create<IEnumerable<User>, IEnumerable<Hall>,IEnumerable<Join>>(users, halls,modelContext1);
            var hallname=_context.Halls.Select(x=> x.Name).ToList();
            List<int> count =new List<int>();
            foreach(var item in hallname)
            {
                count.Add(modelContext.Count(x => x.Hall.Name == item));
            }
            ViewBag.hname = hallname;
            ViewBag.count = count;
            return View(result);
        }
        [HttpPost]
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            ViewBag.users = _context.Users.Count();
            ViewBag.hall1 = _context.Halls.Count();
            ViewBag.hall = _context.UserHalls.Count(x => x.IsApproved == true);
            ViewBag.adminId = HttpContext.Session.GetInt32("AdminId");
            ViewBag.adminname = HttpContext.Session.GetString("Adminname");
            var users = _context.Users.ToList();
            var userhall = _context.UserHalls.Include(x => x.User).Include(h => h.Hall).ToList();
            var halls = _context.Halls.Include(x=>x.Address).ToList();
            var address = _context.Addresses.Include(x => x.Halls).ToList();
            var modelContext = _context.UserHalls.Include(u => u.User).Include(p => p.Hall);
            var modelContext1 = from uh in userhall
                                join u in users on uh.UserId equals u.Id
                                join h in halls on uh.HallId equals h.Id
                                join a in address on h.AddressId equals a.Id
                                select new Join { user = u, hall = h, userhall = uh, address = a };
            
            var hallname = _context.Halls.Select(x => x.Name).ToList();
            List<int> count = new List<int>();
            foreach (var item in hallname)
            {
                count.Add(modelContext.Count(x => x.Hall.Name == item));
            }
            ViewBag.hname = hallname;
            ViewBag.count = count;
            //var modelContext = _context.UserHalls.Include(u => u.User).Include(p => p.Hall);
           
            if (startDate == null && endDate == null)
            {
                var result = Tuple.Create<IEnumerable<User>, IEnumerable<Hall>, IEnumerable<Join>>(users, halls, modelContext1);

                return View(result);
            }
            else if(startDate != null && endDate == null)
            {
                var result = Tuple.Create<IEnumerable<User>, IEnumerable<Hall>, IEnumerable<Join>>(users, halls,  modelContext1.Where(x=>x.userhall.StartDate.Value.Date==startDate).ToList());
                return View(result);
            }
            else if(endDate != null && startDate == null)
            {
                var result = Tuple.Create<IEnumerable<User>, IEnumerable<Hall>, IEnumerable<Join>>(users, halls,  modelContext1.Where(x => x.userhall.EndDate.Value.Date == endDate).ToList());
                return View(result);

            }
            else
            {
                var result = Tuple.Create<IEnumerable<User>, IEnumerable<Hall>, IEnumerable<Join>>(users, halls,  modelContext1.Where(x => x.userhall.StartDate >= startDate &&x.userhall.EndDate<=endDate).ToList());
                return View(result);
            }
        }
        public async Task<IActionResult> Profile(decimal? id)
        {
            ViewBag.adminname = HttpContext.Session.GetString("Adminname");
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
            ViewBag.adminname = HttpContext.Session.GetString("Adminname");
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
        public IActionResult Join()
        {
            var user = _context.Users.ToList();
            var hall = _context.Halls.ToList();
            var userhall = _context.UserHalls.ToList();
            var categories = _context.Categories.ToList();

            var result = from uh in userhall
                         join u in user on uh.UserId equals u.Id
                         join h in hall on uh.HallId equals h.Id
                         join c in categories on uh.CategoryId equals c.Id
                         select new JoinTables { user = u, hall = h, userhall = uh, category = c };
            return View(result);
        }
        public IActionResult Report()
        {
            //int? id = HttpContext.Session.GetInt32("AdminId");
            //var users = _context.Users.Where(x => x.Id == id).FirstOrDefault();
            //ViewBag.user = users.FirstName + " " + users.LastName;
            ViewBag.adminname = HttpContext.Session.GetString("Adminname");
            ViewBag.name = _context.UserHalls.Count();
            ViewBag.nmhall = _context.Halls.Count();
            ViewBag.users = _context.Users.Count();

            var user = _context.Users.ToList();
            var hall = _context.Halls.ToList();
            var userhall = _context.UserHalls.ToList();
            var categories = _context.Categories.ToList();

            var result = from uh in userhall
                         join u in user on uh.UserId equals u.Id
                         join h in hall on uh.HallId equals h.Id
                         join c in categories on uh.CategoryId equals c.Id
                         select new JoinTables { user = u, hall = h, userhall = uh, category = c };
            var modelContext = _context.UserHalls.Include(u => u.User).Include(p => p.Hall);
            var model=Tuple.Create<IEnumerable <JoinTables>,IEnumerable<UserHall>>(result, modelContext);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Report(DateTime? startDate, DateTime? endDate)
        {
            //int? id = HttpContext.Session.GetInt32("AdminId");
            //var users = _context.Users.Where(x => x.Id == id).FirstOrDefault();
            //ViewBag.user = users.FirstName + " " + users.LastName;
            ViewBag.adminname = HttpContext.Session.GetString("Adminname");
            var user = _context.Users.ToList();
            var hall = _context.Halls.ToList();
            var userhall = _context.UserHalls.ToList();
            var categories = _context.Categories.ToList();

            var result = from uh in userhall
                         join u in user on uh.UserId equals u.Id
                         join h in hall on uh.HallId equals h.Id
                         join c in categories on uh.CategoryId equals c.Id
                         select new JoinTables { user = u, hall = h, userhall = uh, category = c };
            var modelContext = _context.UserHalls.Include(u => u.User).Include(p => p.Hall);

            if (startDate == null && endDate == null)
            {
                
                

                var model = Tuple.Create<IEnumerable<JoinTables>, IEnumerable<UserHall>>(result, modelContext);
                ViewBag.name = model.Item1.Count();
                ViewBag.nmhall = _context.Halls.Count();
                ViewBag.total = model.Item1.Sum(x => x.hall.Price);
                ViewBag.users = _context.Users.Count();

                return View(model);
            }
            else if (startDate != null && endDate == null)
            {
                //ViewBag.name = _context.UserHalls.Count();
               
                
                var model = Tuple.Create<IEnumerable<JoinTables>, IEnumerable<UserHall>>( result.Where(x => x.userhall.StartDate.Value.Month == startDate.Value.Month).ToList(), modelContext);
                ViewBag.name = model.Item1.Count();
                ViewBag.nmhall = _context.Halls.Count();
                ViewBag.total = model.Item1.Sum(x => x.hall.Price);
                ViewBag.users = _context.Users.Count();
                return View(model);
            }
            else if (endDate != null && startDate == null)
            {
                //ViewBag.name = _context.UserHalls.Count();
               
                var model = Tuple.Create<IEnumerable<JoinTables>, IEnumerable<UserHall>>(result.Where(x => x.userhall.EndDate.Value.Year == endDate.Value.Year).ToList(), modelContext);
                ViewBag.name = model.Item1.Count();
                ViewBag.nmhall = _context.Halls.Count();
                ViewBag.total = model.Item1.Sum(x => x.hall.Price);
                ViewBag.users = _context.Users.Count();
                return View(model);

            }
            else
            {
                //ViewBag.name = _context.UserHalls.Count();
               
                var model = Tuple.Create<IEnumerable<JoinTables>, IEnumerable<UserHall>>(result.Where(x => x.userhall.StartDate.Value.Month == startDate.Value.Month && x.userhall.EndDate.Value.Year==endDate.Value.Year).ToList(), modelContext);
                ViewBag.name = model.Item1.Count();
                ViewBag.nmhall = _context.Halls.Count();
                ViewBag.total = model.Item1.Sum(x => x.hall.Price);
                ViewBag.users = _context.Users.Count();

                return View(model); 

            }  
        }
       
       
        

    }

}
