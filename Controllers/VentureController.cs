using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ExamOne.Models;
using Microsoft.AspNetCore.Identity;
using ExamOne.Contexts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace ExamOne.Controllers
{
    [Route("venture")]
    public class VentureController : Controller
    {
        private HomeContext dbContext;
        public VentureController(HomeContext context)
        {
            dbContext = context; 
        }

        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            User userInDb = LoggedIn();
            if(userInDb == null)
            {
                return RedirectToAction ("LogOut", "Home");
            }
            List<Venture> AllVentures = dbContext.Ventures.Include( v => v.GuestList).ThenInclude( r => r.Guest).Include( v => v.Planner).OrderBy ( v => v.Date).ToList();
            ViewBag.User = userInDb;
            return View(AllVentures);
        }

        ////////////////////////////

        [Route("new/venture")]
        public IActionResult NewVenture()
        {
            User userInDb = LoggedIn();
            if(userInDb == null)
            {
                return RedirectToAction("LogOut", "Home");
            }
            ViewBag.User = userInDb;
            return View();
        }

        ////////////////////////////////////

        [Route("create/venture")]
        public IActionResult CreateVenture(Venture newVent)
        {
            User userInDb = LoggedIn();
            if(userInDb == null)
            {
                return RedirectToAction("LogOut", "Home");
            }
            if(ModelState.IsValid)
            {
                dbContext.Ventures.Add(newVent);
                dbContext.SaveChanges();
                return Redirect($"/venture/{newVent.VentureId}");
            }
            else
            {
                ViewBag.User = userInDb;
                return View("NewVenture");
            }
        }

        //////////////////////////////////


        [HttpGet("{ventureId}")]
        public IActionResult ShowVenture(int ventureId)
        {
            User userInDb = LoggedIn();
            if(userInDb == null)
            {
                return RedirectToAction("LogOut", "Home");
            }
            Venture show = dbContext.Ventures.Include( v => v.Planner).Include ( v => v.GuestList ).ThenInclude( r => r.Guest ).FirstOrDefault( v => v.VentureId == ventureId);
            ViewBag.User = userInDb;
            return View(show);
        }

        //////////////////////////////////

        [HttpGet("delete/{ventureId}")]
        public IActionResult DeleteVenture(int ventureId)
        {
            User userInDb = LoggedIn();
            if(userInDb == null)
            {
                return RedirectToAction("LogOut", "Home");
            }
            Venture remove = dbContext.Ventures.FirstOrDefault( v => v.VentureId == ventureId);
            dbContext.Ventures.Remove(remove);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        ///////////////////////////////////

        [HttpGet("response/{ventureId}/{userId}/{status}")]
        public IActionResult Rsvp (int ventureId, int userId, string status)
        {
            User userInDb = LoggedIn();
            if(userInDb == null)
            {
                return RedirectToAction("LogOut", "Home");
            }
            if(status == "rsvp")
            {
                Rsvp going = new Rsvp();
                going.UserId = userId;
                going.VentureId = ventureId;
                dbContext.Rsvps.Add(going);
                dbContext.SaveChanges();
            }
            else if(status == "backout")
            {
                Rsvp backout = dbContext.Rsvps.FirstOrDefault( r => r.VentureId == ventureId && r.UserId == userId);
                dbContext.Rsvps.Remove(backout);
                dbContext.SaveChanges();
            }
            else
            {
                return RedirectToAction("Dashboard");
            }
            return RedirectToAction("Dashboard");
        }














        private User LoggedIn()
    {
        User LogIn = dbContext.Users.Include( u => u.PlannedVent).FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));

        return LogIn;
    }
    }
}