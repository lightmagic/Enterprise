using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsMedia.Models;
using System.Data.SqlClient;

namespace NewsMedia.Controllers
{
    public class UsersController : Controller
    {
        //
        // GET: /Users/
        UsersContext db = new UsersContext();

        public string connectionString = "Data Source=KYLE\\SQLSERVER;Initial Catalog=Enterprise;Integrated Security=True";
        private SqlConnection connectToDB;

        private void Connection()
        {
            connectToDB = new SqlConnection(connectionString);
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Users()
        {
            var model = db.UserProfiles.ToList();
            return View(model);
        }

        public UserProfile GetUser(string userName)
        {
            var model = db.UserProfiles.ToList();
            return model.SingleOrDefault(x => x.UserName == userName);
        }

        //Delete user from DB http://www.completecsharptutorial.com/mvc-articles/insert-update-delete-in-asp-net-mvc-5-without-entity-framework/
        public ActionResult DeleteUser(string userName)
        {
            try
            {
                Connection();
                connectToDB.Open();
                UserProfile up = new UserProfile();
                up = GetUser(userName);
                int userId = up.UserId;
                SqlCommand cmd = new SqlCommand("DELETE FROM [dbo].[UserProfile] WHERE UserId = @userid", connectToDB);
                cmd.Parameters.AddWithValue("@userid", userId);
                cmd.ExecuteNonQuery();
                connectToDB.Close();
            }

            catch
            {
                TempData["msg"] = "Error. Could not delete the User";
            }

            return RedirectToAction("Users");
        }


        //Return view for the edit page//
        public ActionResult UserEdit(string userName)
        {
            UserProfile db = new UserProfile();
            db = GetUser(userName);
            return View(db); //the userprofile to edit
        }

        //Edit the user controller//
        [HttpPost]
        public ActionResult UserEdit(string userName, string firstName, string lastName, string email, string profileDesc)
        {
            try
            {
                Connection();
                connectToDB.Open();
                UserProfile db = new UserProfile();
                db = GetUser(userName);
                int userId = db.UserId;

                SqlCommand cmd = new SqlCommand("UPDATE [dbo].[UserProfile] SET firstName = @name_updated, lastName = @surname_updated, profileDesc = @profileDesc_updated, email = @email_updated WHERE UserId = @userid", connectToDB);
                cmd.Parameters.AddWithValue("@name_updated", firstName);
                cmd.Parameters.AddWithValue("@surname_updated", lastName);
                cmd.Parameters.AddWithValue("@profileDesc_updated", profileDesc);
                cmd.Parameters.AddWithValue("@email_updated", email);
                cmd.Parameters.AddWithValue("@userid", userId);
                cmd.ExecuteNonQuery();
                connectToDB.Close();
            }

            catch
            {
                TempData["msg"] = "Error. Could not Update the User!";
            }

            return RedirectToAction("UserEdit");
        }



    }
}
