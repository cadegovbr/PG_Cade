using PGD.UI.Mvc.Filters;
using System.Web;
using System.Web.Mvc;

namespace PGD.UI.Mvc
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LogExceptionHandlerAttribute()
            {
                View = "ErroNaoMapeado"
            });
        }
    }
}
