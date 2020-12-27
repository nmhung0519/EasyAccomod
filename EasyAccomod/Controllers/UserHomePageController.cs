using EasyAccomod.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyAccomod.Controllers
{
    public class UserHomePageController : Controller
    {
        // GET: HomePage
        public ActionResult Index(int id)
        {
            using (var db = new DBContext())
            {
                var account = (from a in db.Accounts
                               where a.Id == id
                               select a).FirstOrDefault();
                if (account == null) return Json("Không tìm thấy người dùng", JsonRequestBehavior.AllowGet);
                account.Posts = (from p in db.Posts
                                 where p.PosterId == account.Id && p.IsShow
                                 orderby p.CreateTime descending
                                 select p).ToList();
                db.Entry(account)
                    .Collection(x => x.Posts)
                    .Load();
                foreach (var item in account.Posts)
                {
                    db.Entry(item)
                        .Collection(x => x.Images)
                        .Load();
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
                return View(account);
            }
        }
    }
}