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
            if (!ModelState.IsValid)
            {
                ModelError err = ModelState.Values.Where(x => x.Errors.Count() != 0).First().Errors[0];
                return Content(err.ErrorMessage, "text/html");
            }
            using (var db = new DBContext())
            {
                string password = model.password.ToMD5();
                var account = (from a in db.Accounts
                               where a.Username == model.username && a.Password == password
                               select a).FirstOrDefault();
                if (account == null)
                {
                    return Content("Tài khoản hoặc mật khẩu không chính xác. Đề nghị nhập lại", "text/html");
                }
                Session["usertype"] = account.Type;
                Session["userid"] = account.Id;
                Session["fullname"] = account.FullName;
            }
            return Content("<script>window.location.href='/';</script>", "text/javascript");
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View(new SignUpModel());
        }

        [HttpPost]
        public ContentResult SignUp(SignUpModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelError err = ModelState.Values.Where(x => x.Errors.Count() != 0).First().Errors[0];
                return Content(err.ErrorMessage, "text/html");
            }
            try
            {
                AccountModel account = new AccountModel();
                account.Accounts = new List<AccountModel>();
                account.Posts = new List<PostModel>();
                account.Notifications = new List<NotificationModel>();
                account.Username = model.Usernname;
                account.Password = model.Password.ToMD5();
                account.FullName = model.Fullname;
                account.Phone = model.Phone;
                account.Email = model.Email;
                account.IdCard = model.IdCard;
                account.Address = model.Address;
                account.WardId = model.WardId;
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
            return Content("<script>window.location.href='/Account/SignIn'</script>", "text/javascript");
        }

        public ActionResult SignOut()
        {
            Session["userid"] = null;
            Session["usertype"] = null;
            Session["fullname"] = null;
            return RedirectToAction("SignIn");
        }
    }
}