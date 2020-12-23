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
        public ActionResult SignIn()
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
                               where a.Username == model.username && a.Password == password
                               select a).FirstOrDefault();
                if (account == null)
                {
                    ModelState.AddModelError("err-msg", "Tài khoản hoặc mật khẩu không chính xác");
                    return View(model);
                }
                Session["usertype"] = account.Type;
                Session["userid"] = account.Id;
                Session["fullname"] = account.FullName;
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View(new SignUpModel());
        }

        [HttpPost]
        public ContentResult SignUp(SignUpModel model)
        {
            if (!ModelState.IsValid) return Content("Dữ liệu không đúng định dạng", "text/html");
            try
            {
                AccountModel account = new AccountModel();
                account.Accounts = new List<AccountModel>();
                account.Posts = new List<PostModel>();
                account.Notifications = new List<NotificationModel>();
                account.Username = model.usernname;
                account.Password = model.password.ToMD5();
                account.FullName = model.fullname;
                account.Phone = model.phone;
                account.Email = model.email;
                account.IdCard = model.idCard;
                account.Address = model.address;
                account.WardId = model.wardId;
                account.Type = model.UserType;
                account.ApproverId = 1;
                using (var db = new DBContext())
                {
                    var acc = (from a in db.Accounts
                               where a.Username == account.Username
                               select a).FirstOrDefault();
                    if (acc != null) return Content("Tài khoản đã tồn tại", "text/html");
                    db.Accounts.Add(account);
                    db.SaveChanges();
                }
            }
            catch (Exception ex) { return Content(ex.Message, "text/html"); }
            return Content("window.location.href='/Account/SignIn'", "text/javascript");
        }

        public ActionResult SignOut()
        {
            Session["userid"] = null;
            Session["fullname"] = null;
            return RedirectToAction("SignIn");
        }
    }
}