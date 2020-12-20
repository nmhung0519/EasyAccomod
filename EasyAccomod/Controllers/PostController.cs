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
                               where a.Id == userid && (a.Type == 1 || a.Type == 2)
                               select a).FirstOrDefault();
                if (account == null) return PartialView("../Account/SignIn", new SignInModel());
            }
            return View(new CreatePostModel());
        }

        public ActionResult AdminPostManager()
        {
            return View();
        }

        public ActionResult HostPostManager()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreatePost(CreatePostModel model)
        {
            if (!ModelState.IsValid) return View(model);
            //
            return RedirectToAction("HostPostManager");
        }

        public ActionResult PostDetail(int id)
        {
            try
            {
                using (var db = new DBContext())
                {
                    var post = (from p in db.Posts
                                where p.Id == id
                                select p).FirstOrDefault();
                    db.Entry(post)
                        .Reference(x => x.Poster)
                        .Load();
                    db.Entry(post)
                        .Reference(x => x.Ward)
                        .Load();
                    db.Entry(post.Ward)
                        .Reference(x => x.District)
                        .Load();
                    db.Entry(post.Ward.District)
                        .Reference(x => x.City)
                        .Load();
                    post.Views = post.Views + 1;
                    db.SaveChanges();
                    return View(post);
                }
            }
            catch (Exception) { return Json("Đã xảy ra lỗi trong quá trình lấy dữ liệu bài viết"); }
        }
    }
}