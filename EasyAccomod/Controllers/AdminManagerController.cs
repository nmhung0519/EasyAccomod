﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EasyAccomod.Models;

namespace EasyAccomod.Controllers
{
    public class AdminManagerController : Controller
    {
        // GET: Manager
        public ActionResult Index()
        {
            int usertype;
            if (Session["userid"] == null || Session["usertype"] == null || !int.TryParse(Session["usertype"].ToString(), out usertype)) return RedirectToAction("SignIn", "Account");
            if (usertype != 1) return Content("<a href='/Account/SignOut'>Đăng nhập lại</a> với tài khoản admin để sử dụng chức năng này", "text/html");
            return View();
        }

        [HttpGet]
        public ActionResult ApproveAccount()
        {
            using (var db = new DBContext())
            {
                var accounts = (from a in db.Accounts
                             where a.Type == 2 && a.Approved == false && a.ApprovalTime.Year <= 2000
                             select a).ToList();
                return PartialView(accounts);
            }
        }

        [HttpPost]
        public JsonResult ApproveAccount(int id)
        {
            int userid, usertype;
            if (Session["userid"] == null || !int.TryParse(Session["userid"].ToString(), out userid)) return Json("SignInFirst");
            if (Session["usertype"] == null || !int.TryParse(Session["usertype"].ToString(), out usertype) || usertype != 1) return Json("AdminOnly");
            try
            {
                using (var db = new DBContext())
                {
                    var account = (from a in db.Accounts
                                   where a.Id == id
                                   select a).FirstOrDefault();
                    if (account.Approved || (account.Approved && account.ApprovalTime.Year > 1)) return Json("AccountProccessed");
                    account.Approved = true;
                    account.ApprovalTime = DateTime.Now;
                    account.ApproverId = userid;
                    account.EditRole = false;
                    db.SaveChanges();
                    return Json("Success");
                }
            }
            catch (Exception ex) { return Json(ex.Message); }
        }

        [HttpPost]
        public JsonResult RefuseAccount(int id)
        {
            int userid, usertype;
            if (Session["userid"] == null || !int.TryParse(Session["userid"].ToString(), out userid)) return Json("SignInFirst");
            if (Session["usertype"] == null || !int.TryParse(Session["usertype"].ToString(), out usertype) || usertype != 1) return Json("AdminOnly");
            using (var db = new DBContext())
            {
                var account = (from a in db.Accounts
                               where a.Id == id
                               select a).FirstOrDefault();
                if (account.Approved || (account.Approved && account.ApprovalTime.Year > 1)) return Json("AccountProccessed");
                account.ApprovalTime = DateTime.Now;
                account.ApproverId = userid;
                db.SaveChanges();
                return Json("Success");
            }
        }

        [HttpPost]
        public JsonResult ApprovePost(int id)
        {
            if (Session["userid"] == null || Session["usertype"] == null) return Json("SignInFirst");
            int userid, usertype = 0;
            if (!int.TryParse(Session["userid"].ToString(), out userid) || !int.TryParse(Session["usertype"].ToString(), out usertype) || usertype != 1) return Json("AdminOnly");
            using (var db = new DBContext())
            {
                var ticket = (from t in db.Tickets
                              where t.Id == id && t.Approved == 0 && t.ApprovalTime.Year < 2000
                              select t).FirstOrDefault();
                if (ticket == null) return Json("TicketNotFound");
                if (ticket.Approved != 0) return Json("TicketProcessed");
                ticket.ApproverId = userid;
                ticket.ApprovalTime = DateTime.Now;
                ticket.Approved = 1;
                db.Entry(ticket)
                    .Reference(x => x.Post)
                    .Load();
                if (ticket.Post.Approved == -1) return Json("PostRefued");
                //if (ticket.Post.EndTime >= DateTime.Now && ticket.Post.Approved == 1 && ticket.Post.IsShow) return Json("PostShowed");
                ticket.Post.Approved = 1;
                ticket.Post.IsShow = true;
                ticket.Post.StartTime = DateTime.Now;
                switch (ticket.UnitTime)
                {
                    case 1:
                        ticket.Post.EndTime = ticket.Post.StartTime.AddDays(ticket.Time * 7);
                        break;
                    case 2:
                        ticket.Post.EndTime = ticket.Post.StartTime.AddMonths(ticket.Time);
                        break;
                    case 3:
                        ticket.Post.EndTime = ticket.Post.StartTime.AddMonths(ticket.Time * 3);
                        break;
                    case 4:
                        ticket.Post.EndTime = ticket.Post.StartTime.AddYears(ticket.Time);
                        break;
                    default:
                        ticket.Post.EndTime = ticket.Post.StartTime;
                        break;
                }
                db.SaveChanges();
            }
            return Json("Success");
        }

        [HttpPost]
        public JsonResult RefusePost(int id)
        {
            if (Session["userid"] == null || Session["usertype"] == null) return Json("SignInFirst");
            int userid, usertype = 0;
            if (!int.TryParse(Session["userid"].ToString(), out userid) || !int.TryParse(Session["usertype"].ToString(), out usertype) || usertype != 1) return Json("AdminOnly");
            using (var db = new DBContext())
            {
                var ticket = (from t in db.Tickets
                              where t.Id == id && t.Approved == 0
                              select t).FirstOrDefault();
                if (ticket == null) return Json("TicketNotFound");
                if (ticket.Approved != 0) return Json("TicketProcessed");
                ticket.ApproverId = userid;
                ticket.ApprovalTime = DateTime.Now;
                ticket.Approved = -1;
                db.Entry(ticket)
                    .Reference(x => x.Post)
                    .Load();
                if (ticket.Post.Approved == -1) return Json("PostRefued");
                if (ticket.Post.EndTime >= DateTime.Now && ticket.Post.Approved == 1 && ticket.Post.IsShow) return Json("PostShowed");
                ticket.Post.IsShow = false;
                if (ticket.Post.Approved == 0) ticket.Post.Approved = -1;
                db.SaveChanges();
            }
            return Json("Success");
        }

        [HttpGet]
        public ActionResult ApproveComment()
        {
            using (var db = new DBContext())
            {
                var comments = (from c in db.Comments
                                where (!c.Approved) && (c.ApprovalTime.Year <= 1)
                                select c).ToList();
                foreach (var item in comments)
                {
                    db.Entry(item)
                        .Reference(x => x.Post)
                        .Load();
                    db.Entry(item)
                        .Reference(x => x.User)
                        .Load();
                    db.Entry(item.Post)
                        .Reference(x => x.Poster)
                        .Load();
                    db.Entry(item.Post)
                        .Reference(x => x.City)
                        .Load();
                    db.Entry(item.Post)
                        .Reference(x => x.District)
                        .Load();
                    db.Entry(item.Post)
                        .Reference(x => x.Ward)
                        .Load();
                    db.Entry(item.Post)
                        .Collection(x => x.Images)
                        .Load();
                }
                return PartialView(comments);
            }
        }

        [HttpPost]
        public JsonResult ApproveComment(int id)
        {
            int userid, usertype;
            if (Session["userid"] == null || Session["usertype"] == null || !int.TryParse(Session["userid"].ToString(), out userid) || !int.TryParse(Session["usertype"].ToString(), out usertype)) return Json("SignInFirst");
            if (usertype != 1) return Json("AdminOnly");
            try
            {
                using (var db = new DBContext())
                {
                    var comment = (from c in db.Comments
                                   where c.Id == id
                                   select c).FirstOrDefault();
                    if (comment == null) { return Json("CommentNotFound"); }
                    if (comment.ApprovalTime.Year > 1) return Json("CommentProcessed");
                    comment.ApprovalTime = DateTime.Now;
                    comment.ApproverId = userid;
                    comment.Approved = true;
                    db.SaveChanges();
                }
                return Json("Success");
            }
            catch (Exception ex) { return Json(ex.Message); }
        }

        [HttpPost]
        public JsonResult RefuseComment(int id)
        {
            int userid, usertype;
            if (Session["userid"] == null || Session["usertype"] == null || !int.TryParse(Session["userid"].ToString(), out userid) || !int.TryParse(Session["usertype"].ToString(), out usertype)) return Json("SignInFirst");
            if (usertype != 1) return Json("AdminOnly");
            try
            {
                using (var db = new DBContext())
                {
                    var comment = (from c in db.Comments
                                   where c.Id == id
                                   select c).FirstOrDefault();
                    if (comment == null) { return Json("CommentNotFound"); }
                    if (comment.ApprovalTime.Year > 1) return Json("CommentProcessed");
                    comment.ApprovalTime = DateTime.Now;
                    comment.ApproverId = userid;
                    db.SaveChanges();
                }
                return Json("Success");
            }
            catch (Exception ex) { return Json(ex.Message); }
        }

        [HttpGet]
        public ActionResult ApprovePost()
        {
            using (var db = new DBContext())
            {
                var tickets = (from t in db.Tickets
                               where t.Approved == 0 && t.ApprovalTime.Year <= 1
                               select t).ToList();
                foreach (var item in tickets)
                {
                    db.Entry(item)
                        .Reference(x => x.Post)
                        .Load();
                    db.Entry(item.Post)
                        .Collection(x => x.Images)
                        .Load();
                    db.Entry(item.Post)
                        .Reference(x => x.Ward)
                        .Load();
                    db.Entry(item.Post)
                        .Reference(x => x.District)
                        .Load();
                    db.Entry(item.Post)
                        .Reference(x => x.City)
                        .Load();
                    db.Entry(item.Post)
                        .Reference(x => x.Poster)
                        .Load();
                }
                return PartialView(tickets);
            }
        }

        public ActionResult ApprovePurchase()
        {
            int usertype;
            if (Session["userid"] == null || Session["usertype"] == null || !int.TryParse(Session["usertype"].ToString(), out usertype)) return RedirectToAction("SignIn", "Account");
            if (usertype != 1) return Content("<a href='/Account/SignOut'>Đăng nhập lại</a> với tài khoản admin để sử dụng chức năng này", "text/html");
            List<PostModel> result = new List<PostModel>();
            using (var db = new DBContext())
            {
                var posts = (from p in db.Posts
                             where p.Approved == 1 && p.Sold && p.IsShow
                             select p).ToList();
                foreach (var item in posts)
                {
                    db.Entry(item)
                        .Collection(x => x.Tickets)
                        .Load();
                    var tmp = (from t in item.Tickets
                               where t.ApprovalTime.Year <= 2000
                               orderby t.CreateTime descending
                               select t).FirstOrDefault();
                    if (tmp != null)
                    {
                        item.ApprovalTicket = tmp;
                        result.Add(item);
                        db.Entry(item)
                            .Reference(x => x.Ward)
                            .Load();
                        db.Entry(item)
                            .Reference(x => x.District)
                            .Load();
                        db.Entry(item)
                            .Reference(x => x.City)
                            .Load();
                        db.Entry(item)
                            .Reference(x => x.Poster)
                            .Load();
                        db.Entry(item)
                            .Collection(x => x.Images)
                            .Load();
                    }
                }
                return PartialView(result);
            }
        }

        [HttpGet]
        public ActionResult ApprovedPost()
        {
            try
            {
                int userid, usertype;
                if (Session["userid"] == null || Session["usertype"] == null || !int.TryParse(Session["userid"].ToString(), out userid) || !int.TryParse(Session["usertype"].ToString(), out usertype)) return PartialView("../Account/SignIn", new SignInModel());
                if (usertype != 1) return Content("<a href='/Account/SignOut'>Đăng nhập lại</a> với tài khoản admin để sử dụng chức năng này", "text/html");
                using (var db = new DBContext())
                {
                    var posts = (from p in db.Posts
                                 where p.Approved == 1
                                 orderby p.CreateTime descending
                                 select p).ToList();
                    foreach (var item in posts)
                    {
                        db.Entry(item)
                            .Reference(x => x.Poster)
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
                        db.Entry(item)
                            .Collection(x => x.Images)
                            .Load();
                        db.Entry(item)
                            .Collection(x => x.Tickets)
                            .Load();
                        item.ApprovalTicket = (from t in db.Tickets
                                               where t.PostId == item.Id && t.Approved == 1
                                               orderby t.ApprovalTime descending
                                               select t).FirstOrDefault();
                        if (item.ApprovalTicket != null)
                        {
                            db.Entry(item.ApprovalTicket)
                                .Reference(x => x.Approver)
                                .Load();
                        }
                    }
                    return PartialView(posts);
                }
            }
            catch (Exception ex) { return Content(ex.Message, "text/html"); }
        }

        public ActionResult RefusedPost()
        {
            int userid, usertype;
            if (Session["userid"] == null || Session["usertype"] == null || !int.TryParse(Session["userid"].ToString(), out userid) || !int.TryParse(Session["usertype"].ToString(), out usertype)) return PartialView("../Account/SignIn", new SignInModel());
            if (usertype != 1) return Content("<a href='/Account/SignOut'>Đăng nhập lại</a> với tài khoản admin để sử dụng chức năng này", "text/html");
            try
            {
                using (var db = new DBContext())
                {
                    var posts = (from p in db.Posts
                                 where p.Approved == -1
                                 select p).ToList();
                    foreach (var item in posts)
                    {
                        db.Entry(item)
                            .Reference(x => x.Ward)
                            .Load();
                        db.Entry(item)
                            .Reference(x => x.District)
                            .Load();
                        db.Entry(item)
                            .Reference(x => x.City)
                            .Load();
                        db.Entry(item)
                            .Reference(x => x.Poster)
                            .Load();
                        db.Entry(item)
                            .Collection(x => x.Images)
                            .Load();
                        item.ApprovalTicket = (from t in db.Tickets
                                               where t.PostId == item.Id
                                               orderby t.CreateTime descending
                                               select t).FirstOrDefault();
                        if (item.ApprovalTicket != null)
                        {
                            db.Entry(item.ApprovalTicket)
                                .Reference(x => x.Approver)
                                .Load();
                        }
                    }
                    return PartialView(posts);
                }
            }
            catch (Exception ex) { return Json(ex.Message); }
        }

        [HttpPost]
        public JsonResult UndoPost(int id)
        {
            int userid, usertype;
            if (Session["userid"] == null || Session["usertype"] == null || !int.TryParse(Session["userid"].ToString(), out userid) || !int.TryParse(Session["usertype"].ToString(), out usertype)) return Json("SignInFirst");
            if (usertype != 1) return Json("AdminOnly");
            try
            {
                using (var db = new DBContext())
                {
                    var post = (from p in db.Posts
                                where p.Id == id
                                select p).FirstOrDefault();
                    if (post == null) return Json("Không tìm thấy bài đăng tương ứng hoặc bài đăng đã bị xoá");
                    if (post.Approved != -1) return Json("Không thể hoàn tác bài đăng vì bài đăng đã được hoàn tác trước đó");
                    var ticket = (from t in db.Tickets
                                  where t.PostId == id
                                  orderby t.CreateTime descending
                                  select t).FirstOrDefault();
                    ticket.Approved = 0;
                    ticket.ApprovalTime = DateTime.MinValue;
                    ticket.ApproverId = userid;
                    post.Approved = 0;
                    db.SaveChanges();
                }
                return Json("Success");
            }
            catch (Exception ex) { return Json(ex.Message); }

        }
        public ActionResult ChartBasedOnTimePosting()
        {
            return PartialView();
        }
        public ActionResult RefusedAccount()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult ApprovedAccount()
        {
            int userid, usertype;
            if (Session["userid"] == null || Session["usertype"] == null || !int.TryParse(Session["userid"].ToString(), out userid) || !int.TryParse(Session["usertype"].ToString(), out usertype)) return PartialView("../Account/SignIn", new SignInModel());
            if (usertype != 1) return Content("<a href='/Account/SignOut'>Đăng nhập lại</a> với tài khoản admin để sử dụng chức năng này", "text/html");
            try
            {
                using (var db = new DBContext())
                {
                    var accounts = (from a in db.Accounts
                                    where a.Type == 2 && a.Approved == true
                                    select a).ToList();
                    foreach (var item in accounts)
                    {
                        db.Entry(item)
                            .Reference(x => x.Approver)
                            .Load();
                    }
                    return PartialView(accounts);
                }
            }
            catch (Exception ex) { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
        }

        [HttpPost]
        public JsonResult AuthorizeAccount(int id)
        {
            int userid, usertype;
            if (Session["userid"] == null || Session["usertype"] == null || !int.TryParse(Session["userid"].ToString(), out userid) || !int.TryParse(Session["usertype"].ToString(), out usertype)) return Json("SignInFirst");
            if (usertype != 1) return Json("AdminOnly");
            try
            {
                using (var db = new DBContext())
                {
                    var account = (from a in db.Accounts
                                   where a.Id == id
                                   select a).FirstOrDefault();
                    if (account == null) return Json("Không tìm thấy tài khoản tương ứng");
                    if (account.Type != 2 || account.EditRole) return Json("Tài khoản đã có quyền chỉnh sửa thông tin cá nhân hoặc bạn không thể thay đổi quyền chỉnh sửa thông tin cá nhân.");
                    account.EditRole = true;
                    db.SaveChanges();
                    return Json("Success");
                }
            }
            catch (Exception ex) { return Json(ex.Message); }
        }


        [HttpPost]
        public JsonResult UnauthorizeAccount(int id)
        {
            int userid, usertype;
            if (Session["userid"] == null || Session["usertype"] == null || !int.TryParse(Session["userid"].ToString(), out userid) || !int.TryParse(Session["usertype"].ToString(), out usertype)) return Json("SignInFirst");
            if (usertype != 1) return Json("AdminOnly");
            try
            {
                using (var db = new DBContext())
                {
                    var account = (from a in db.Accounts
                                   where a.Id == id
                                   select a).FirstOrDefault();
                    if (account == null) return Json("Không tìm thấy tài khoản tương ứng");
                    if (account.Type != 2) return Json("Bạn không thể thay đổi quyền chỉnh sửa thông tin cá nhân của tài khoản này.");
                    if (!account.EditRole || !account.Approved) return Json("Người dung đã thay đổi thông tin cá nhân hoặc quyền chỉnh sửa thông tin cá nhân đã được thu hồi trước đó");
                    account.EditRole = false;
                    db.SaveChanges();
                    return Json("Success");
                }
            }
            catch (Exception ex) { return Json(ex.Message); }
        }
    }
}