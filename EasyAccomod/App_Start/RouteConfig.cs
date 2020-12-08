﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace EasyAccomod
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "City Search",
                url: "{city}/Search",
                defaults: new { controller = "Post", action = "Search" }
            );
            routes.MapRoute(
                name: "District Search",
                url: "{city}/{district}/Search",
                defaults: new { controller = "Post", action = "Search" }
            );
            routes.MapRoute(
                name: "Ward Search",
                url: "{city}/{district}/{ward}/Search",
                defaults: new { controller = "Post", action = "Search" }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
