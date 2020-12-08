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

        public ActionResult Search(string city, string district, string ward, string keyword)
        {
            if (city != null && city != "") city = city.Replace("-", " ");
            if (district != null && district != "") district = district.Replace("-", " ");
            if (ward != null && ward != "") ward = ward.Replace("-", " ");
            using (var db = new DBContext())
            {
                
            }
            return RedirectToAction("Index", "Home");
        }
    }
}