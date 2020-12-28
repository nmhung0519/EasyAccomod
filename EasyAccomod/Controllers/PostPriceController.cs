using EasyAccomod.Helpers;
using EasyAccomod.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyAccomod.Controllers
{
    public class PostPriceController : Controller
    {
        // GET: PostPrice
        public JsonResult Index(int TimeLength, int TimeUnit)
        {
            try
            {
                using (var db = new DBContext())
                {
                    int postPrice = (from pp in db.PostPrices
                                     where pp.Type == TimeUnit
                                     select pp.Price).FirstOrDefault();
                    int amount = TimeLength * postPrice;
                    
                    return Json(amount.ToVND(), JsonRequestBehavior.AllowGet);
                }
            }
            catch { return Json("Đã xảy ra lỗi trong quá trình kiểm tra số tiền phải thanh toán", JsonRequestBehavior.AllowGet); }
        }
    }
}