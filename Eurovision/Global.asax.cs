using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Eurovision.Helpers;

namespace Eurovision
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("CheckMy",
                    "CheckMy/Boolean/{type}/{id}",
                    new { controller = "CheckMy", action = "Boolean" },
                    new { type = @"\D{1,20}", id = @"\d{1,6}" });
            //routes.MapRoute(
            //    "AllocateItem", // Route name
            //    "Correspondence/Allocate", // URL with parameters
            //    new { controller = "Correspondence", action = "Create" }
            //    , new { CorrespondenceID = @"\d{1,6}" , StageID = @"\d{1,6}"}
            //);
            //routes.MapRoute("Audit", "{auditType}/Audit/{id}"
            //        , new
            //        {
            //            controller = "Audit",
            //            action = "Audit",
            //        }
            //        , new
            //        {
            //            auditType=@"\D{1,20}",
            //            id=@"\d{1,6}"
            //        });
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }, 
                // Add the namespace of your desktop controllers here
                new[] { "Eurovision.Controllers" } 
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalFilters.Filters.Add(new RedirectMobileDevicesToMobileAreaAttribute(), 1);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}