using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewsMedia.Models;
using System.Data.SqlClient;

namespace NewsMedia.Controllers
{

    public class HomeController : Controller
    {
        UsersContext db = new UsersContext();
        ArticleContext art = new ArticleContext(); 

        public ActionResult Index()
        {

            var model = art.ArticleCollection.ToList();
            return View(model);
        }

    }
}
