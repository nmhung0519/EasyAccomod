using EasyAccomod.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyAccomod.Controllers
{
    public class PostController : Controller
    {
        public ActionResult Index(int id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreatePost()
        {
            int userid;
            if (Session["userid"] == null || !int.TryParse(Session["userid"].ToString(), out userid)) return PartialView("../Account/SignIn", new SignInModel());
            using (var db = new DBContext())
            {
                var account = (from a in db.Accounts
                               where a.Id == userid
                                && (a.Type == 1 || (a.Type == 2 && a.ApprovalTime.Year != 0))
                               select a).FirstOrDefault();
                if (account == null) return PartialView("CreatePostAccessDeny");
            }
            return View(new CreatePostModel());
        }

        [HttpPost]
        public ContentResult CreatePost(CreatePostModel model)
        {
            if (!ModelState.IsValid) return Content("Form không hợp lệ", "text/html");
            try
            {
                PostModel newPost = new PostModel();
                newPost.Address = model.Address;
                newPost.CityId = model.CityId;
                newPost.DistrictId = model.DistrictId;
                newPost.WardId = model.WardId;
                newPost.Price = model.Price;
                newPost.PriceUnit = 1;
                newPost.Type = model.Type;
                newPost.Area = model.Area;
                newPost.ElectricityPrice = "s" + model.ElectricityPrice;
                newPost.WaterPrice = "s" + model.WaterPrice;
                newPost.AirConditioner = (model.AirConditioner) ? 1 : 0;
                newPost.WaterHeater = (model.WaterHeater) ? 1 : 0;
                newPost.PrivateKitchen = (model.PrivateKitchen) ? 1 : 0;
                newPost.Balcony = (model.Balcony) ? 1 : 0;
                newPost.ClosedRoom = (model.ClosedRoom) ? 1 : 0;
                newPost.WithoutHost = (model.WithoutHost) ? 1 : 0;
                newPost.PosterId = int.Parse(Session["userid"].ToString());
                newPost.ContactName = Session["fullname"].ToString();
                using (var db = new DBContext())
                {
                    var user = (from a in db.Accounts
                                where a.Id == newPost.PosterId
                                select a).FirstOrDefault();
                    if (user == null) return Content("window.location.href = '/Account/SignIn'", "text/javascript");
                    newPost.ContactPhone = user.Phone;
                    if (user.Type == 1) newPost.Approved = true;
                }
                switch (newPost.Type)
                {
                    case 1:
                        newPost.Title = "Phòng trọ";
                        break;
                    case 2:
                        newPost.Title = "Chung cư mini";
                        break;
                    case 3:
                        newPost.Title = "Nhà nguyên căn";
                        break;
                    case 4:
                        newPost.Title = "Chung cư nguyên căn";
                        break;
                }

                newPost.Title = newPost.Title + " - " + newPost.Address;
                newPost.Content = newPost.Title;
                newPost.RentTime = "6 tháng";
                newPost.Deposit = "1 tháng";
                newPost.DatePerTime = "1 tháng";
                newPost.CreateTime = DateTime.Now;

                TicketModel ticket = new TicketModel();
                ticket.ApproverId = int.Parse(Session["userid"].ToString());
                ticket.StartTime = DateTime.Now;
                if (newPost.Approved)
                {
                    ticket.EndTime = DateTime.Now.AddDays(7);
                    ticket.Approved = newPost.Approved;
                }
                newPost.Tickets = new List<TicketModel> { ticket };
                using (var db = new DBContext())
                {
                    db.Posts.Add(newPost);
                    db.SaveChanges();
                }

            }
            catch (Exception ex) { return Content(ex.Message, "text/html"); }
            return Content("window.location.href = '/Post/HostPostManager'", "text/javascript");
        }

        public ActionResult PostDetail(int id)
        {
            try
            {
                using (var db = new DBContext())
                {
                    var post = (from p in db.Posts
                                where p.Id == id
                                select p).FirstOrDefault();
                    db.Entry(post)
                        .Reference(x => x.Poster)
                        .Load();
                    db.Entry(post)
                        .Reference(x => x.Ward)
                        .Load();
                    db.Entry(post.Ward)
                        .Reference(x => x.District)
                        .Load();
                    db.Entry(post.Ward.District)
                        .Reference(x => x.City)
                        .Load();
                    post.Views = post.Views + 1;
                    db.SaveChanges();
                    return View(post);
                }
            }
            catch (Exception) { return Json("Đã xảy ra lỗi trong quá trình lấy dữ liệu bài viết"); }
        }
    }
}