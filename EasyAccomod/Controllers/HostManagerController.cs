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
        // GET: PostManager
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
                var model = (from p in db.Posts
                             where p.PosterId == userid
                             select p).ToList();
                return PartialView(model);
            }
        }
    }
}