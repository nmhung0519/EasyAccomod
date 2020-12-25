using EasyAccomod.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyAccomod.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(SearchModel model)
        {
            using (var db = new DBContext())
            {
                var posts = (from p in db.Posts
                             where ((model.KeyWord == null) || p.Title.Contains(model.KeyWord) || p.Content.Contains(model.KeyWord) || p.Address.Contains(model.KeyWord))
                                && (model.Type == 0 || p.Type == model.Type)
                                && ((model.CityId == 0) || ((p.WardId == model.WardId) || (model.WardId == 0 && (p.DistrictId == model.DistrictId || (model.DistrictId == 0 && p.CityId == model.CityId)))))
                             select p).ToList();
                if (posts != null) 
                    foreach (var item in posts)
                    {
                        db.Entry(item)
                            .Reference(x => x.Poster)
                            .Load();
                        db.Entry(item)
                                .Collection(x => x.Images)
                                .Load();
                    }
                return View("../Post/ListPost", new SearchResultModel(model, posts));
            }
        }

        public ActionResult GetByLocation(string city, string district, string ward)
        {
            try
            {
                if (city != null) city = city.Replace('-', ' ');
                if (district != null) district = district.Replace('-', ' ');
                if (ward != null) ward = ward.Replace('-', ' ');
                using (var db = new DBContext())
                {
                    
                    int cityId = (from c in db.Cities
                                  where c.CityName == "" + city
                                  select c.Id).FirstOrDefault();
                    int districtId = 0;
                    if (cityId != 0) districtId = (from d in db.Districts
                                      where (d.DistrictName == "" + district) && (d.CityId == cityId)
                                      select d.Id).FirstOrDefault();
                    int wardId = 0;
                    if (districtId != 0) wardId = (from w in db.Wards
                                  where (w.WardName == "" + ward) && (w.DistrictId == districtId)
                                  select w.Id).FirstOrDefault();
                    var posts = db.Posts.SqlQuery("CALL usp_post_ticket_getpostbylocation(" + cityId + "," + districtId + "," + wardId + ")").ToList<PostModel>();
                    if (posts != null)
                        foreach (var item in posts)
                        {
                            db.Entry(item)
                                .Reference(x => x.Poster)
                                .Load();
                            db.Entry(item)
                                .Collection(x => x.Images)
                                .Load();
                        }
                    return View("../Post/ListPost", new SearchResultModel(new SearchModel { CityId = cityId, DistrictId  = districtId, WardId = wardId }, posts));
                }
            }
            catch (Exception ex) { return Json(ex.Message, JsonRequestBehavior.AllowGet); }
        }

        [HttpGet]
        public ActionResult AdvancedSearch()
        {
            return View(new AdvancedSearchModel());
        }

        [HttpPost]
        public ActionResult AdvancedSearch(AdvancedSearchModel model)
        {
            return View("../Post/ListPost");
        }

        [HttpGet]
        public ActionResult NotificationBody()
        {
            int userid;
            if (Session["userid"] == null || !int.TryParse(Session["userid"].ToString(), out userid)) return null;
            using (var db = new DBContext())
            {
                var notifications = (from n in db.Notifications
                                     where n.ReceiverId == userid
                                     orderby n.CreatedTime descending
                                     select n).ToList();
                return PartialView(notifications);
            }
        }
    }
}