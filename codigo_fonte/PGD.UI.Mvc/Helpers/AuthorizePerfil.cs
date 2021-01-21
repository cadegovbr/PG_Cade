using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PGD.UI.Mvc.Helpers
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AuthorizePerfilAttribute : AuthorizeAttribute
    {
        public AuthorizePerfilAttribute(params object[] roles)
        {
            if (roles != null)
            {
                if (roles.Any(r => r.GetType().BaseType != typeof(Enum)))
                    throw new ArgumentException("roles");

                this.Roles = string.Join(",", roles.Select(r => Enum.GetName(r.GetType(), r)));
            }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }


        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            var urlHelper = new UrlHelper(filterContext.RequestContext);
            // check if session is supported  
            if (ctx.Session != null)
            {
                if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        filterContext.HttpContext.Response.ClearContent();
                        filterContext.HttpContext.Items["AjaxPermissionDenied"] = true;
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult(urlHelper.Action("Index", "Home"));
                    }
                } else
                {
                    filterContext.Result = new RedirectResult(urlHelper.Action("NaoAutorizado", "Erro"));
                }
            }
        }
    }
}
