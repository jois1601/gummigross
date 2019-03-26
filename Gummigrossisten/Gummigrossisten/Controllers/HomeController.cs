using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.Entity.Validation;
using Gummigrossisten.DbOps;
using Gummigrossisten.Models;
using Gummigrossisten.Models.ViewModel;

namespace Gummigrossisten.Controllers
{
    public class HomeController : Controller
    {
        private DbOp dbc = new DbOp();

        public ActionResult Index()
        {
            HttpCookie Newcookie = Request.Cookies["User"];
            
            if (Newcookie != null)
            {
                Newcookie.Expires = DateTime.Now.AddHours(5);
                HttpContext.Response.SetCookie(Newcookie);
                user theuser = dbc.GetUser(Convert.ToInt32(Newcookie.Value));
               
                

                if (theuser.fk_access_id == 1)
                {
                    return RedirectToAction("AdminStart", "Home", new { id = theuser.userID });
                }
                else
                {
                    return RedirectToAction("Start", "Home", new { id = theuser.userID });
                }

            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        public ActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
        public ActionResult Error()
        {
            Response.StatusCode = 505;
            return View();
        }
        public ActionResult Login()
      {
            if (Request.Cookies["User"] != null)
            {
                var c = new HttpCookie("User");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            
            TempData["felinlogg"] = "";
            var username = form["username"];
            var password = form["password"];

            var user = dbc.LogIn(username, password);



            if (user == null)
            {
                TempData["felinlogg"] = "Felaktiga inloggningsuppgifter!";
                return View();
            }
            else if (user.fk_access_id == 2)
            {
                TempData["felinlogg"] = "";
                HttpCookie cookie = new HttpCookie("User");
                cookie.Expires = DateTime.Now.AddHours(5);
                cookie.Value = user.userID.ToString();
                HttpContext.Response.SetCookie(cookie);

                return RedirectToAction("Start", "Home", new { id = user.userID, search = "" });
            }
            else if (user.fk_access_id == 3)
            {
                TempData["felinlogg"] = "";

                HttpCookie cookie = new HttpCookie("User");
                cookie.Expires = DateTime.Now.AddHours(5);
                cookie.Value = user.userID.ToString();
                HttpContext.Response.SetCookie(cookie);
                return RedirectToAction("Start", "Home", new { id = user.userID, search = "" });
            }

            else if (user.fk_access_id == 5)
            {
                TempData["felinlogg"] = "";

                HttpCookie cookie = new HttpCookie("User");
                cookie.Expires = DateTime.Now.AddHours(5);
                cookie.Value = user.userID.ToString();
                HttpContext.Response.SetCookie(cookie);
                return RedirectToAction("Start", "Home", new { id = user.userID, search = "" });
            }

            else
            {
                TempData["felinlogg"] = "";
                HttpCookie cookie = new HttpCookie("User");
                cookie.Expires = DateTime.Now.AddHours(5);
                cookie.Value = user.userID.ToString();
                HttpContext.Response.SetCookie(cookie);
                return RedirectToAction("AdminStart", "Home", new { id = user.userID, search = "" });
            }

        }
        public ActionResult Start(int id, string search, string season)
        {
            user theuser = new user();
            theuser = dbc.GetUser(id);

            HttpCookie Newcookie = Request.Cookies["User"];

            if (Newcookie != null)
            {
                if(Newcookie.Value == id.ToString())
                {
                    Newcookie.Value = theuser.userID.ToString();
                    Newcookie.Expires = DateTime.Now.AddHours(5);
                    HttpContext.Response.SetCookie(Newcookie);

            
                    TempData["username"] = theuser.username;
                    TempData["userID"] = theuser.userID;
                    TempData["price"] = theuser.fk_access_id.ToString();

                    VMAdmin vmad = new VMAdmin();

                    List<tire> tirelist = dbc.GetAlltires(search, season);


                    List<tire> tirelists = dbc.GetAlltires(search, season);
                    List<user> UserList = dbc.GetAllUsers();
                    news thenews = dbc.GetTheNews(1);
                    vmad.TireList = tirelist;
                    vmad.UserList = UserList;
                    vmad.News = thenews;

                    return View(vmad);

                }
                else
                {

                    return RedirectToAction("Index", "Home");
                }
                

            }
            else
            {
                return RedirectToAction("Index","Home");
            }
            
            
        }
        [HttpPost]
        public ActionResult Start(FormCollection form)
        {
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

            VMAdmin vmad = new VMAdmin();
            user theuser = new user();
            theuser = dbc.GetUser(id);

            HttpCookie Newcookie = Request.Cookies["User"];

            if (Newcookie != null)
            {
                if (Newcookie.Value == id.ToString())
                {
                    Newcookie.Value = theuser.userID.ToString();
                    Newcookie.Expires = DateTime.Now.AddHours(5);
                    HttpContext.Response.SetCookie(Newcookie);

                    

                    TempData["username"] = theuser.username;
                    TempData["userID"] = theuser.userID;
                    TempData["price"] = theuser.fk_access_id.ToString();
                    List<tire> tirelist = dbc.GetAlltires(search, season);
                    List<user> UserList = dbc.GetAllUsers();
                    news thenews = dbc.GetTheNews(1);

                    //foreach (var u in UserList)
                    //{
                    //    vmad.UserList.Add(u);
                    //}
                    //foreach (var t in tirelist)
                    //{
                    //    vmad.TireList.Add(t);
                    //}
                    vmad.TireList = tirelist;
                    vmad.UserList = UserList;
                    vmad.News = thenews;
                    try
                    {
                        return View(vmad);

                    }
                    catch (Exception)
                    {

                        return View();
                    }
                    

                }
                else
                {

                    return RedirectToAction("Index", "Home");
                }


            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        [HttpPost]
        public ActionResult AdminStart(FormCollection form)
        {
            //Innan vi listar ut däcken måste vi försöka spara vem som är inloggad på ett bra sätt!! Cookie?
            HttpCookie Newcookie = Request.Cookies["User"];

            Newcookie.Expires = DateTime.Now.AddHours(5);

            user theuser = new user();
            theuser = dbc.GetUser(Convert.ToInt32(Newcookie.Value));
            TempData["user"] = theuser.userID;
            var searcha = form["searchadmin"];
            var season = form["season"];

            var newsbutton = form["newsbutton"];
            var newstext = form["newstext"];
            var newstitle = form["newstitle"];
            var deletetire2 = form["deletetire2"];
            var deletetire = form["deletetire"];
            var deleteuser = form["deleteuser"];

            if (deletetire2 != null && deleteuser == null)
            {
                //ta bort däck
                dbc.DeleteTire(Convert.ToInt32(deletetire2));
            }
            if (deleteuser != null && deletetire == null)
            {
                //ta bort user
                dbc.DeleteUser(Convert.ToInt32(deleteuser));
            }
            if(newsbutton != null)
            {
                //ändra nyheterna
                news nyheter = dbc.GetTheNews(Convert.ToInt32(newsbutton));
                nyheter.title = newstitle;
                nyheter.text = newstext;
                dbc.UpdateNews(nyheter);
                
            }
            return RedirectToAction("AdminStart", "Home", new { id = theuser.userID, search = searcha, season = season });
        }

        public ActionResult EditTire(int id)
        {
            HttpCookie Newcookie = Request.Cookies["User"];
            
            if(Newcookie.Value == "1" || Newcookie.Value == "4")
            {
                Newcookie.Expires = DateTime.Now.AddHours(5);
                user theuser = new user();
                theuser = dbc.GetUser(Convert.ToInt32(Newcookie.Value));
                TempData["user"] = theuser.userID;

                tire theTire = dbc.GetATire(id);
                return View(theTire);

            }
            else
            {
                return RedirectToAction("Login","Home");
            }
             
        }
        [HttpPost]
        public ActionResult EditTire(FormCollection form)
        {
            var tireID = form["tireID"];
            var brand = form["brand"].ToUpperInvariant();
            var dimension = form["dimension"].ToUpperInvariant();
            var type = form["type"].ToUpperInvariant();
            var season = form["season"].ToUpperInvariant();
            var tbc = form["tbc"].ToUpperInvariant();
            var rl = form["rl"].ToUpperInvariant();
            var bd = form["bd"].ToUpperInvariant();
            var sd = form["sd"];
            var inprice = form["inprice"];
            var outpriceA = form["outpriceA"];
            var outpriceB = form["outpriceB"];
            var balance = form["balance"];


            tire theTire = dbc.GetATire(Convert.ToInt32(tireID));
            theTire.brand = brand;
            theTire.dimension = dimension;
            theTire.type = type;
            theTire.season = season;
            theTire.tirebearingcapacity = tbc;
            theTire.rollingresistance = rl;
            theTire.breakingdistance = bd;
            theTire.sounddecibel = sd;
            theTire.inprice = Convert.ToDecimal(inprice);
            theTire.outpriceA = Convert.ToDecimal(outpriceA);
            theTire.outpriceB = Convert.ToDecimal(outpriceB);
            theTire.balance = Convert.ToInt32(balance);

            dbc.UpdateTire(theTire);


            TempData["UpdateTire"] = "Du har uppdaterat däcket!";
            HttpCookie Newcookie = Request.Cookies["User"];
            Newcookie.Expires = DateTime.Now.AddHours(5);
            user theuser = new user();
            theuser = dbc.GetUser(Convert.ToInt32(Newcookie.Value));
            TempData["user"] = theuser.userID;
            return View(theTire);
        }
        public ActionResult EditUser(int id)
        {
            HttpCookie Newcookie = Request.Cookies["User"];

            if (Newcookie.Value == "1" || Newcookie.Value == "4")
            {
                Newcookie.Expires = DateTime.Now.AddHours(5);
                user theuser = new user();
                theuser = dbc.GetUser(Convert.ToInt32(Newcookie.Value));
                TempData["user"] = theuser.userID;

                user aUser = dbc.GetUser(id);
                return View(aUser);

            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        [HttpPost]
        public ActionResult EditUser(FormCollection form)
        {
            var userID = form["userID"];
            var pricelist = form["pricelist"];


            user theuser1 = dbc.GetUser(Convert.ToInt32(userID));
            if(pricelist == "BILLIGA PRISLISTAN")
            {
                theuser1.fk_access_id = 2;
            }
            if (pricelist == "DYRA PRISLISTAN")
            {
                theuser1.fk_access_id = 3;
            }

            dbc.UpdateUser(theuser1);


            TempData["UpdateUser"] = "Du har uppdaterat användaren!";
            HttpCookie Newcookie = Request.Cookies["User"];
            Newcookie.Expires = DateTime.Now.AddHours(5);
            user theuser = new user();
            theuser = dbc.GetUser(Convert.ToInt32(Newcookie.Value));
            TempData["user"] = theuser.userID;
            return View(theuser1);
        }
        public ActionResult AdminSettings(int id)
        {
            HttpCookie Newcookie = Request.Cookies["User"];
            if(Newcookie != null)
            {
                if (Newcookie.Value == id.ToString())
                {
                    Newcookie.Expires = DateTime.Now.AddHours(5);
                    user user = new user();
                    user = dbc.GetUser(id);
                    TempData["user"] = user.userID;

                    return View(user);
                }
                else
                {
                    return RedirectToAction("login", "Home");
                }
            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }
        [HttpPost]
        public ActionResult AdminSettings(FormCollection form)
        {
            var ID = form["userid"];
            var userID = ID;
            user user = new user();
            user = dbc.GetUser(Convert.ToInt32(userID));
            TempData["user"] = user.userID;

            var username = form["username"];
            var currentpass = form["currentpassword"];
            var newpass = form["newpass"];
            var newpassagain = form["newpassagain"];

            if (user.password == currentpass && newpass == newpassagain)
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
        public ActionResult Settings(int id)
        {
            HttpCookie Newcookie = Request.Cookies["User"];
            if(Newcookie != null)
            {
                if(Newcookie.Value == id.ToString())
                {
                    Newcookie.Expires = DateTime.Now.AddHours(5);
                    user user = new user();
                    user = dbc.GetUser(id);
                    TempData["user"] = user.userID;

                    return View(user);

                }
                else
                {
                    return RedirectToAction("login", "Home");
                }

            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }
        [HttpPost]
        public ActionResult Settings(FormCollection form)
        {
            var ID = form["userid"];
            var userID = ID;
            user user = new user();
            user = dbc.GetUser(Convert.ToInt32(userID));
            TempData["user"] = user.userID;

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
        public ActionResult CreateTire()
        {
            HttpCookie Newcookie = Request.Cookies["User"];
            if (Newcookie.Value == "1" || Newcookie.Value =="4")
            {
                    Newcookie.Expires = DateTime.Now.AddHours(5);
                    user user = new user();
                    user = dbc.GetUser(Convert.ToInt32(Newcookie.Value));
                    TempData["user"] = user.userID;

                    return View(user);

            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }
        [HttpPost]
        public ActionResult CreateTire(FormCollection form)
        {
            var brand = form["brand"].ToUpperInvariant();
            var dimension = form["dimension"].ToUpperInvariant();
            var type = form["type"].ToUpperInvariant();
            var season = form["season"].ToUpperInvariant();
            var tbc = form["tbc"].ToUpperInvariant();
            var rl = form["rl"].ToUpperInvariant();
            var bd = form["bd"].ToUpperInvariant();
            var sd = form["sd"];
            var inprice = form["inprice"];
            var outpriceA = form["outpriceA"];
            var outpriceB = form["outpriceB"];
            var balance = form["balance"];


            tire theTire = new tire();

            theTire.brand = brand;
            theTire.dimension = dimension;
            theTire.type = type;
            theTire.season = season;
            theTire.tirebearingcapacity = tbc;
            theTire.rollingresistance = rl;
            theTire.breakingdistance = bd;
            theTire.sounddecibel = sd;
            theTire.inprice = Convert.ToDecimal(inprice);
            theTire.outpriceA = Convert.ToDecimal(outpriceA);
            theTire.outpriceB = Convert.ToDecimal(outpriceB);
            theTire.balance = Convert.ToInt32(balance);

            dbc.CreateTire(theTire);

            TempData["createdTire"] = "Du har nu lagt till ett nytt däck!";
            HttpCookie Newcookie = Request.Cookies["User"];
            Newcookie.Expires = DateTime.Now.AddHours(5);
            user theuser = new user();
            theuser = dbc.GetUser(Convert.ToInt32(Newcookie.Value));
            TempData["user"] = theuser.userID;
            return View();
        }
        public ActionResult CreateUser()
        {
            HttpCookie Newcookie = Request.Cookies["User"];
            if (Newcookie.Value == "1" || Newcookie.Value == "4")
            {
                Newcookie.Expires = DateTime.Now.AddHours(5);
                user user = new user();
                user = dbc.GetUser(Convert.ToInt32(Newcookie.Value));
                TempData["user"] = user.userID;

                return View(user);

            }
            else
            {
                return RedirectToAction("login", "Home");
            }
        }
        [HttpPost]
        public ActionResult CreateUser(FormCollection form)
        {
            var username = form["username"];
            var password = form["password"];
            var passwordagain = form["passwordagain"];
            var pricelist = form["pricelist"];

            if(password == passwordagain)
            {
                user theuser = new user();
                theuser.username = username;
                theuser.password = password;

                if(pricelist == "BILLIGA PRISLISTAN")
                {
                    theuser.fk_access_id = 2;
                }
                if(pricelist == "DYRA PRISLISTAN")
                {
                    theuser.fk_access_id = 3;
                }
                if (pricelist == "EX.MOMS PRISLISTAN")
                {
                    theuser.fk_access_id = 5;
                }

                dbc.CreateUser(theuser);
                TempData["createdUser"] = "Du har nu lagt till en ny användare!";
                HttpCookie Newcookie = Request.Cookies["User"];
                Newcookie.Expires = DateTime.Now.AddHours(5);
                user theusers = new user();
                theusers = dbc.GetUser(Convert.ToInt32(Newcookie.Value));
                TempData["user"] = theusers.userID;
                return View();

            }
            else
            {
                TempData["passwordmatch"] = "Lösenorden är inte likadana!";
                return View();
            }


        }

    }
}