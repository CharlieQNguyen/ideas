using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using wedding_planner.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace wedding_planner.Controllers
{
    public class HomeController : Controller
    {
        private Context _context;

        public HomeController(Context context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("processingUser")]
        public IActionResult RegisterUser(User userReg, string confirm)
        {
            if(ModelState.IsValid)// check to see for validation
            {
                if(userReg.password == confirm)
                {
                    List<User> A_user = _context.Users.Where(user => user.email == userReg.email).ToList();//checking for duplicate email
                    int userCount = A_user.Count();
                    if(userCount == 0)
                    {
                        PasswordHasher<User> Hasher = new PasswordHasher<User>();
                        userReg.password = Hasher.HashPassword(userReg, userReg.password);
                        _context.Users.Add(userReg);
                        _context.SaveChanges();

                        int userId = _context.Users.SingleOrDefault(u => u.email == userReg.email).userId;
                        HttpContext.Session.SetInt32("Id", userId);// have id in session
                        HttpContext.Session.SetString("Users", userReg.name);
                        return RedirectToAction("Dashboard");
                    }
                }
            }
            return View("index");
        }
        [HttpGet]
        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            int? activeUser = HttpContext.Session.GetInt32("Id");//active userId
            ViewBag.activeUser = activeUser;

            string userP = HttpContext.Session.GetString("Users");//user name
            ViewBag.User = userP;


            var allIdeas = _context.Ideas.Include(i => i.user).Include(i => i.likes).ToList();// got the list of ideas in database
            ViewBag.allIdeas = allIdeas;
            return View("Dashboard");
        }
        [HttpPost]
        [Route("processingLogin")]
        public IActionResult LoginUser(string email, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.email == email);
            if(user !=null && password !=null)
            {
                var Hasher = new PasswordHasher<User>();
                if (0 != Hasher.VerifyHashedPassword(user, user.password, password))
                {
                    HttpContext.Session.SetString("Users", user.name); //var Users =_context.Users
                    HttpContext.Session.SetInt32("Id", user.userId);//had todo this for session or there will e null value
                    return RedirectToAction("Dashboard");
                }
            }
            return View("index");
        }/////////////end of login
        [HttpPost]
        [Route("processingPost")]
        public IActionResult LogPost(Idea newIdea)
        {
            if (ModelState.IsValid)
            {
                int? user = HttpContext.Session.GetInt32("Id"); // now have variable user as an int (userId)
                newIdea.userId = (int)user;
                _context.Ideas.Add(newIdea);
                _context.SaveChanges();

                return RedirectToAction("Dashboard");
            }
            else
            {
                return View("Dashboard");
            }
        }
        [HttpGet]
        [Route("like/{id}")]
        public IActionResult Like(int id)
        {
            int? activeUser = HttpContext.Session.GetInt32("Id");// grabed active user ID
            Like newLike = new Like();
            newLike.userId = (int)activeUser; //had to cast int because activeUser is int?
            newLike.ideasId = id;
            _context.Add(newLike);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [HttpGet]
        [Route("delete/{id}")]
        public IActionResult Delete(int id)
        {
            Idea thisIdea = _context.Ideas.SingleOrDefault(i => i.ideasId == id);
            _context.Ideas.Remove(thisIdea);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }
        [HttpGet]  
        [Route("user/{id}")]
        public IActionResult Viewuser(int id)
        {
            User thisUser = _context.Users.Include(u => u.likes).Include(u => u.ideas).SingleOrDefault(u => u.userId == id);// entire user object
            ViewBag.user = thisUser;
            return View();
        }
        [HttpGet]
        [Route("bright/{id}")]
        public IActionResult Viewidea(int id)
        {
            Idea thisIdea = _context.Ideas.Include(l => l.likes).ThenInclude(l => l.userObject).Include(i => i.user).SingleOrDefault(i => i.ideasId == id);// entire Id object that include likes then include list of likes also grabbing user id in Ideas table
            ViewBag.idea = thisIdea;

            return View("Viewidea");
        }

    }
}




// [HttpGet]
// [Route("idea/{id}")]
// public IActionResult idea(int id)
// {
//     if (HttpContext.Session.GetInt32("Id") == null)
//     {
//         return RedirectToAction("Index");
//     }
//     User currentUser = _context.Users.SingleOrDefault(u => u.userId == HttpContext.Session.GetInt32("Id"));
//     Like NewLike = new Like
//     {
//         userId = currentUser.userId,
//         ideasId = id,

//     };
//     _context.Likes.Add(NewLike);
//     _context.SaveChanges();
//     return RedirectToAction("Dashboard");
