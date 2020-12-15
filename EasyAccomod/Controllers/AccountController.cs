using EasyAccomod.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EasyAccomod.Helpers;

namespace EasyAccomod.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Signin()
        {
            return View(new SignInModel());
        }

        [HttpPost]
        public ActionResult SignIn(SignInModel model)
        {
            if (!ModelState.IsValid) return View(model);
            using (var db = new DBContext())
            {
                string password = model.password.ToMD5();
                var account = (from a in db.Accounts
                               where a.username == model.username && a.password == password
                               select a).FirstOrDefault();
                if (account == null)
                {
                    ModelState.AddModelError("err-msg", "Tài khoản hoặc mật khẩu không chính xác");
                    return View(model);
                }

                Session["userid"] = account.Id;
                Session["fullname"] = account.LastName + " " + account.FirstName;
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View(new SignUpModel());
        }

        [HttpPost]
        public ActionResult SignUp(SignUpModel model)
        {
            if (!ModelState.IsValid) return (View(model));
            return RedirectToAction("SignIn");
        }

        public ActionResult Signout()
        {
            Session["userid"] = null;
            Session["fullname"] = null;
            return RedirectToAction("SignIn");
        }
    }
}