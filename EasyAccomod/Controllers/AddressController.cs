using EasyAccomod.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyAccomod.Controllers
{
    public class AddressController : Controller
    {
        [HttpGet]
        public JsonResult CityList()
        {
            using (var db = new DBContext())
            {
                var cities = (from c in db.Cities
                              select c).ToList();
                List<KeyNamePair> data = new List<KeyNamePair>();
                foreach(var item in cities)
                {
                    data.Add(new KeyNamePair(item.Id, item.CityName));
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult DistrictList(int id)
        {
            using (var db = new DBContext())
            {
                var districts = (from d in db.Districts
                              where d.CityId == id
                              orderby d.DistrictName ascending
                              select d).ToList();
                List<KeyNamePair> data = new List<KeyNamePair>();
                foreach (var item in districts)
                {
                    data.Add(new KeyNamePair(item.Id, item.DistrictName));
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult WardList(int id)
        {
            using (var db = new DBContext())
            {
                var wards = (from d in db.Wards
                                 where d.DistrictId == id
                                 orderby d.WardName ascending
                                 select d).ToList();
                List<KeyNamePair> data = new List<KeyNamePair>();
                foreach (var item in wards)
                {
                    data.Add(new KeyNamePair(item.Id, item.WardName));
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
    }
}