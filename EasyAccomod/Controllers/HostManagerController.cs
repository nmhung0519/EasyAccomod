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
            return View();
        }

        public ActionResult Post()
        {
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
                                            && p.EndTime.Year == 0)
                                       select p).ToList();
                model.HidingPost = (from p in posts
                                    where p.Approved == 1
                                        && (!p.IsShow || p.Sold)
                                    select p).ToList();
                model.RefusedPost = (from p in posts
                                    where p.Approved == -1
                                    select p).ToList();
                return PartialView(model);
            }
        }
    }
}