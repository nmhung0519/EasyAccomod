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

        public ActionResult ApproveComment()
        {
            return PartialView();
        }

        public ActionResult ApprovePost()
        {
            return PartialView();
        }

        public ActionResult ApprovePurchase()
        {
            return PartialView();
        }
    }
}