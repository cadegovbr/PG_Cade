using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PGD.UI.Mvc.Helpers;

namespace PGD.UI.Mvc.Controllers
{
    public class ErroController : Controller
    {
        // GET: ErroNaoMapeado
        public ActionResult Index()
        {
            // LogManager.LogErro(Server.GetLastError());
            Session["UserLogado"] = null;
            return View("Error");
        }

        public ActionResult UsuarioNaoEncontrado()
        {
            return View("UsuarioNaoEncontrado");
        }
        public ActionResult NaoAutorizado()
        {
            return View();
        }
    }
}