using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Security;
using System.Security.Permissions;
using SignalR;

namespace Chat
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
            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }// Parameter defaults
            );

        }

        protected void Application_Start()
        {
            RouteTable.Routes.MapHubs("~/signalr");

            AreaRegistration.RegisterAllAreas();

            SetAppDomainData();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        [SecuritySafeCritical]
        public static void SetAppDomainData()
        {
            SecurityPermission sp = new SecurityPermission(SecurityPermissionFlag.ControlAppDomain);
            sp.Assert();

            AppDomain.CurrentDomain.SetData("SQLServerCompactEditionUnderWebHosting", true);
        }
    }
}