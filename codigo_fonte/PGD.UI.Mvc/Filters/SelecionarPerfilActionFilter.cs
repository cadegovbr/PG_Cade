using PGD.Application.Interfaces;
using PGD.Application.ViewModels;
using PGD.UI.Mvc.Controllers;
using System.Web.Mvc;

namespace PGD.UI.Mvc.Filters
{
    public class SelecionarPerfilActionFilter: ActionFilterAttribute, IActionFilter
    {
        protected readonly IUsuarioAppService _usuarioAppService;

 

        void IActionFilter.OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = (BaseController)filterContext.Controller;
            var user = controller.getUserLogado();
 
            if (user != null && user.IdUsuario != 0  )
            {
                if (PodeSelecionarPerfil(user))
                {
                    filterContext.Result = controller.RedirectToAction("Selecionar", "Perfil", new { UrlRedirect = filterContext.HttpContext.Request.Url });
                }
            }
            else
            {
                filterContext.Result = controller.RedirectToAction("Index", "Home", null);
            }

        }

        private bool PodeSelecionarPerfil(UsuarioViewModel usuario)
        {
            return (usuario.Perfis.Count > 1 &&
                usuario.Perfis.Contains(Domain.Enums.Perfil.Dirigente) &&   
                usuario.Perfis.Contains(Domain.Enums.Perfil.Solicitante));

        }
    }
}