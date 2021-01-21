using System.Web;
using System.Web.Mvc;

namespace PGD.Services.REST.SIGRHAPI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
