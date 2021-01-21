using PGD.Application.Interfaces;
using PGD.Application.ViewModels;
using PGD.Domain.Interfaces.Service;
using System;
using System.Linq;
using System.Web.Mvc;
using PGD.Application.ViewModels.Filtros;
using PGD.Domain.Constantes;

namespace PGD.UI.Mvc.Controllers
{
    public class UnidadeTipoPactoController : BaseController
    {
        IUnidade_TipoPactoAppService unidadeTipoPactoAppService;
        ITipoPactoService tipoPactoService;

        public UnidadeTipoPactoController(IUsuarioAppService usuarioAppService, IUnidade_TipoPactoAppService unidadeTipoPactoAppService,
            IUnidadeService unidadeService, ITipoPactoService tipoPactoService) : base(usuarioAppService)
        {
            _unidadeService = unidadeService;
            this.tipoPactoService = tipoPactoService;
            this.unidadeTipoPactoAppService = unidadeTipoPactoAppService;
        }

        // GET: UnidadeTipoPacto
        public ActionResult Index()
        {
            PrepararTempDataDropdowns();

            return View(unidadeTipoPactoAppService.ObterTodos());
        }

        [HttpPost]
        public ActionResult Index(SearchUnidadeTipoPactoViewModel model)
        {
            var retorno = unidadeTipoPactoAppService.Buscar(new UnidadeTipoPactoFiltroViewModel
            {
                IncludeTipoPacto = true, IncludeUnidade = true, IdUnidade = model.IdUnidade, IdTipoPacto = model.IdTipoPacto
                , Skip = model.Skip, Take = model.Take
            });

            return Json(retorno);
        }

        public ActionResult Create(int? id)
        {
            PrepararTempDataDropdowns();

            Unidade_TipoPactoViewModel obj = new Unidade_TipoPactoViewModel();

            if (!id.HasValue)
                obj.IdUnidade_TipoPacto = 0;
            else
                obj = unidadeTipoPactoAppService.ObterTodos().Where(a => a.IdUnidade_TipoPacto == id).FirstOrDefault();

            return View(obj);
        }

        [HttpPost]
        public ActionResult Create(Unidade_TipoPactoViewModel model)
        {
            PrepararTempDataDropdowns();
            var user = getUserLogado();

            if (ModelState.IsValid)
            {
                if(model.IdUnidade_TipoPacto == 0)
                    unidadeTipoPactoAppService.Adicionar(model, user);
                else
                    unidadeTipoPactoAppService.Atualizar(model, user);
                return setMessageAndRedirect(Mensagens.MS_003, "Index");
            }
            return View(model);
            
        }

        [HttpPost]
        public ActionResult Delete(int idUnidadeTipoPacto)
        {
            if (idUnidadeTipoPacto == 0)
                return Json(false, JsonRequestBehavior.AllowGet);

            var user = getUserLogado();
            unidadeTipoPactoAppService.Remover(idUnidadeTipoPacto, user);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public void PrepararTempDataDropdowns()
        {
            TempData["lstUnidade"] = _unidadeService.ObterUnidades().ToList();
            TempData["lstTipos"] = tipoPactoService.ObterTodos().ToList();
        }
    }
}