using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Validation;
using Gummigrossisten.DbOps;
using Gummigrossisten.Models;

namespace Gummigrossisten.Controllers
{
    public class HomeController : Controller
    {
        private DbOp dbc = new DbOp();

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection form)
        {
            HttpCookie cookie = new HttpCookie("User");

            cookie.Expires = DateTime.Now.AddHours(5);
            HttpContext.Response.SetCookie(cookie);

            TempData["felinlogg"] = "";
            var username = form["username"];
            var password = form["password"];

            var user = dbc.LogIn(username, password);

            

            if (user == null)
            {
                TempData["felinlogg"] = "Felaktiga inloggningsuppgifter!";
                return View();
            }
            else if (user.fk_access_id== 2)
            {
                TempData["felinlogg"] = "";

                return RedirectToAction("Start", "Home", new { id = user.userID, search = "" });
            }
            else if (user.fk_access_id == 3)
            {
                TempData["felinlogg"] = "";
                return RedirectToAction("Start", "Home", new { id = user.userID, search = "" });
            }
            else
            {
                TempData["felinlogg"] = "";
                return RedirectToAction("AdminStart", "Home", new { id = user.userID, search = "" });
            }

        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Start(int id, string search, string season)
        {
            user theuser = new user();
            theuser = dbc.GetUser(id);

            HttpCookie Newcookie = Request.Cookies["User"];
            Newcookie.Expires = DateTime.Now.AddHours(5);
            Newcookie.Value = theuser.userID.ToString();
            HttpContext.Response.SetCookie(Newcookie);

            
                TempData["username"] = theuser.username;
                TempData["userID"] = theuser.userID;
                TempData["price"] = theuser.fk_access_id.ToString();
                List<tire> tirelist = dbc.GetAlltires(search, season);
                return View(tirelist);

            
            
        }
        [HttpPost]
        public ActionResult Start(FormCollection form)
        {
            //Innan vi listar ut däcken måste vi försöka spara vem som är inloggad på ett bra sätt!! Cookie?
            HttpCookie Newcookie = Request.Cookies["User"];
            Newcookie.Expires = DateTime.Now.AddHours(5);

            user theuser = new user();
            theuser = dbc.GetUser(Convert.ToInt32(Newcookie.Value));

            var searcha = form["search"];
            var season = form["season"];

            return RedirectToAction("Start", "Home", new { id = theuser.userID, search = searcha, season = season });
        }
        public ActionResult AdminStart(int id, string search, string season)
        {
            user theuser = new user();
            theuser = dbc.GetUser(id);

            HttpCookie Newcookie = Request.Cookies["User"];
            Newcookie.Expires = DateTime.Now.AddHours(5);
            Newcookie.Value = theuser.userID.ToString();
            HttpContext.Response.SetCookie(Newcookie);

            TempData["username"] = theuser.username;
            TempData["userID"] = theuser.userID;
            TempData["price"] = theuser.fk_access_id.ToString();
            List<tire> tirelist = dbc.GetAlltires(search, season);
            return View(tirelist);
        }
        [HttpPost]
        public ActionResult AdminStart(FormCollection form)
        {
            //Innan vi listar ut däcken måste vi försöka spara vem som är inloggad på ett bra sätt!! Cookie?
            HttpCookie Newcookie = Request.Cookies["User"];
            Newcookie.Expires = DateTime.Now.AddHours(5);

            user theuser = new user();
            theuser = dbc.GetUser(Convert.ToInt32(Newcookie.Value));

            var searcha = form["search"];
            var season = form["season"];

            return RedirectToAction("AdminStart", "Home", new { id = theuser.userID, search = searcha, season = season });
        }

        public ActionResult EditTire(int id)
        {
            tire theTire = dbc.GetATire(id);
            return View(theTire); 
        }
        [HttpPost]
        public ActionResult EditTire(FormCollection form)
        {
            var tireID = form["tireID"];

            tire theTire = dbc.GetATire(Convert.ToInt32(tireID));
            return View(theTire);
        }


        public ActionResult Settings(int id)
        {
            user user = new user();
            user = dbc.GetUser(id);

            return View(user);
        }
        [HttpPost]
        public ActionResult Settings(FormCollection form)
        {
            var ID = form["userid"];
            var userID = ID;
            user user = new user();
            user = dbc.GetUser(Convert.ToInt32(userID));
            
            var username = form["username"];
            var currentpass = form["currentpassword"];
            var newpass = form["newpass"];
            var newpassagain = form["newpassagain"];

            if(user.password == currentpass && newpass == newpassagain)
            {
                user.password = newpass;
                TempData["correct"] = "Du har nu bytt lösenord!";
                TempData["felsettings"] = "";

                dbc.UpdateUser(user);
                return View(user);
            }
            else
            {
                TempData["correct"] = "";
                TempData["felsettings"] = "Du har skrivit in fel uppgifter!";
                return View(user);

            }

        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}