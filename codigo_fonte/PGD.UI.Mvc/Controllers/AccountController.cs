using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.WsFederation;
using Microsoft.Owin.Security;
using PGD.UI.Mvc.Helpers;


//using System.IdentityModel.Services;

namespace PGD.UI.Mvc.Controllers
{
    public class AccountController : Controller
    {

        public void SignIn()
        {
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/" },
                    WsFederationAuthenticationDefaults.AuthenticationType);
            }
        }

        
        public void SignOut()
        {
            Session["UserLogado"] = null;
            Session["CpfUsuarioForcado"] = null;
            Session.Abandon();
            string callbackUrl = Url.Action("Final", "Account", routeValues: null, protocol: Request.Url.Scheme);

            HttpContext.GetOwinContext().Authentication.SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                WsFederationAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);


            
            //FederatedAuthentication.WSFederationAuthenticationModule.SignOut(false);

            //string issuer = FederatedAuthentication.WSFederationAuthenticationModule.Issuer;

            //var urlBuilder = new System.UriBuilder(Request.Url.AbsoluteUri)
            //{
            //    Path = Url.Action("Final", "Account"),
            //    Query = null,
            //};

            //string returnUrl = urlBuilder.ToString();

            //var signOut = new SignOutRequestMessage(new Uri(issuer), returnUrl);

            // string urlFinal = signOut.WriteQueryString();


            //return this.Redirect(urlFinal);
 
        }


        public ActionResult Final()
        {
            return View();
        }

        //#region Escuro
        //public void setEscuro()
        //{
        //    if ((string)Session["escuro"] == "true")
        //    {
        //        Session["escuro"] = null;
        //    }
        //    else
        //    {
        //        Session["escuro"] = "true";
        //    }

        //}
        //#endregion
    }
}