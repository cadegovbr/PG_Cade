using PGD.Services.REST.SIGRHAPI.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace PGD.Services.REST.SIGRHAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Filters.Add(new LogExceptionHandlerAttribute());
            AreaRegistration.RegisterAllAreas();
        }
    }
}
