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

        //public string connectionString = "Data Source=tcp:enterprisekyle.database.windows.net,1433;Initial Catalog=kyleEnterprise;Persist Security Info=False;User ID=kyle;Password=Enterprise123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
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

        //display single article
        public ActionResult View(string Title)
        {
            Article art = new Article();
            art = GetArticle(Title);
            return View(art);
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
        [Authorize]
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
        [Authorize]
        public ActionResult Edit(string Title, string subHeader, string Content, int categoryId, bool breakingNews)
        {
            try
            {
                Connection();
                connectToDB.Open();

                Article art = new Article();
                art = GetArticle(Title); //retreive the article title
                int articleId = art.articleId;

                int isBreakingbool = 0;
                if (breakingNews == true)
                {
                    isBreakingbool = 1;
                }

                SqlCommand cmd = new SqlCommand("UPDATE [dbo].[Article] SET Title = @Title, subHeader = @subHeader, Content = @Content, categoryId = @categoryId, breakingNews = @breakingNews WHERE articleId = @articleId", connectToDB);
                cmd.Parameters.AddWithValue("@Title", Title);
                cmd.Parameters.AddWithValue("@subHeader", subHeader);
                cmd.Parameters.AddWithValue("@Content", Content);
                cmd.Parameters.AddWithValue("@categoryId", categoryId);
                cmd.Parameters.AddWithValue("@breakingNews", isBreakingbool);
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
        [Authorize]
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

        public ActionResult Sports()
        {
            var model = SportsCategory();
            return View(model);
        }

        public List<Article> SportsCategory()
        {
            Connection();
            List<Article> sportCategory = new List<Article>(); //list of articles of that particular category
            DataTable getSport = new DataTable(); //to use select option which will retreive all data
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [dbo].[Article] WHERE categoryId = '1'", connectToDB);
            adapter.Fill(getSport); //fill in sportTb with data retreived from sql

            foreach (DataRow data in getSport.Rows)
            {
                Article art = new Article();
                art.Title = data["Title"].ToString();
                art.subHeader = data["subHeader"].ToString();
                art.Content = data["Content"].ToString();
                art.imageArticle = data["imageArticle"].ToString();
                art.dateCreated = (DateTime)data["dateCreated"];
                art.UserId = (int)data["UserId"];
                art.categoryId = (int)data["categoryId"];
                sportCategory.Add(art);
            }

            //return the list
            return sportCategory;
        }

        public ActionResult Opinion()
        {
            var model = OpinionCategory();
            return View(model);
        }

        public List<Article> OpinionCategory()
        {
            Connection();
            List<Article> opinionCategory = new List<Article>(); //list of articles of that particular category
            DataTable getopinion = new DataTable(); //to use select option which will retreive all data
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [dbo].[Article] WHERE categoryId = '2'", connectToDB);
            adapter.Fill(getopinion); //fill in sportTb with data retreived from sql

            foreach (DataRow data in getopinion.Rows)
            {
                Article art = new Article();
                art.Title = data["Title"].ToString();
                art.subHeader = data["subHeader"].ToString();
                art.Content = data["Content"].ToString();
                art.imageArticle = data["imageArticle"].ToString();
                art.dateCreated = (DateTime)data["dateCreated"];
                art.UserId = (int)data["UserId"];
                art.categoryId = (int)data["categoryId"];
                opinionCategory.Add(art);
            }

            //return the list
            return opinionCategory;
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
                art.imageArticle = data["imageArticle"].ToString();
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
                art.imageArticle = data["imageArticle"].ToString();
                art.dateCreated = (DateTime)data["dateCreated"];
                art.UserId = (int)data["UserId"];
                art.categoryId = (int)data["categoryId"];
                overseaCategory.Add(art);
            }

            //return the list
            return overseaCategory;
        }

        public ActionResult Travel()
        {
            var model = TravelCategory();
            return View(model);
        }

        public List<Article> TravelCategory()
        {
            Connection();
            List<Article> travelCategory = new List<Article>(); //list of articles of that particular category
            DataTable getTravel = new DataTable(); //to use select option which will retreive all data
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [dbo].[Article] WHERE categoryId = '5'", connectToDB);
            adapter.Fill(getTravel); //fill in sportTb with data retreived from sql

            foreach (DataRow data in getTravel.Rows)
            {
                Article art = new Article();
                art.Title = data["Title"].ToString();
                art.subHeader = data["subHeader"].ToString();
                art.Content = data["Content"].ToString();
                art.imageArticle = data["imageArticle"].ToString();
                art.dateCreated = (DateTime)data["dateCreated"];
                art.UserId = (int)data["UserId"];
                art.categoryId = (int)data["categoryId"];
                travelCategory.Add(art);
            }
            //return the list
            return travelCategory;
        }

        public ActionResult Odd()
        {
            var model = OddCategory();
            return View(model);
        }

        public List<Article> OddCategory()
        {
            Connection();
            List<Article> oddCategory = new List<Article>(); //list of articles of that particular category
            DataTable getodd = new DataTable(); //to use select option which will retreive all data
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM [dbo].[Article] WHERE categoryId = '6'", connectToDB);
            adapter.Fill(getodd); //fill in sportTb with data retreived from sql

            foreach (DataRow data in getodd.Rows)
            {
                Article art = new Article();
                art.Title = data["Title"].ToString();
                art.subHeader = data["subHeader"].ToString();
                art.Content = data["Content"].ToString();
                art.imageArticle = data["imageArticle"].ToString();
                art.dateCreated = (DateTime)data["dateCreated"];
                art.UserId = (int)data["UserId"];
                art.categoryId = (int)data["categoryId"];
                oddCategory.Add(art);
            }

            //return the list
            return oddCategory;
        }

    }
}
