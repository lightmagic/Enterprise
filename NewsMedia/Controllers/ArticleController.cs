using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsMedia.Models;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Data;

namespace NewsMedia.Controllers
{
    public class ArticleController : Controller
    {
        //
        // GET: /Article/
        UsersContext db = new UsersContext();
        ArticleContext art = new ArticleContext();

        public string connectionString = "Data Source=KYLE\\SQLSERVER;Initial Catalog=Enterprise;Integrated Security=True";
        private SqlConnection connectToDB;

        private void Connection()
        {
            connectToDB = new SqlConnection(connectionString);
        }

        //View
        public ActionResult Articles()
        {
            var model = art.ArticleCollection.ToList();
            return View(model);
        }

        //to get the user who shall be connected with the article
        public UserProfile GetUser(string userName)
        {
            var model = db.UserProfiles.ToList();
            return model.SingleOrDefault(x => x.UserName == userName);
        }

        public Article GetArticle(string Title)
        {
            var model = art.ArticleCollection.ToList();
            return model.SingleOrDefault(a => a.Title == Title);
        }

        public ActionResult Create()
        {
            return View();
        }

        //create
        [HttpPost]
        public ActionResult Create(string username, string categories, string Title, string subHeader, 
                                   string Content, string imageArticle, bool breakingNews)
        {
            try
            {
                Connection();
                connectToDB.Open();

                UserProfile db = new UserProfile(); 
                db = GetUser(username); //will retreive the username for the created article, so that we can link the article to user
                int userId = db.UserId;

                int isBreakingbool = 0;
                if (breakingNews == true)
                {
                    isBreakingbool = 1;
                }

                SqlCommand cmd = new SqlCommand("INSERT INTO [dbo].[Article] (Title, subHeader, Content, dateCreated, imageArticle, breakingNews, UserId, categoryId) VALUES (@articleTitle, @subHeader, @articleContent, @dateCreated, @imageArticle, @isBreaking, @UserId, @categoryId)", connectToDB);
                cmd.Parameters.AddWithValue("@articleTitle", Title);
                cmd.Parameters.AddWithValue("@subHeader", subHeader);
                cmd.Parameters.AddWithValue("@articleContent", Content);
                cmd.Parameters.AddWithValue("@dateCreated", DateTime.Now);
                cmd.Parameters.AddWithValue("@imageArticle", imageArticle);
                cmd.Parameters.AddWithValue("@isBreaking", isBreakingbool);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@categoryId", categories); 
                cmd.ExecuteNonQuery(); //executes the query so that it will query all the data into Article database
                connectToDB.Close();
            }

            catch
            {
                TempData["msg"] = "Error. Could not Create the article";
            }
            return RedirectToAction("Articles");
        }

        //Edit
        public ActionResult Edit(string Title)
        {
            Article art = new Article();
            art = GetArticle(Title);
            return View(art); 
        }

        [HttpPost]
        public ActionResult Edit(string Title, string subHeader, string Content, int categoryId)
        {
            try
            {
                Connection();
                connectToDB.Open();

                Article art = new Article();
                art = GetArticle(Title); //retreive the article title
                int articleId = art.articleId;


                SqlCommand cmd = new SqlCommand("UPDATE [dbo].[Article] SET Title = @Title, subHeader = @subHeader, Content = @Content, categoryId = @categoryId WHERE articleId = @articleId", connectToDB);
                cmd.Parameters.AddWithValue("@Title", Title);
                cmd.Parameters.AddWithValue("@subHeader", subHeader);
                cmd.Parameters.AddWithValue("@Content", Content);
                cmd.Parameters.AddWithValue("@categoryId", categoryId);
                cmd.Parameters.AddWithValue("@articleId", articleId);

                cmd.ExecuteNonQuery(); //executes the query so that it will query all the data and Updates Article database
                connectToDB.Close();
            }

            catch
            {
                TempData["msg"] = "Error. Could not Create the article";
            }
            return RedirectToAction("Articles");
        }


        //Delete
        public ActionResult DeleteArticle(string Title)
        {
            try
            {
                Connection();
                connectToDB.Open();
                Article art = new Article();
                art = GetArticle(Title);
                int ArticleId = art.articleId;
                SqlCommand cmd = new SqlCommand("DELETE FROM [dbo].[Article] WHERE articleId = @ArticleId", connectToDB);
                cmd.Parameters.AddWithValue("@ArticleId", ArticleId);
                cmd.ExecuteNonQuery();
                connectToDB.Close();
            }

            catch
            {
                TempData["msg"] = "Error. Could not Delete the article";
            }

            return RedirectToAction("Articles");
        }

        public ActionResult National()
        {
            //nat
            var model = NationalCategory();
            return View(model);
        }

        public List<Article> NationalCategory()
        {
            Connection();
            List<Article> nationalCategory = new List<Article>(); //list of articles of that particular category
            DataTable getNat = new DataTable(); //to use select option which will retreive all data
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [dbo].[Article] WHERE categoryId = '3'", connectToDB);
            adapter.Fill(getNat); //fill in sportTb with data retreived from sql

            foreach (DataRow data in getNat.Rows)
            {
                Article art = new Article();
                art.Title = data["Title"].ToString();
                art.subHeader = data["subHeader"].ToString();
                art.Content = data["Content"].ToString();
                art.dateCreated = (DateTime)data["dateCreated"];
                art.UserId = (int)data["UserId"];
                art.categoryId = (int)data["categoryId"];
                nationalCategory.Add(art);
            }

            //return the list
            return nationalCategory;
        }

        public ActionResult _OverSeas()
        {
            var model = OverSeasCategory();
            return View(model);
        }

        public List<Article> OverSeasCategory()
        {
            Connection();
            List<Article> overseaCategory = new List<Article>(); //list of articles of that particular category
            DataTable getSea = new DataTable(); //to use select option which will retreive all data
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [dbo].[Article] WHERE categoryId = '4'", connectToDB);
            adapter.Fill(getSea); //fill in sportTb with data retreived from sql

            foreach (DataRow data in getSea.Rows)
            {
                Article art = new Article();
                art.Title = data["Title"].ToString();
                art.subHeader = data["subHeader"].ToString();
                art.Content = data["Content"].ToString();
                art.dateCreated = (DateTime)data["dateCreated"];
                art.UserId = (int)data["UserId"];
                art.categoryId = (int)data["categoryId"];
                overseaCategory.Add(art);
            }

            //return the list
            return overseaCategory;
        }

    }
}
