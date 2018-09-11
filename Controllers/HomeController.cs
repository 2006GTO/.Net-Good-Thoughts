using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bright_idea.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Threading;


namespace bright_idea.Controllers
{
    public class HomeController : Controller
    {

        private YourContext _context;
    public HomeController(YourContext context)
    {
        _context = context;
    }
        public IActionResult Index()
        {
            return View();
        }


    [HttpPost]
    public IActionResult Create(ViewModel FormData)
    {
        
        System.Console.WriteLine("got over here");
        User user = FormData.regUser;
        System.Console.WriteLine("got over user");
        if(FormData.regUser!= null){
            System.Console.WriteLine("got over here tooo");
        if(ModelState.IsValid){
        System.Console.WriteLine("model stat valid");
        PasswordHasher<User> Hasher = new PasswordHasher<User>();
        user.Password = Hasher.HashPassword(user, user.Password);
        System.Console.WriteLine("just before adding");
        System.Console.WriteLine(user);
        System.Console.WriteLine(user.Name);
        _context.Add(user);
        _context.SaveChanges();
        System.Console.WriteLine("***Created a User***");
        HttpContext.Session.SetInt32("id", user.Userid);
        HttpContext.Session.SetString("name", user.Name);
        HttpContext.Session.SetString("alias", user.Alias);
        return RedirectToAction("Ideas");
        }
        System.Console.WriteLine("null info coming thru");
        }
        System.Console.WriteLine("User creation rejected*******");        
        return View("Index", FormData);
    }

    [HttpPost]
    [Route("Login")]
    public IActionResult Login(ViewModel FormData){
        UserLog loguser = FormData.loginUser;
        if(ModelState.IsValid){
        var user = _context.users.SingleOrDefault(u => u.Email == loguser.LogEmail);
        if(user != null && user.Password != null)
        {
            var Hasher = new PasswordHasher<User>();
            if(0 != Hasher.VerifyHashedPassword(user, user.Password, loguser.LogPassword))
            {
                ViewBag.num = (int)user.Userid;
                _context.SaveChanges();
                HttpContext.Session.SetString("name", user.Name);
                System.Console.WriteLine("***Logging in a User***");
                HttpContext.Session.SetInt32("id", (int)ViewBag.num);
                return RedirectToAction("Ideas");}
        ModelState.AddModelError("LogPassword", "Incorrect password.");
        return View("Index");
        }
        ModelState.AddModelError("LogEmail", "Incorrect email.");
        System.Console.WriteLine("***User was denied login!!!***");
        return View("Index");
    }
            ModelState.AddModelError("Invald", "Invalid email or password.");

    return View("Index");}




    public IActionResult Ideas(ViewModel user)
    {
        if(HttpContext.Session.GetInt32("id") == null) { 
            return RedirectToAction("Logout");}
        ViewBag.alias = HttpContext.Session.GetString("alias");
        ViewBag.name = HttpContext.Session.GetString("name");
        ViewBag.id = HttpContext.Session.GetInt32("id");
        List<Idea> All = _context.ideas.Include(u => u.Likes).Include(g => g.Creator).ToList();
        ViewBag.ideas = All.OrderByDescending(x => x.Likes.Count);
        TempData["idea"] = "ideas";
        return View();
    }


    [HttpPost]
    public IActionResult New(ViewModel FormData)
    {

                if(HttpContext.Session.GetInt32("id") == null) { 
            return RedirectToAction("Logout");};
        Idea idea = new Idea();

        System.Console.WriteLine(FormData);
        System.Console.WriteLine(FormData.Idea.Text);

        // if(HttpContextSession.GetInt32("id") == null) { 
        //     return RedirectToAction("Logout");};
    if(ModelState.IsValid){
    User user = _context.users.SingleOrDefault(u => u.Userid== HttpContext.Session.GetInt32("id"));
    FormData.Idea.Creator = user;
    _context.Add(FormData.Idea);
    _context.SaveChanges();
    System.Console.WriteLine(FormData.Idea.Creator.Name + ": is our creator");
    System.Console.WriteLine("***Created an idea***");
    return RedirectToAction("Ideas");
    }
    System.Console.WriteLine("idea creation rejected*******");
    return View("Ideas");
    }

    [HttpGet]
    [Route("Home/Delete/{num}/{word}")]
    public IActionResult Delete(int num, string word)
    {
            if(HttpContext.Session.GetInt32("id") == null) { 
            return RedirectToAction("Logout");};

        Idea del = _context.ideas.SingleOrDefault(u => u.Ideaid == num);
        _context.Remove(del);
        _context.SaveChanges();
        if(word == "ideas"){
        return RedirectToAction("Ideas");
        }
        return RedirectToAction("Popular");
    }


    [HttpGet]
    [Route("Home/Like/{num}/{word}")]
    public IActionResult Like(int num, string word)
    {


        if(HttpContext.Session.GetInt32("id") == null) { 
            return RedirectToAction("Logout");};

        Idea it = _context.ideas.Include(g => g.Likes).ThenInclude(a => a.User).SingleOrDefault(u => u.Ideaid == num);
        User user = _context.users.SingleOrDefault(u => u.Userid == HttpContext.Session.GetInt32("id"));

        Like newl = new Like{
            Userid = user.Userid,
            User = user,
            Ideaid = it.Ideaid,
            Idea = it
        };

        it.Likes.Add(newl);
        _context.SaveChanges();
        if(word == "ideas"){
        return RedirectToAction("Ideas");
        }
        return RedirectToAction("Popular");
    }

    [HttpGet]
    [Route("Home/AllLiked/{num}")]
    public IActionResult AllLiked(int num)
    {
        if(HttpContext.Session.GetInt32("id") == null) { 
            return RedirectToAction("Logout");};

        ViewModel idear = new ViewModel()
        {
           Idea = _context.ideas.SingleOrDefault(u => u.Ideaid == num),
           allUsers = _context.users.ToList(),
           allLikes = _context.likes.Where(u => u.Ideaid == num).ToList(),
        };
        ViewBag.name = HttpContext.Session.GetString("name");
        ViewBag.id = HttpContext.Session.GetInt32("id");


        return View(idear);
    }

    [HttpGet]
    [Route("/UserShow/{num}")]
    public IActionResult UserShow(int num)
    {
        if(HttpContext.Session.GetInt32("id") == null) { 
            return RedirectToAction("Logout");}
    User user = _context.users.SingleOrDefault(x => x.Userid== num);

 int thisid = num;
        var AllLikes = _context.likes.Where(u => u.Userid == thisid);
        ViewBag.userlikes = AllLikes.Count();

        var AllPosts = _context.ideas.Where(u => u.Userid == thisid);
        ViewBag.userposts = AllPosts.Count();

        ViewBag.user = user;
        return View("Details"); 
    }



        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            System.Console.WriteLine("***User is logging out ***");
            return RedirectToAction("Index");
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
