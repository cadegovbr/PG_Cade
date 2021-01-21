using System;
using PGD.Application.AutoMapper;
using System.Net;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace PGD.UI.Mvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfig.RegisterMappings();

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimsUtil.CPF;
#if DEBUG
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
#endif

        }
        
        void Session_Start(object sender, EventArgs e) 
        {
            if (Request.IsSecureConnection) Response.Cookies["ASP.NET_SessionID"].Secure = false;
        }

        protected void Application_EndRequest()
        {
            if (Context.Items["AjaxPermissionDenied"] is bool)
            {
                Context.Response.StatusCode = 401;
                Context.Response.End();
            }
        }
        

    }
}
