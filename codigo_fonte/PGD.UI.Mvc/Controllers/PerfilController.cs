using PGD.Application.Interfaces;
using PGD.Application.ViewModels;
using PGD.Domain.Enums;
using PGD.Domain.Interfaces.Service;
using PGD.UI.Mvc.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PGD.UI.Mvc.Controllers
{
    
    public class PerfilController : BaseController
    {

        public PerfilController(IUsuarioAppService usuarioAppService, IUnidadeService unidadeService)
            : base(usuarioAppService)
        {
            _unidadeService = unidadeService;
        }

        public ActionResult Selecionar(string UrlRedirect)
        {
            ViewBag.UrlRedirect = UrlRedirect;
            return View();
        }

        [HttpPost]
        public ActionResult Selecionar(Domain.Enums.Perfil? perfil, string UrlRedirect)
        {
            if (perfil == null)
                return View();

            var user = getUserLogado();
            if (! _usuarioAppService.PodeSelecionarPerfil(user))
            {
                ModelState.AddModelError("", @"Este usuário não pode selecionar perfil");
                return View();
            }

            user.Perfis = new List<Domain.Enums.Perfil>();
            user.Perfis.Add(perfil.Value);

            if (!String.IsNullOrEmpty(UrlRedirect))
                return Redirect(UrlRedirect);
            else
                return RedirectToAction("Index", "Pacto");
        }

        // GET: Perfil
        public ActionResult Index()
        {
            var lista = _usuarioAppService.ObterTodosAdministradores();
            return View(lista);
        }
        
        public JsonResult BuscarTodosUsuarios()
        {
            var listaUsuarios = new JavaScriptSerializer().Serialize(_usuarioAppService.ObterTodos().Select(x => new { x.Nome, x.CPF }).ToList());
            return Json(listaUsuarios, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Index(string txtPesquisaUsuario)
        {
            var user = getUserLogado();
            if (!string.IsNullOrEmpty(txtPesquisaUsuario))
            {
                var usuarioVM = _usuarioAppService.ObterPorNome(txtPesquisaUsuario);
                if (usuarioVM != null)
                {
                    usuarioVM.Usuario = getUserLogado();
                    usuarioVM.ValidationResult = new DomainValidation.Validation.ValidationResult();
                    usuarioVM = _usuarioAppService.TornarRemoverAdministrador(usuarioVM, true);
                    if (usuarioVM.ValidationResult.IsValid)
                        return setMessageAndRedirect(usuarioVM.ValidationResult.Message, "Index");
                    else
                        setModelErrorList(usuarioVM.ValidationResult.Erros);
                }
                else
                    ModelState.AddModelError("", @"Usuário não encontrado");
            }
            else
                ModelState.AddModelError("", string.Format(PGD.Domain.Constantes.Mensagens.ME_002, "Nome"));

            var lista = _usuarioAppService.ObterTodosAdministradores();
            return View(lista);
        }


        public ActionResult Delete(string id)
        {
            var cpf = id;
            var usuarioVM = _usuarioAppService.ObterPorCPF(cpf);
            usuarioVM.ValidationResult = new DomainValidation.Validation.ValidationResult();
            if (usuarioVM != null)
            {
                usuarioVM.Usuario = getUserLogado();
                usuarioVM = _usuarioAppService.TornarRemoverAdministrador(usuarioVM, false);
                if (usuarioVM.ValidationResult.IsValid)
                    return setMessageAndRedirect(usuarioVM.ValidationResult.Message, "Index");
                else
                    return setMessageAndRedirect(usuarioVM.ValidationResult.Erros, "Index");
            }
            else
                return setMessageAndRedirect("Usuário não encontrado", "Index");
        }
    }
}