using System;
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
            return View();
        }

        [HttpGet]
        public ActionResult ApproveAccount()
        {
            using (var db = new DBContext())
            {
                var hosts = (from a in db.Accounts
                             where a.Type == 2 && a.Approved == false
                             select a).ToList();
                return PartialView(hosts);
            }
        }

        [HttpPost]
        public JsonResult ApproveAccount(int id)
        {
            int userid, usertype;
            if (Session["userid"] == null || int.TryParse(Session["userid"].ToString(), out userid)) return Json("SignInFirst");
            if (Session["usertype"] == null || !int.TryParse(Session["usertype"].ToString(), out usertype) || usertype != 1) return Json("AdminOnly");
            using (var db = new DBContext())
            {
                var account = (from a in db.Accounts
                               where a.Id == id
                               select a).FirstOrDefault();
                if (account.Approved || (account.Approved && account.ApprovalTime.Year > 1)) return Json("AccountProccessed");
                account.Approved = true;
                account.ApprovalTime = DateTime.Now;
                account.ApproverId = userid;
                db.SaveChanges();
                return Json("Successs");
            }
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
                              where t.Id == id
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
                if (ticket.Post.EndTime >= DateTime.Now && ticket.Post.Approved == 1 && ticket.Post.IsShow) return Json("PostShowed");
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
                              where t.Id == id
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
                return Json("Sucesss");
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
                return Json("Sucesss");
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
            return PartialView();
        }
    }
}