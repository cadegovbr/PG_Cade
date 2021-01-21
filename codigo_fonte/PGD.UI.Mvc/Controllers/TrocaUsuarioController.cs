using PGD.Application.Interfaces;
using PGD.Application.ViewModels;
using System.Web.Mvc;

namespace PGD.UI.Mvc.Controllers
{
    public class TrocaUsuarioController : BaseController
    {
        IUsuarioAppService _Usuarioservice;

        public TrocaUsuarioController(IUsuarioAppService usuarioAppService) : base(usuarioAppService)
        {
            _Usuarioservice = usuarioAppService;
        }

        // GET: TrocaUsuario
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Troca(UsuarioViewModel user)
        {
            Session["UserLogado"] = null;
            Session["CpfUsuarioForcado"] = user.CpfUsuario;
            return RedirectToAction("Index", "Home");
        }
    }
}