using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gummigrossisten.Models;
using System.Data.Entity.Validation;
using System.Data.Entity;


namespace Gummigrossisten.DbOps
{
    public class DbOp
    {
        private gummi_grossisten_se_dbEntities db = new gummi_grossisten_se_dbEntities();



        public user LogIn(string username, string password)
        {
            user user = new user();
            var username1 = username.ToLower();
            var password1 = password;

            var users = db.user.Where(x => x.username == username1).ToList();
            if (users.Count == 1)
            {
                foreach (var item in users)
                {
                    user = item;
                }
                if(user.password == password1)
                {
                    return user;
                }
                else
                {
                    return null;
                }
                
            }
            else
            {
                return null;
            }
        }
        public user GetUser(int id)
        {
            user user = new user();
            try
            {
                user = db.user.Find(id);
                return user;

            }
            catch(Exception)
            {
                return null;
            }
        }
        public List<user> GetAllUsers()
        {
            List<user> UserList = db.user.ToList();
            return UserList;
        }
        public List<tire> GetAlltires(string search, string season)
        {
            if(search == "" || search == null)
            {

                if (season == "SOMMAR")
                {
                    var tires = db.tire.Where(x=>x.season == "SOMMAR").ToList();
                    List<tire> SortedList = tires.OrderByDescending(x => x.balance).ToList();

                    return SortedList;
                }
                else if (season == "VINTER")
                {
                    var tires = db.tire.Where(x => x.season == "VINTER").ToList();
                    List<tire> SortedList = tires.OrderByDescending(x => x.balance).ToList();

                    return SortedList;
                }
                else
                {
                    var tires = db.tire.ToList();
                    List<tire> SortedList = tires.OrderByDescending(x => x.balance).ToList();

                    return SortedList;
                }
            }
            else
            {
                if(season == "SOMMAR")
                {
                    var tireslist = db.tire.Where(x => x.dimension.Contains(search) && x.season == "SOMMAR" || x.brand.Contains(search) && x.season == "SOMMAR").ToList();
                    List<tire> SortedList = tireslist.OrderByDescending(x => x.balance).ToList();
                    return SortedList;
                }
                else if(season == "VINTER")
                {
                    var tireslist = db.tire.Where(x => x.dimension.Contains(search) && x.season == "VINTER" || x.brand.Contains(search) && x.season == "VINTER").ToList();
                    List<tire> SortedList = tireslist.OrderByDescending(x => x.balance).ToList();
                    return SortedList;
                }
                else
                {
                    var tireslist = db.tire.Where(x => x.dimension.Contains(search) || x.brand.Contains(search)).ToList();
                    List<tire> SortedList = tireslist.OrderByDescending(x => x.balance).ToList();
                    return SortedList;
                }

            }
            

        }
        public void UpdateUser(user user)
        {

            try
            {
                user theuser = db.user.Single(x => x.userID == user.userID);
                db.SaveChanges();
            }
            catch (Exception)
            {

            }


        }
        public tire GetATire(int id)
        {
            tire theTire = db.tire.Find(id);

            return theTire;
        }
        public void UpdateTire(tire thetire)
        {
            try
            {
                tire theNewTire = db.tire.Single(x => x.tireID == thetire.tireID);
                db.SaveChanges();
            }
            catch (Exception)
            {

            }
        }
        public void CreateTire(tire theTires)
        {
            try
            {
                tire theTire = theTires;
                db.tire.Add(theTire);
                db.SaveChanges();
            }
            catch (Exception)
            {

            }
        }
        public void CreateUser(user theUsers)
        {
            try
            {
                var isthereusers = db.user.Where(x => x.username == theUsers.username).ToList();

                if(isthereusers.Count == 0)
                {
                    user theuser = theUsers;
                    db.user.Add(theuser);
                    db.SaveChanges();
                }
                
            }
            catch (Exception)
            {

            }
        }
        public void DeleteTire(int id)
        {
            try
            {
                var tire = db.tire.Find(id);

                db.Entry(tire).State = EntityState.Deleted;
                db.SaveChanges();
            }
            catch (Exception)
            {

            }
        }
        public void DeleteUser(int id)
        {
            try
            {
                var user = db.user.Find(id);

                db.Entry(user).State = EntityState.Deleted;
                db.SaveChanges();
            }
            catch (Exception)
            {

            }
        }
        public news GetTheNews(int id)
        {
            news thenews = db.news.Find(id);
            return thenews;
        }
        public void UpdateNews(news thenews)
        {
            try
            {
                news nyheten = db.news.Single(x => x.newsID == thenews.newsID);
                db.SaveChanges();


            }
            catch (Exception)
            {

            }

        }
    }
}