using EasyAccomod.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyAccomod.Controllers
{
    public class HostManagerController : Controller
    {
        public ActionResult Index()
        {
            if (Session["userid"] == null || Session["usertype"] == null) return RedirectToAction("SignIn", "Account");
            int userid, usertype;
            if (int.TryParse(Session["usertype"].ToString(), out usertype) && int.TryParse(Session["userid"].ToString(), out userid) && usertype != 1 && usertype != 2) return Json("Access Deny", JsonRequestBehavior.AllowGet);
            return View();
        }
        public ActionResult Post()
        {
            if (Session["userid"] == null) return RedirectToAction("SignIn", "Account");
            int userid;
            if (!(int.TryParse(Session["userid"].ToString(), out userid))) RedirectToAction("SignIn", "Account");
            using (var db = new DBContext())
            {
                var posts = (from p in db.Posts
                             where p.PosterId == userid
                             select p).ToList();
                foreach (var item in posts)
                {
                    db.Entry(item)
                        .Reference(x => x.City)
                        .Load();
                    db.Entry(item)
                        .Reference(x => x.District)
                        .Load();
                    db.Entry(item)
                        .Reference(x => x.Ward)
                        .Load();
                    db.Entry(item)
                        .Collection(x => x.Images)
                        .Load();
                }
                HostPostsModel model = new HostPostsModel();
                model.ShowingPost = (from p in posts
                                     where p.Approved == 1
                                        && p.IsShow
                                        && !p.Sold
                                        && p.EndTime >= DateTime.Now
                                     select p).ToList();
                model.ApprovingPost = (from p in posts
                                       where p.Approved == 0
                                            || (p.Approved == 1
                                            && !p.Sold
                                            && !p.IsShow
                                            && p.EndTime.Year < 2)
                                       select p).ToList();
                model.SoldPost = (from p in posts
                                    where p.Sold
                                    select p).ToList();
                model.ExpiredPost = (from p in posts
                                     where p.Approved == 1
                                        && p.IsShow
                                        && p.EndTime < DateTime.Now
                                     select p).ToList();
                model.RefusedPost = (from p in posts
                                    where p.Approved == -1
                                    select p).ToList();
                return PartialView(model);
            }
        }

        [HttpPost]
        public JsonResult ChangePostStatus(int id, bool sold)
        {
            try
            {
                int userid = 0;
                if (Session["userid"] == null || !int.TryParse(Session["userid"].ToString(), out userid)) Json("SignInFirst");
                using (var db = new DBContext())
                {
                    var post = (from p in db.Posts
                                where p.Id == id
                                select p).FirstOrDefault();
                    if (post.PosterId != userid) return Json("NotOwned");
                    if (post == null) return Json("PostNotFound");
                    if (post.Sold == sold) return Json("Proccessed");
                    post.Sold = sold;
                    db.SaveChanges();
                }
                return Json("Success");
            }
            catch (Exception ex) { return Json(ex.Message); }
        }

        [HttpPost]
        public JsonResult DeletePost(int id)
        {
            try
            {
                int userid = 0;
                if (Session["userid"] == null || !int.TryParse(Session["userid"].ToString(), out userid)) Json("SignInFirst");
                using (var db = new DBContext())
                {
                    var post = (from p in db.Posts
                                where p.Id == id
                                select p).FirstOrDefault();
                    if (post.PosterId != userid) return Json("NotOwned");
                    if (post == null) return Json("PostNotFound");
                    db.Posts.Remove(post);
                    db.SaveChanges();
                }
                return Json("Success");
            }
            catch (Exception ex) { return Json(ex.Message); }
        }
    }
}