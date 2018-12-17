using Microsoft.EntityFrameworkCore;
using LogReg.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
// Other usings

public class UserController : Controller
{
    private UserContext dbContext;
 
    public UserController(UserContext context)
    {
        dbContext = context;
    }
 // -----------------------------------------Reg PAGE------------------------------------
    [HttpGet]
    [Route("")]
    public IActionResult Index()
    {

        
        return View();
    }

//------------------------------------------LOG PAGE --------------------------------------
    [HttpGet]
    [Route("login")]
    public IActionResult Log()
    {

        
        return View();
    }


//---------------------------------LOG OUT -----------------------------------------------------
    [HttpGet]
    [Route("delete")]
    public IActionResult Delete()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Log");
    }



//-----------------------------------------LOGIN LOGIC ------------------------------------------
    [HttpPost]
    [Route("login")]
    public IActionResult Login(LoginUser userSubmission)
    {
        if(ModelState.IsValid)
        {
            // If inital ModelState is valid, query for a user with provided email
            var userInDb = dbContext.Users.FirstOrDefault(u => u.Email == userSubmission.Email);
            // If no user exists with provided email
            if(userInDb == null)
            {
                // Add an error to ModelState and return to View!
                ModelState.AddModelError("Password", "Invalid Email/Password");
                return View("log");
            }            
            // Initialize hasher object
            var hasher = new PasswordHasher<LoginUser>();            
            // varify provided password against hash stored in db
            var result = hasher.VerifyHashedPassword(userSubmission, userInDb.Password, userSubmission.Password);            
            // result can be compared to 0 for failure
            if(result == 0)
            {
                // handle failure (this should be similar to how "existing email" is handled)
                ModelState.AddModelError("Password", "No User found!!!");                    
                return View("log");
            }
        var userInfo = dbContext.Users.FirstOrDefault(u => u.Email == userSubmission.Email);

        HttpContext.Session.SetInt32("UserId", userInfo.UserId);
        HttpContext.Session.SetString("UserName", userInfo.FirstName);
        
        return Redirect("dashboard");
        }
        else{
            return View("log");
        }
    }

//-----------------------------------REGISTRATION -------------------------------------------
    [HttpPost]
    [Route("register")]
    public IActionResult Register(User user)
    {
         if(ModelState.IsValid)
            {
                // If a User exists with provided email
                if(dbContext.Users.Any(u => u.Email == user.Email))
                {
                    // Manually add a ModelState error to the Email field, with provided
                    // error message
                    ModelState.AddModelError("Email", "Email already in use!");
                    
                    return View("Index");
                }
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                user.Password = Hasher.HashPassword(user, user.Password);
                dbContext.Add(user);
                dbContext.SaveChanges();
                return RedirectToAction("Log");

            }
            else
            {
                return View("Index");
            }
    }

}