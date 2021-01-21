using PGD.Application.Interfaces;
using PGD.Application.ViewModels;
using PGD.Domain.Enums;
using PGD.UI.Mvc.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace PGD.UI.Mvc.Controllers
{
    public class CriterioAvaliacaoController : BaseController
    {

        readonly ICriterioAvaliacaoAppService _criterioAvaliacaoAppService;
        readonly IItemAvaliacaoAppService _itemAvaliacaoAppService;
        readonly INotaAvaliacaoAppService _notaAvaliacaoAppService;

        public CriterioAvaliacaoController(IUsuarioAppService usuarioAppService, ICriterioAvaliacaoAppService criterioAvaliacaoService, IItemAvaliacaoAppService itemAvaliacaoService, INotaAvaliacaoAppService notaAvaliacaoAppService)
            : base(usuarioAppService)
        {
            _criterioAvaliacaoAppService = criterioAvaliacaoService;
            _itemAvaliacaoAppService = itemAvaliacaoService;
            _notaAvaliacaoAppService = notaAvaliacaoAppService;
        }

        // GET: Default
        public ActionResult Index()
        {
            return View(_criterioAvaliacaoAppService.ObterTodos());
        }

        [HttpPost]
        public ActionResult Index(string DescCriterioAvaliacao)
        {
            var lista = _criterioAvaliacaoAppService.ObterTodos();

            if (!string.IsNullOrEmpty(DescCriterioAvaliacao))
            {
                lista = lista.Where(x => x.DescCriterioAvaliacao.ToUpper().Contains(DescCriterioAvaliacao.ToUpper())).ToList();
            }

            return View(lista);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpNotFoundResult();

            var obj = _criterioAvaliacaoAppService.ObterPorId(id.Value);
            if (obj == null)
                return new HttpNotFoundResult();

            foreach (var item in obj.ItensAvaliacao)
            {
                var _item = _itemAvaliacaoAppService.ObterPorId(item.IdItemAvaliacao);

                if (_item != null)
                {
                    _item.Usuario = getUserLogado();
                    _itemAvaliacaoAppService.Remover(_item);
                }
            }   

            obj.Usuario = getUserLogado();
            var criterioAvaliacaoReturn = _criterioAvaliacaoAppService.Remover(obj);

            return setMessageAndRedirect(criterioAvaliacaoReturn.ValidationResult.Message, "Index");
        }

        public ActionResult Create(int? id)
        {
            CriterioAvaliacaoViewModel obj;

            if (!id.HasValue)
            {
                obj = new CriterioAvaliacaoViewModel
                {
                    Usuario = getUserLogado(),
                    ItensAvaliacao = new List<ItemAvaliacaoViewModel>()
                };

                obj.ItensAvaliacao.Add(new ItemAvaliacaoViewModel { Excluir = false });
            }
            else
            {
                obj = _criterioAvaliacaoAppService.ObterPorId(id.Value);
                obj.Usuario = getUserLogado();

            }

            PrepararTempDataDropdowns();

            return View(obj);
        }

        [HttpPost]
        public ActionResult Create(CriterioAvaliacaoViewModel criterioAvaliacaoViewModel)
        {
            for (int i = 0; i < criterioAvaliacaoViewModel.ItensAvaliacao.Count; i++)
                if (criterioAvaliacaoViewModel.ItensAvaliacao[i].Excluir)
                {
                    ModelState.Remove("ItensAvaliacao[" + i + "].IdItemAvaliacao");
                    ModelState.Remove("ItensAvaliacao[" + i + "].DescItemAvaliacao");
                    ModelState.Remove("ItensAvaliacao[" + i + "].ImpactoNota");
                    ModelState.Remove("ItensAvaliacao[" + i + "].IdNotaMaxima");
                    ModelState.Remove("ItensAvaliacao[" + i + "].Excluir");
                }

            if (ModelState.IsValid)
            {
                criterioAvaliacaoViewModel.Usuario = getUserLogado();

                if (criterioAvaliacaoViewModel.IdCriterioAvaliacao == 0)
                    criterioAvaliacaoViewModel = _criterioAvaliacaoAppService.Adicionar(criterioAvaliacaoViewModel);
                else
                    criterioAvaliacaoViewModel = _criterioAvaliacaoAppService.Atualizar(criterioAvaliacaoViewModel);

                if (criterioAvaliacaoViewModel.ValidationResult.IsValid)
                    return setMessageAndRedirect(criterioAvaliacaoViewModel.ValidationResult.Message, "Index");
                else
                    setModelErrorList(criterioAvaliacaoViewModel.ValidationResult.Erros);
            }

            PrepararTempDataDropdowns();

            return View(criterioAvaliacaoViewModel);
        }

        public PartialViewResult AddItemAvaliacao(int pCount)
        {
            var modelItemAva = new KeyValuePair<int, ItemAvaliacaoViewModel>(pCount, new ItemAvaliacaoViewModel());
            PrepararTempDataDropdowns();
            return PartialView("_ItemAvaliacaoPartial", modelItemAva);
        }        
        public void PrepararTempDataDropdowns()
        {
            TempData["lstNotasAvaliacao"] = _notaAvaliacaoAppService.ObterTodosPorNivelAvaliacao((int)eNivelAvaliacao.Detalhada);
        }

    }
}
