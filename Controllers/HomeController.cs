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
    public class HomeController : Controller
    {
        private HomeContext dbContext;
        public HomeController(HomeContext context)
        {
            dbContext = context;
        }
        public IActionResult Index()
        {
            return View();
        }

    [HttpGet("login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("register")]
    public IActionResult Register(User register)
    {
        if(ModelState.IsValid)
        {
            if(dbContext.Users.Any( u => u.Email == register.Email))
            {
                ModelState.AddModelError("Email", "That email already exists.");
                return View("Index");
            }
            else
            {
                PasswordHasher<User> hash = new PasswordHasher<User>();
                register.Password = hash.HashPassword(register, register.Password);

                dbContext.Users.Add(register);
                dbContext.SaveChanges();
                return RedirectToAction("Login");
            }
        }
        else
        {
            return View("Index");
        }
    }

    [HttpPost("signin")]
    public IActionResult SignIn(LoginUser log)
    {
        if(ModelState.IsValid)
        {
            User check = dbContext.Users.FirstOrDefault(u => u.Email ==log.LoginEmail);
            if(check == null)
            {
                ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                return View("Login");
            }
            else
            {
                PasswordHasher<LoginUser> compare = new PasswordHasher<LoginUser>();
                var result = compare.VerifyHashedPassword(log,check.Password,log.LoginPassword);
                if(result == 0)
                {
                ModelState.AddModelError("LoginEmail", "Invalid Email/Password");
                return View("Login");
                }
                else
                {
                HttpContext.Session.SetInt32("UserId", check.UserId);
                return RedirectToAction("Dashboard", "Venture");
                }
            }
        }
        else
        {
            return View("Login");
        }
    }





// /////////////////////////////This will be erased and in line 81 i will change Success DONE



    [HttpGet("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }

    private User LoggedIn()
    {
        User LogIn = dbContext.Users.Include( u => u.PlannedVent).FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));

        return LogIn;
    }












////////////////////////////////////
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
