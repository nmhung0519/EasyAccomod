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
        // GET: UserHomePage
        public ActionResult Index()
        {
            int userid;
            if (Session["userid"] == null || !int.TryParse(Session["userid"].ToString(), out userid)) return RedirectToAction("SignIn", "Account");
            using (var db = new DBContext())
            {
                var account = (from a in db.Accounts
                               where a.Id == userid
                               select a).FirstOrDefault();
                if (account == null) return RedirectToAction("SignOut", "Account");
                return View(account);
            }
        }

        public ActionResult UserInformation()
        {
            int userid;
            if (Session["userid"] == null || !int.TryParse(Session["userid"].ToString(), out userid)) return PartialView("/Account/SignIn", new SignInModel());
            using (var db = new DBContext())
            {
                var account = (from a in db.Accounts
                               where a.Id == userid
                               select a).FirstOrDefault();
                if (account == null) return PartialView("/Account/SignIn", new SignInModel());
                return PartialView(account);
            }
        }
    }
}