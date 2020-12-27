using EasyAccomod.Helpers;
using EasyAccomod.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyAccomod.Controllers
{
    public class EditInfoController : Controller
    {
        // GET: EditInfo
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

        [HttpGet]
        public ActionResult ChangePassword()
        {
            int userid;
            if (Session["userid"] == null || !int.TryParse(Session["userid"].ToString(), out userid)) return PartialView("/Account/SignIn", new SignInModel());
            return PartialView(new ChangePasswordModel());
        }

        [HttpPost]
        public ContentResult ChangePassword(ChangePasswordModel model)
        {
            int userid;
            if (Session["userid"] == null || !int.TryParse(Session["userid"].ToString(), out userid)) return Content("<script>window.location.href='/Account/SignIn'</script>", "text/javascript");
            if (!ModelState.IsValid) return Content("<span style='color: red'>>Kiểm tra lại giá trị đã nhập</span", "text/html");
            using (var db = new DBContext())
            {
                var account = (from a in db.Accounts
                               where a.Id == userid
                               select a).FirstOrDefault();
                if (account == null) return Content("<span style='color: red'>Không tìm thấy tài khoản</span>", "text/html");
                if (account.Password == model.OldPassword.ToMD5()) return Content("<span style='color: red'>Mật khẩu cũ không đúng</span>", "text/html");
                account.Password = model.NewPassword.ToMD5();
                db.SaveChanges();
            }
            return Content("Đổi mật khẩu thành công", "text/html");
        }
    }
}