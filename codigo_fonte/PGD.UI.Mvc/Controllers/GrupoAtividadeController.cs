using PGD.Application.Interfaces;
using PGD.Application.ViewModels;
using PGD.Domain.Enums;
using PGD.UI.Mvc.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PGD.UI.Mvc.Controllers
{
    public class GrupoAtividadeController : BaseController
    {
        IGrupoAtividadeAppService _GrupoAtividadeservice;
        IAtividadeAppService _Atividadeservice;
        ITipoPactoAppService _TipoPactoService;

        public GrupoAtividadeController(IUsuarioAppService usuarioAppService, IGrupoAtividadeAppService grupoAtividadeservice, 
            IAtividadeAppService atividadeservice, ITipoPactoAppService tipoPactoAppService)
            : base(usuarioAppService)
        {
            _GrupoAtividadeservice = grupoAtividadeservice;
            _Atividadeservice = atividadeservice;
            _TipoPactoService = tipoPactoAppService;
        }

        // GET: GrupoAtividade
        public ActionResult Index()
        {
            return View(_GrupoAtividadeservice.ObterTodos());
        }

        [HttpPost]
        public ActionResult Index(string NomGrupoAtividade)
        {
            var lista = _GrupoAtividadeservice.ObterTodos();

            if (!string.IsNullOrEmpty(NomGrupoAtividade))
            {
                lista = lista.Where(x => x.NomGrupoAtividade.ToUpper().Contains(NomGrupoAtividade.ToUpper())).ToList();
            }

            return View(lista);
        }

        public ActionResult Create(int? id)
        {
            var grupoatividadeViewModel = new GrupoAtividadeViewModel();

            ViewBag.Atividades = new SelectList(_Atividadeservice.ObterTodos(), "IdAtividade", "NomAtividade");
            ViewBag.TiposPacto = _TipoPactoService.ObterTodos().ToList();

            if (!id.HasValue)
            {
                grupoatividadeViewModel.Atividades = new List<AtividadeViewModel>();
                grupoatividadeViewModel.IdsTipoPacto = new List<int> { (int)eTipoPacto.PGD_Pontual };
            }
            else
            {
                grupoatividadeViewModel = _GrupoAtividadeservice.ObterPorId(id.Value);
                grupoatividadeViewModel.idsAtividades = new List<int>();
                foreach (var obj in grupoatividadeViewModel.Atividades)
                    grupoatividadeViewModel.idsAtividades.Add(obj.IdAtividade);

                grupoatividadeViewModel.IdsTipoPacto = grupoatividadeViewModel.TiposPacto.Select(t => t.IdTipoPacto).ToList();
            }
            return View(grupoatividadeViewModel);
        }

        [HttpPost]
        public ActionResult Create(GrupoAtividadeViewModel grupoatividadeViewModel)
        {
            if (ModelState.IsValid)
            {
                grupoatividadeViewModel.Usuario = getUserLogado();

                grupoatividadeViewModel.Atividades = _GrupoAtividadeservice.PreencheList(grupoatividadeViewModel.idsAtividades);
                grupoatividadeViewModel.TiposPacto = _GrupoAtividadeservice.PreencheListTipoPacto(grupoatividadeViewModel.IdsTipoPacto);

                if (grupoatividadeViewModel.IdGrupoAtividade == 0)
                    grupoatividadeViewModel = _GrupoAtividadeservice.Adicionar(grupoatividadeViewModel);
                else
                    grupoatividadeViewModel = _GrupoAtividadeservice.Atualizar(grupoatividadeViewModel);

                if (grupoatividadeViewModel.ValidationResult.IsValid)
                    return setMessageAndRedirect(grupoatividadeViewModel.ValidationResult.Message, "Index");
                else
                    setModelErrorList(grupoatividadeViewModel.ValidationResult.Erros);
            }
            else
            {
                if (grupoatividadeViewModel.idsAtividades == null)
                    grupoatividadeViewModel.idsAtividades = new List<int>();
                ViewBag.Atividades = new SelectList(_Atividadeservice.ObterTodos(), "IdAtividade", "NomAtividade");
                ViewBag.TiposPacto = _TipoPactoService.ObterTodos().ToList();

            }
            return View(grupoatividadeViewModel);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpNotFoundResult();

            var obj = _GrupoAtividadeservice.ObterPorId(id.Value);
            if (obj == null)
                return new HttpNotFoundResult();

            obj.Usuario = getUserLogado();
            var atividadeReturn = _GrupoAtividadeservice.Remover(obj);

            return setMessageAndRedirect(atividadeReturn.ValidationResult.Message, "Index");
        }
    }
}