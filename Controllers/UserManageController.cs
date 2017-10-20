using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DojoActivity.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace DojoActivity.Controllers
{
    
    public class UserManageController : Controller
    {
        private DojoActivityContext _context;
        public UserManageController(DojoActivityContext context)
        {
            _context = context;
        }
        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("register")]
        public IActionResult Register(RegisterViewModel model)
        {
            User User = _context.Users.Where(u => u.Email == model.Email).SingleOrDefault();
            if (User != null){
                ModelState.AddModelError("Email", "Email is already in use");
                return View("Index");
            }
            else if (ModelState.IsValid){
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                User NewUser = new User();
                NewUser.Password = Hasher.HashPassword(NewUser, model.Password);
                NewUser.FirstName = model.FirstName;
                NewUser.LastName = model.LastName;
                NewUser.Email = model.Email;
                _context.Users.Add(NewUser);
                _context.SaveChanges();
                int UserId = _context.Users.Last()
                .UserId;
                HttpContext.Session.SetInt32("UserId", UserId);
                HttpContext.Session.SetString("Username", model.FirstName);
                return RedirectToAction("Home", "Activity");
            }
            return View("Index");
        }
        [HttpPost]
        [Route("login")]
        public IActionResult Login(LoginViewModel model)
        {
            if(ModelState.IsValid){
                User user = _context.Users.Where(u => u.Email == model.Email).SingleOrDefault();
                if (user != null && model.Password != null)
                {
                    var Hasher = new PasswordHasher<User>();
                    if( 0 != Hasher.VerifyHashedPassword(user, user.Password, model.Password))
                    {
                        HttpContext.Session.SetInt32("UserId", user.UserId);
                        HttpContext.Session.SetString("Username", user.FirstName);
                        return RedirectToAction("Home", "Activity");
                    }
                }
            }
            ViewBag.Errors = "Wrong Password or Email";
            return View("Index");
        }
        [HttpPost]
        [Route("logout")]
        public IActionResult Logout()
        {
            return View("Index");
        }
    }
}
