using EasyAccomod.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

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
                                && (a.Type == 1 || (a.Type == 2 && a.Approved && a.ApprovalTime.Year != 0))
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
                newPost.ElectricityPrice = model.ElectricityPrice;
                newPost.WaterPrice = model.WaterPrice;
                newPost.AirConditioner = (model.AirConditioner) ? 1 : 0;
                newPost.WaterHeater = (model.WaterHeater) ? 1 : 0;
                newPost.PrivateKitchen = (model.PrivateKitchen) ? 1 : 0;
                newPost.Balcony = (model.Balcony) ? 1 : 0;
                newPost.ClosedRoom = (model.ClosedRoom) ? 1 : 0;
                newPost.WithoutHost = (model.WithoutHost) ? 1 : 0;
                newPost.PosterId = int.Parse(Session["userid"].ToString());
                newPost.ContactName = Session["fullname"].ToString();
                newPost.CreateTime = DateTime.Now;
                using (var db = new DBContext())
                {
                    var user = (from a in db.Accounts
                                where a.Id == newPost.PosterId
                                select a).FirstOrDefault();
                    if (user == null) return Content("window.location.href = '/Account/SignIn'", "text/javascript");
                    newPost.ContactPhone = user.Phone;
                    if (user.Type == 1)
                    {
                        newPost.StartTime = newPost.CreateTime;
                        newPost.EndTime = newPost.CreateTime.AddDays(7);
                        newPost.Approved = 1;
                        newPost.IsShow = true;
                    }
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
                TicketModel ticket = new TicketModel();
                ticket.ApproverId = 0;
                ticket.CreateTime = DateTime.Now;
                ticket.Time = 1;
                ticket.UnitTime = 1;
                if (newPost.Approved == 1)
                {
                    ticket.Approved = 1;
                }
                newPost.Tickets = new List<TicketModel> { ticket };
                newPost.Images = new List<PostImageModel>();
                for (int i = 0; i < model.UploadImages.Count(); i++)
                {
                    var file = model.UploadImages[i];
                    if (file == null) continue;
                    string fileName = newPost.CreateTime.Year
                        + ((newPost.CreateTime.Month < 10) ? "0" : "") + newPost.CreateTime.Month
                        + ((newPost.CreateTime.Day < 10) ? "0" : "") + newPost.CreateTime.Day
                        + ((newPost.CreateTime.Hour < 10) ? "0" : "") + newPost.CreateTime.Hour
                        + ((newPost.CreateTime.Minute < 10) ? "0" : "") + newPost.CreateTime.Minute
                        + ((newPost.CreateTime.Second < 10) ? "0" : "") + newPost.CreateTime.Second
                        + i + newPost.PosterId + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("~/UploadImage/" + fileName));
                    newPost.Images.Add(new PostImageModel { Path = fileName });
                }
                using (var db = new DBContext())
                {
                    db.Posts.Add(newPost);
                    db.SaveChanges();
                }

            }
            catch (Exception ex) { return Content(ex.Message, "text/html"); }
            return Content("<script>window.location.href = '/HostManager'</script>", "text/javascript");
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
                    db.Entry(post)
                        .Collection(x => x.Images)
                        .Load();
                    post.Views = post.Views + 1;
                    ViewPostModel viewPost = new ViewPostModel();
                    viewPost.UserId = 0;
                    if (Session["userid"] != null) viewPost.UserId = int.Parse(Session["userid"].ToString());
                    viewPost.PostId = post.Id;
                    viewPost.Time = DateTime.Now;
                    db.ViewPosts.Add(viewPost);
                    db.SaveChanges();
                    return View(post);
                }
            }
            catch (Exception) { return Json("Đã xảy ra lỗi trong quá trình lấy dữ liệu bài viết"); }
        }
    }
}