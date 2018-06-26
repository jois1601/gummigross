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
            TempData["felinlogg"] = "";
            var username = form["username"];
            var password = form["password"];

            var user = dbc.LogIn(username, password);

            if(user == null)
            {
                TempData["felinlogg"] = "Felaktiga inloggningsuppgifter!";
                return View();
            }
            else
            {
                TempData["felinlogg"] = "";
                return RedirectToAction("Start", "Home", new { id = user.userID});
            }

        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Start(int id)
        {
            user theuser = new user();
            theuser = dbc.GetUser(id);
            string search = "";
            TempData["username"] = theuser.username;
            TempData["userID"] = theuser.userID;
            TempData["price"] = theuser.fk_access_id.ToString();
            List<tire> tirelist = dbc.GetAlltires(search);
            return View(tirelist);
        }
        [HttpPost]
        public ActionResult Start(FormCollection form)
        {
            //Innan vi listar ut däcken måste vi försöka spara vem som är inloggad på ett bra sätt!! Cookie?
            var search = form["search"];
           
            List<tire> tirelist = dbc.GetAlltires(search);
            return View(tirelist);
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