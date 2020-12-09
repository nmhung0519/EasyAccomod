using EasyAccomod.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyAccomod.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Signin()
        {
            return View(new SignInModel());
        }

        [HttpPost]
        public ActionResult SignIn(SignInModel model)
        {
            if (!ModelState.IsValid) return View(model);
            //Xử lý đăng nhập
            return View();
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View(new SignUpModel());
        }

        [HttpPost]
        public ActionResult SignUp(SignUpModel model)
        {
            if (!ModelState.IsValid) return (View(model));
            return View();
        }
    }
}