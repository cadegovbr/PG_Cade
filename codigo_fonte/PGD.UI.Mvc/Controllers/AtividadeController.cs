using PGD.Application.Interfaces;
using PGD.Application.ViewModels;
using PGD.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PGD.UI.Mvc.Helpers;

namespace PGD.UI.Mvc.Controllers
{
    public class AtividadeController : BaseController
    {
        IAtividadeAppService _atividadeAppService;
        ITipoAtividadeAppService _tipoAtividadeAppService;

        public AtividadeController(IUsuarioAppService usuarioAppService, IAtividadeAppService atividadeservice, ITipoAtividadeAppService tipoatividadeservice)
            : base(usuarioAppService)
        {
            _atividadeAppService = atividadeservice;
            _tipoAtividadeAppService = tipoatividadeservice;
        }

        // GET: Atividade
        public ActionResult Index()
        {
            return View(_atividadeAppService.ObterTodos());
        }

        [HttpPost]
        public ActionResult Index(string NomAtividade)
        {
            var lista = _atividadeAppService.ObterTodos();

            if (!string.IsNullOrEmpty(NomAtividade))
            {
                lista = lista.Where(x => x.NomAtividade.ToUpper().Contains(NomAtividade.ToUpper())).ToList();
            }

            return View(lista);
        }

        public ActionResult Create(int? id)
        {
            AtividadeViewModel obj = new AtividadeViewModel();
            if (!id.HasValue)
            {
                obj = new AtividadeViewModel();
                obj.Usuario = getUserLogado();
                obj.Tipos = new List<TipoAtividadeViewModel>();
                obj.Tipos.Add(new TipoAtividadeViewModel { Excluir = false });
            }
            else
            {
                obj = _atividadeAppService.ObterPorId(id.Value);
                obj.Usuario = getUserLogado();

                obj.PctMinimoReducao = obj.PctMinimoReducao;
            }
            ViewBag.ListaUsuarios = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(_usuarioAppService.ObterTodos(obj.Usuario.Unidade).Select(a => a.Nome).ToList());

            return View(obj);
        }

        [HttpPost]
        public ActionResult Create(AtividadeViewModel atividadeViewModel)
        {
            for (int i = 0; i < atividadeViewModel.Tipos.Count; i++)
                if (atividadeViewModel.Tipos[i].Excluir)
                {
                    ModelState.Remove("Tipos[" + i + "].IdTipoAtividade");
                    ModelState.Remove("Tipos[" + i + "].Faixa");
                    ModelState.Remove("Tipos[" + i + "].DuracaoFaixa");
                    ModelState.Remove("Tipos[" + i + "].DuracaoFaixaPresencial");
                    ModelState.Remove("Tipos[" + i + "].Excluir");
                }

            if (ModelState.IsValid)
            {
                atividadeViewModel.Usuario = getUserLogado();

                if (atividadeViewModel.IdAtividade == 0)
                    atividadeViewModel = _atividadeAppService.Adicionar(atividadeViewModel);
                else
                    atividadeViewModel = _atividadeAppService.Atualizar(atividadeViewModel);

                if (atividadeViewModel.ValidationResult.IsValid)
                    return setMessageAndRedirect(atividadeViewModel.ValidationResult.Message, "Index");
                else
                    setModelErrorList(atividadeViewModel.ValidationResult.Erros);
            }
            return View(atividadeViewModel);
        }

        public PartialViewResult AddTipoAtividade(int pCount)
        {
            var modelTipo = new KeyValuePair<int, TipoAtividadeViewModel>(pCount, new TipoAtividadeViewModel());
            return PartialView("_TipoAtividadePartial", modelTipo);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpNotFoundResult();

            var obj = _atividadeAppService.ObterPorId(id.Value);
            if (obj == null)
                return new HttpNotFoundResult();

            obj.Usuario = getUserLogado();
            var atividadeReturn = _atividadeAppService.Remover(obj);

            return setMessageAndRedirect(atividadeReturn.ValidationResult.Message, "Index");
        }
    }
}