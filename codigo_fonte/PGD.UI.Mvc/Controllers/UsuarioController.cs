using CsQuery.ExtensionMethods;
using PGD.Application.Interfaces;
using PGD.Application.Util;
using PGD.Application.ViewModels;
using PGD.Application.ViewModels.Filtros;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PGD.Domain.Enums;
using PGD.Domain.Entities;

namespace PGD.UI.Mvc.Controllers
{
    public class UsuarioController : BaseController
    {
        public UsuarioController(IUsuarioAppService usuarioAppService) : base(usuarioAppService)
        {
        }

        public ActionResult Index()
        {
            return View(new SearchUsuarioViewModel());
        }

        [HttpPost]
        public ActionResult Index(SearchUsuarioViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { camposNaoPreenchidos = RetornaErrosModelState() });
            var filtro = new UsuarioFiltroViewModel
            {
                Nome = model.NomeUsuario, Matricula = model.MatriculaUsuario, IdUnidade = model.IdUnidade, IncludeUnidadesPerfis = true,
                Take = model.Take, Skip = model.Skip
            };
            var usuarios = _usuarioAppService.Buscar(filtro);
            usuarios.Lista.ForEach(x => x.CPF = x.CPF.MaskCpfCpnj());
            return Json(usuarios);
        }

        [HttpGet]
        public ActionResult VincularPerfil(int id)
        {
            var usuario = _usuarioAppService.Buscar(new UsuarioFiltroViewModel
            {
                Id = id
            }).Lista.FirstOrDefault();

            PrepararTempDataPerfis();

            return View(new VincularPerfilUsuarioViewModel { 
                Cpf = usuario.CPF.MaskCpfCpnj(),
                Matricula = usuario.Matricula,
                Nome = usuario.Nome,
                IdUsuario = usuario.IdUsuario
            });
        }

        [HttpPost]
        public ActionResult VincularPerfil(VincularPerfilUsuarioViewModel model)
        {
            if (!ModelState.IsValid) return Json(new { camposNaoPreenchidos = RetornaErrosModelState() });

            var usuarioLogado = getUserLogado();
            var retorno = _usuarioAppService.VincularUnidadePerfil(model, usuarioLogado.CPF);

            return Json(retorno);
        }

        [HttpPost]
        public ActionResult ExcluirVinculo(long idUsuarioPerfilUnidade)
        {
            var usuarioLogado = getUserLogado();
            var retorno = _usuarioAppService.RemoverVinculoUnidadePerfil(idUsuarioPerfilUnidade, usuarioLogado.CPF);
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult BuscarUnidadesPerfisUsuario(int idUsuario, int take, int skip)
        {
            var retorno = _usuarioAppService.BuscarPerfilUnidade(new UsuarioPerfilUnidadeFiltroViewModel
            {
                IdUsuario = idUsuario,
                Skip = skip,
                Take = take,
                OrdenarDescendente = true
            });
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuscarUnidadesPorNomeOuSigla(string query)
        {
            var retorno = _usuarioAppService.BuscarUnidades(new UnidadeFiltroViewModel {NomeOuSigla = query});
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        private void PrepararTempDataPerfis()
        {
            var lista = new List<PerfilViewModel>
            {
                new PerfilViewModel { Nome = Domain.Enums.Perfil.Administrador.ToString(), IdPerfil = Domain.Enums.Perfil.Administrador.GetHashCode() },
                new PerfilViewModel { Nome = Domain.Enums.Perfil.Dirigente.ToString(), IdPerfil = Domain.Enums.Perfil.Dirigente.GetHashCode() }
            };
            TempData["lstPerfil"] = lista;
        }
    }
}