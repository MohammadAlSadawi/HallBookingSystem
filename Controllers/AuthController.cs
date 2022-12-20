using FirstProjectHallBooking.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;

namespace FirstProjectHallBooking.Controllers
{
    public class AuthController : Controller
    {
        private readonly ModelContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AuthController(ModelContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register([Bind("Id,FirstName,LastName,Email,Phone,Address,ImagePath,ImageFile")] User user, string userName, string password)
        {
            if (ModelState.IsValid)
            {
                if (user.ImageFile != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Guid.NewGuid().ToString() + "_" + user.ImageFile.FileName; 
                    string path = Path.Combine(wwwRootPath + "/Image/", fileName); 

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        user.ImageFile.CopyTo(fileStream);
                    }
                    user.ImagePath = fileName;
                }
                _context.Add(user);
                _context.SaveChangesAsync();
                UsersLogin usersLogin = new UsersLogin();
                usersLogin.UserName = userName;
                usersLogin.Password = password;
                usersLogin.RoleId= 2;
                usersLogin.UserId = user.Id;
                _context.Add(usersLogin);
                _context.SaveChangesAsync();
                return RedirectToAction(nameof(Register));
            }
            return View(user);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login([Bind("UserName", "Password")] UsersLogin usersLogin)
        {
            var user = _context.UsersLogins.Include(x=>x.User).Where(x => x.UserName == usersLogin.UserName && x.Password == usersLogin.Password).SingleOrDefault();
            if (user != null)
            {
                switch (user.RoleId)
                {
                    case 1:
                        //HttpContext.Session.SetInt32("AdminId", (int)user.UserId);
                        HttpContext.Session.SetInt32("AdminId", (int)user.UserId);
                        HttpContext.Session.SetString("Adminname", user.User.FirstName +" "+ user.User.LastName);
                        return RedirectToAction("Index", "Admin");

                    case 2:
                        HttpContext.Session.SetInt32("ClientId", (int)user.UserId);
                        HttpContext.Session.SetString("Clientname", user.User.FirstName + " " + user.User.LastName);
                        return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "incorrect user name or password");
            return View();
        }
    }
}
