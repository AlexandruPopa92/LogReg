using Microsoft.EntityFrameworkCore;
using LogReg.Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
// Other usings

public class WeddingController : Controller
{
    private UserContext dbContext;
 
    public WeddingController(UserContext context)
    {
        dbContext = context;
    }

    [HttpGet]
    [Route("dashboard")]
    public IActionResult Index(string msg)
    {
        if(HttpContext.Session.GetString("UserName") == null)
        {
            return Redirect("login");
        }
        List<Wedding> AllWeddings = dbContext.Weddings.Include(c=>c.Attendees).ThenInclude(a=>a.User).ToList();
        ViewBag.AllWeddings = AllWeddings;
        ViewBag.Id =HttpContext.Session.GetInt32("UserId");
        ViewBag.Name =HttpContext.Session.GetString("UserName");

        
        return View();
    }

// ++++++++++++++++++++++++++++++++++ ADD WEDDING PAGE +++++++++++++++++++++++++++++++++++++++++
    [HttpGet]
    [Route("new")]
    public IActionResult New()
    {
        if(HttpContext.Session.GetString("UserName") == null)
        {
            return Redirect("login");
        }
        ViewBag.Id =HttpContext.Session.GetInt32("UserId");
        return View("new");
    }

//----------------------------------ADD WEDDING TO DB ---------------------------------------------------------
    [HttpPost]
    [Route("createwedding")]
    public IActionResult CreateWedding(Wedding wedding)
    {   
        if(ModelState.IsValid){
            if(wedding.Date<DateTime.Now){
                ModelState.AddModelError("Date", "Wedding Date can not be in Past!!!");
                ViewBag.Id =HttpContext.Session.GetInt32("UserId");
                return View("new");
            }
            dbContext.Add(wedding);
            dbContext.SaveChanges();
            return Redirect($"create/{wedding.WeddingId}");
            }
            ViewBag.Id =HttpContext.Session.GetInt32("UserId");
            return View("new");
    }

//----------------------------------SHOW WEDDING PAGE ---------------------------------------------
    [HttpGet]
    [Route("create/{id}")]
    public IActionResult Show(int id)
    {
        var OneWedding = dbContext.Weddings.Include(a=>a.Attendees).ThenInclude(u=>u.User).Where(a=>a.WeddingId == id).ToList();


        return View(OneWedding);
    }

//-------------------------------- DELETE WEDDING -------------------------------------------------------
    [HttpGet]
    [Route("wedding/{id}")]
    public IActionResult DeleteWedding(int id)
    {
        var Wedding = dbContext.Weddings.Where(a=>a.WeddingId == id).FirstOrDefault();
        dbContext.Remove(Wedding);
        dbContext.SaveChanges();
        return RedirectToAction("Index");
    }


//----------------------------- ADD GUEST TO WEDDING --------------------------------------
    [HttpPost]
    [Route("guest")]
    public IActionResult AddGuest(Attendance attendance)
    {
        dbContext.Add(attendance);
        dbContext.SaveChanges();
        return RedirectToAction("Index");
    }


//----------------------------REMOVE GUEST FROM WEDDING ------------------------------------------
    [HttpGet]
    [Route("delete/{id}")]
    public IActionResult DeleteGuest(int id)
    {
        var OneAttendance = dbContext.Attendances.Where(a=>a.WeddingId ==id && a.UserId ==HttpContext.Session.GetInt32("UserId")).FirstOrDefault();
        dbContext.Remove(OneAttendance);
        dbContext.SaveChanges();
        return RedirectToAction("Index");
    }

}