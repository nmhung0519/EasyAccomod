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

        [HttpGet]
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
                db.Entry(account)
                    .Reference(x => x.Ward)
                    .Load();
                db.Entry(account.Ward)
                    .Reference(x => x.District)
                    .Load();
                UserInformationModel model = new UserInformationModel();
                model.Fullname = account.FullName;
                model.Phone = account.Phone;
                model.Email = account.Email;
                model.CityId = account.Ward.District.CityId;
                model.DistrictId = account.Ward.DistrictId;
                model.WardId = account.WardId;
                model.IdCard = account.IdCard;
                model.Address = account.Address;
                model.EditRole = account.EditRole;
                return PartialView(model);
            }
        }

        [HttpPost]
        public ContentResult UserInformation(UserInformationModel model)
        {
            int userid;
            if (Session["userid"] == null || !int.TryParse(Session["userid"].ToString(), out userid)) return Content("<script>window.location.href='/Account/SignIn';</script>", "text/javascript");
            try
            {
                using (var db = new DBContext())
                {
                    var account = (from a in db.Accounts
                                   where a.Id == userid
                                   select a).FirstOrDefault();
                    if (account == null) return Content("<script>window.location.href='/Account/SignOut';</script>", "text/javascript");
                    if (!account.EditRole) return Content("<span style='color: red'>Bạn phải được admin cấp quyền chỉnh sửa để có thể chỉnh sửa thông tin cá nhân</span>", "text/html");
                    if (account.Type == 2)
                    {
                        account.Approved = false;
                        account.ApprovalTime = DateTime.MinValue.AddYears(1999);
                    }
                    account.FullName = model.Fullname;
                    account.Phone = model.Phone;
                    account.Email = model.Email;
                    account.IdCard = model.IdCard;
                    account.WardId = model.WardId;
                    account.Address = model.Address;
                    db.SaveChanges();
                    if (account.Type == 2) return Content("Cập nhật thông tin cá nhân thành công. Bạn sẽ không được đăng bài cho đến khi admin duyệt.", "text/html");
                }
                return Content("Cập nhật thông tin cá nhân thành công", "text/html");
            }
            catch (Exception ex) { return Content("<span style='color: red'>" + ex.Message + "</span>", "text/html"); }
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
            if (!ModelState.IsValid) return Content("<span style='color: red'>Các trường không được để trống</span", "text/html");
            using (var db = new DBContext())
            {
                var account = (from a in db.Accounts
                               where a.Id == userid
                               select a).FirstOrDefault();
                if (account == null) return Content("<span style='color: red'>Không tìm thấy tài khoản</span>", "text/html");
                if (account.Password != model.OldPassword.ToMD5()) return Content("<span style='color: red'>Mật khẩu cũ không đúng</span>", "text/html");
                account.Password = model.NewPassword.ToMD5();
                db.SaveChanges();
            }
            return Content("Đổi mật khẩu thành công", "text/html");
        }
    }
}