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
                             where ((model.KeyWord == null) || p.Title.Contains(model.KeyWord) || p.Content.Contains(model.KeyWord))
                                && (model.Type == 0 || p.Type == model.Type)
                                && ((model.CityId == 0) || ((p.WardId == model.WardId) || (model.WardId == 0 && (p.DistrictId == model.DistrictId || (model.DistrictId == 0 && p.CityId == model.CityId)))))
                             select p).ToList();
                if (posts != null) 
                    foreach (var item in posts)
                    {
                        db.Entry(item)
                            .Reference(x => x.Poster)
                            .Load();
                    }
                return View("../Post/ListPost", new SearchResultModel(model, posts));
            }
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
    }
}