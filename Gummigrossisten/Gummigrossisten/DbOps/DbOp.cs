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
            var users = db.user.Where(x => x.username == username1 && x.password == password).ToList();
            if (users.Count == 1)
            {
                foreach (var item in users)
                {
                    user = item;
                }
                return user;
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
        public List<tire> GetAlltires(string search)
        {
            if(search != "")
            {
                var tireslist = db.tire.Where(x => x.dimension.Contains(search)).ToList();
                List<tire> SortedList = tireslist.OrderByDescending(x => x.balance).ToList();
                return SortedList;
            }
            else
            {
                var tires = db.tire.ToList();
                List<tire> SortedList = tires.OrderByDescending(x => x.balance).ToList();

                return SortedList;

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
    }
}