using FirstProjectHallBooking.Models;
using FirstProjectHallBooking.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace FirstProjectHallBooking.Controllers
{
    public class StartController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        [BindProperty]
        public HomeVM homeVM { get; set; }
        public StartController(ILogger<HomeController> logger, ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult About()
        {
            homeVM = new HomeVM
            {
                halls = _context.Halls.Include(x => x.Address).ToList(),
                home = _context.Homes.FirstOrDefault(),
                Aboutu = _context.Aboutus.FirstOrDefault()
                //users = _context.UserHalls.Include(x => x.User).Include(x => x.Hall).ToList()

            };
            return View(homeVM);
        }
        public IActionResult Contact()
        {
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
                return RedirectToAction(nameof(Contact));
            }
            return View(contactu);
        }
    }
}
