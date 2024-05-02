using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CRUD_API
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "UpdateMatch",
               url: "BCSeries/UpdateMatch/{id}",
               defaults: new { controller = "BCSeries", action = "UpdateMatch", Id = UrlParameter.Optional }
               );

            routes.MapRoute(
             name: "DeleteMatch",
             url: "BCSeries/DeleteMatch/{id}",
             defaults: new { controller = "BCSeries", action = "DeleteMatch", Id = UrlParameter.Optional }
             );

            routes.MapRoute(
              name: "UpdateSeries",
              url: "BCSeries/UpdateSeries/{id}",
              defaults: new { controller = "BCSeries", action = "UpdateSeries", Id = UrlParameter.Optional }
              );

            routes.MapRoute(
             name: "DeleteSeries",
             url: "BCSeries/DeleteSeries/{id}",
             defaults: new { controller = "BCSeries", action = "DeleteSeries", Id = UrlParameter.Optional }
             );

            routes.MapRoute(
                name: "Index",
                url: "BCSeries/Index",
                defaults: new { controller = "BCSeries", action = "Index" }
                );

            routes.MapRoute(
               name: "SearchCity",
               url: "Home/SearchCity/{city}",
               defaults: new { controller = "Home", action = "SearchCity", city = UrlParameter.Optional }
           );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
