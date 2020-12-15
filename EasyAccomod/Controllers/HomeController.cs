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
                             where (p.Title.Contains(model.KeyWord) || p.Content.Contains(model.KeyWord))
                                && (model.Type == 0 || p.Type == model.Type)
                                && ((p.CityId == 0) || ((p.WardId == model.WardId) || (p.WardId == 0 && (p.DistrictId == model.DistrictId || (p.DistrictId == 0 && p.CityId == model.CityId)))))
                             select p).ToList();
                return View("SearchResult", new SearchResultModel(model, posts));
            }
        }
    }
}