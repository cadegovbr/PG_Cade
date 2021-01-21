using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PGD.UI.Mvc.Filters
{
    public class LogExceptionHandlerAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            // if (filterContext.Exception != null)
            // {
            //     var msgErro = "";
            //     var exception = filterContext.Exception;
            //     while(exception != null)
            //     {
            //         msgErro += exception.Message + ";\n ";
            //         exception = exception.InnerException;
            //     }
            //     msgErro += "\n\nStackTrace: " + filterContext.Exception.StackTrace;
            //     // CGU.Util.LogManagerComum.LogarErro(exception, null, msgErro);
            // }

            base.OnException(filterContext);
        }
    }
}