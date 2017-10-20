using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using DojoActivity.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DojoActivity.Controllers
{
    
    public class ActivityController : Controller
    {
        private DojoActivityContext _context;
        public ActivityController(DojoActivityContext context)
        {
            _context = context;
        }
        // GET: /Home/
        [HttpGet]
        [Route("home")]
        public IActionResult Home()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if(UserId == null)
            {
                return RedirectToAction("Index", "UserManage");
            }
            List<Activity> AllActivities = new List<Activity>();
            AllActivities = _context.Activities.Where(a => a.Date > DateTime.Now).OrderBy(a => a.Date).Include(a => a.User).Include(a => a.Participants).ToList();
            ViewBag.Activities = AllActivities;
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.Username = HttpContext.Session.GetString("Username");
            
            return View("Home");
        }
        [HttpGet]
        [Route("new")]
        public IActionResult NewActivity()
        {
            int? UserId = HttpContext.Session.GetInt32("UserId");
            if(UserId == null)
            {
                return RedirectToAction("Index", "UserManage");
            }
            ViewBag.Errors = TempData["Errors"];
            return View("NewActivity");
        }
        [HttpPost]
        [Route("new")]
        public IActionResult CreateActivity(string Title, TimeSpan Time, DateTime Day, int Duration, string Unit, string Description)
        {
            if(ModelState.IsValid && Title != null && Time != null && Day != null && Duration != 0 && Description != null)
            {
                Activity NewActivity = new Activity();
                NewActivity.Title = Title;
                NewActivity.Date = Day + Time;
                NewActivity.UserId = (int)HttpContext.Session.GetInt32("UserId");
                NewActivity.Description = Description;
                NewActivity.Duration = Duration.ToString() + Unit;
                _context.Activities.Add(NewActivity);
                _context.SaveChanges();
                return RedirectToAction("Home");
            }
            TempData["Error"] = "Error!";
            return RedirectToAction("NewActivity");
        }
        [HttpGet]
        [Route("delete/{Id}")]
        public IActionResult Delete(int Id)
        {
            List<Participant> Goners = _context.Participants.Where(p => p.ActivityId == Id).ToList();
            foreach(var Goner in Goners)
            {
                _context.Participants.Remove(Goner);
            }
            Activity Cancel = _context.Activities.Where(a => a.ActivityId == Id).SingleOrDefault();
            _context.Activities.Remove(Cancel);
            _context.SaveChanges();
            return RedirectToAction("Home");
        }
        [HttpGet]
        [Route("participate/{Id}")]
        public IActionResult Participate(int Id)
        {
            int? UserId =
            HttpContext.Session.GetInt32("UserId");
            Participant NewParticipant = new Participant();
            NewParticipant.UserId = (int)UserId;
            NewParticipant.ActivityId = Id;
            _context.Participants.Add(NewParticipant);
            _context.SaveChanges();
            return RedirectToAction("Home");
        }
        [HttpGet]
        [Route("leave/{Id}")]
        public IActionResult Leave(int Id)
        {
            int? UserId =
            HttpContext.Session.GetInt32("UserId");
            Participant NoGo = _context.Participants.SingleOrDefault(g => (g.UserId == UserId && g.ActivityId == Id));
            _context.Participants.Remove(NoGo);
            _context.SaveChanges();
            return RedirectToAction("Account");
        }
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "UserManage");
        }
    }
}
