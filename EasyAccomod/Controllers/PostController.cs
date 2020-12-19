using EasyAccomod.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyAccomod.Controllers
{
    public class PostController : Controller
    {
        public ActionResult Index(int id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreatePost()
        {
            int userid;
            if (Session["userid"] == null || !int.TryParse(Session["userid"].ToString(), out userid)) return PartialView("../Account/SignIn", new SignInModel());
            using (var db = new DBContext())
            {
                var account = (from a in db.Accounts
                               where a.Id == userid && a.Type == 2
                               select a).FirstOrDefault();
                if (account == null) return PartialView("../Account/SignIn", new SignInModel());
            }
            return View(new CreatePostModel());
        }

        [HttpPost]
        public ActionResult CreatePost(CreatePostModel model)
        {
            
            return View();
        }

        public ActionResult PostDetail(int id)
        {
            return View();
        }
    }
}