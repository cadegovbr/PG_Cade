using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace PGD.Services.REST.SIGRHAPI.Filter
{
    public class LogExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                var msgErro = "";
                var exception = context.Exception;
                while (exception != null)
                {
                    msgErro += exception.Message + "; ";
                    exception = exception.InnerException;
                }

                logger.Error(msgErro);
            }

            // log error to NLog
            base.OnException(context);
        }
    }

}