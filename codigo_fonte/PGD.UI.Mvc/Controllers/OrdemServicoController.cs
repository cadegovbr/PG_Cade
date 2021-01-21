using DomainValidation.Validation;
using PGD.Application.Interfaces;
using PGD.Application.ViewModels;
using PGD.Domain.Enums;
using PGD.UI.Mvc.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PGD.UI.Mvc.Controllers
{
    public class OrdemServicoController : BaseController
    {
        readonly IOrdemServicoAppService _OrdemServicoAppService;
        readonly IGrupoAtividadeAppService _GrupoAtividadeAppService;
        readonly ICriterioAvaliacaoAppService _CriterioAvaliacaoAppService;


        public OrdemServicoController(IUsuarioAppService usuarioAppService, IOrdemServicoAppService ordemServicoAppService, IGrupoAtividadeAppService grupoAtividadeAppService, ICriterioAvaliacaoAppService criterioAvaliacaoAppService)
            : base(usuarioAppService)
        {
            _OrdemServicoAppService = ordemServicoAppService;
            _GrupoAtividadeAppService = grupoAtividadeAppService;
            _CriterioAvaliacaoAppService = criterioAvaliacaoAppService;
        }


        // GET: OrdemServico
        public ActionResult Index(bool contemErro = false, string mensagem = null)
        {
            if (contemErro)
            {
                var lstErros = new List<ValidationError>() { new ValidationError(mensagem) };
                setModelErrorList(lstErros);
            }

            ViewBag.OrdemVigente = _OrdemServicoAppService.GetOrdemVigente();
            return View(_OrdemServicoAppService.ObterTodos());
        }

        [HttpPost]
        public ActionResult Index(SearchOrdemServicoViewModel obj)
        {
            var lista = _OrdemServicoAppService.ObterTodos();

            if (!string.IsNullOrEmpty(obj.Descricao))
            {
                lista = lista.Where(x => x.DescOrdemServico.ToUpper().Contains(obj.Descricao.ToUpper())).ToList();
            }
            if (obj.IdOrdemServico.HasValue && obj.IdOrdemServico.Value != 0)
            {
                lista = lista.Where(x => x.IdOrdemServico.ToString().Contains(obj.IdOrdemServico.ToString())).ToList();
            }
            if (DateTime.MinValue != obj.DataInicial)
            {
                lista = lista.Where(x => x.DatInicioNormativo >= obj.DataInicial || x.DatInicioSistema >= obj.DataInicial).ToList();
            }
            if (DateTime.MinValue != obj.DataFinal)
            {
                lista = lista.Where(x => x.DatInicioNormativo <= obj.DataFinal || x.DatInicioSistema <= obj.DataFinal).ToList();
            }
            ViewBag.OrdemVigente = _OrdemServicoAppService.GetOrdemVigente();
            return View(lista);
        }


        public ActionResult Create(int? id)
        {
            var osViewModel = new OrdemServicoViewModel();

            ViewBag.Grupos = new SelectList(_GrupoAtividadeAppService.ObterTodos(), "IdGrupoAtividade", "NomGrupoAtividade");
            ViewBag.CriteriosAvaliacao = new SelectList(_CriterioAvaliacaoAppService.ObterTodos(), "IdCriterioAvaliacao", "DescCriterioAvaliacao");

            if (!id.HasValue)
            {
                osViewModel.Grupos = new List<GrupoAtividadeViewModel>();
                osViewModel.CriteriosAvaliacao = new List<CriterioAvaliacaoViewModel>();
                osViewModel.DatInicioNormativo = DateTime.Now;
                osViewModel.DatInicioSistema = DateTime.Now;
            }
            else
            {
                osViewModel = _OrdemServicoAppService.ObterPorId(id.Value);
                
                osViewModel.idsGrupos = new List<int>();
                
                foreach (var obj in osViewModel.Grupos)
                    osViewModel.idsGrupos.Add(obj.IdGrupoAtividadeOriginal);

                osViewModel.idsCriteriosAvaliacao = new List<int>();

                foreach (var obj in osViewModel.CriteriosAvaliacao)
                    osViewModel.idsCriteriosAvaliacao.Add(obj.IdCriterioAvaliacaoOriginal);
            }

            return View(osViewModel);
        }

        [HttpPost]
        public ActionResult Create(OrdemServicoViewModel grupoatividadeViewModel)
        {
            if (ModelState.IsValid)
            {
                grupoatividadeViewModel.Usuario = getUserLogado();

                grupoatividadeViewModel.Grupos = _OrdemServicoAppService.PreencheListGrupoAtividade(grupoatividadeViewModel.idsGrupos);
                grupoatividadeViewModel.CriteriosAvaliacao = _OrdemServicoAppService.PreencheListCriterioAvaliacao(grupoatividadeViewModel.idsCriteriosAvaliacao);

                if (!grupoatividadeViewModel.IdOrdemServico.HasValue || grupoatividadeViewModel.IdOrdemServico.Value == 0)
                    grupoatividadeViewModel = _OrdemServicoAppService.Adicionar(grupoatividadeViewModel);
                else
                    grupoatividadeViewModel = _OrdemServicoAppService.Atualizar(grupoatividadeViewModel);

                if (grupoatividadeViewModel.ValidationResult.IsValid)
                    return setMessageAndRedirect(grupoatividadeViewModel.ValidationResult.Message, "Index");
                else
                    setModelErrorList(grupoatividadeViewModel.ValidationResult.Erros);
            }
            
            if (grupoatividadeViewModel.idsGrupos == null)
                    grupoatividadeViewModel.idsGrupos = new List<int>();

            if (grupoatividadeViewModel.idsCriteriosAvaliacao == null)
                grupoatividadeViewModel.idsCriteriosAvaliacao = new List<int>();

            ViewBag.Grupos = new SelectList(_GrupoAtividadeAppService.ObterTodos(), "IdGrupoAtividade", "NomGrupoAtividade");
            ViewBag.CriteriosAvaliacao = new SelectList(_CriterioAvaliacaoAppService.ObterTodos(), "IdCriterioAvaliacao", "DescCriterioAvaliacao");

            return View(grupoatividadeViewModel);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpNotFoundResult();

            var obj = _OrdemServicoAppService.ObterPorId(id.Value);
            if (obj == null)
                return new HttpNotFoundResult();

            obj.Usuario = getUserLogado();
            
            if (obj.DatInicioSistema <= DateTime.Today)
            {
                return setMessageAndRedirect(
                    "Index",
                    new RouteValueDictionary { 
                        { "mensagem", "Não é possível excluir uma OS com Data de início menor ou igual a data de hoje!" },
                        { "contemErro", true }
                    });
            }
             
            var atividadeReturn = _OrdemServicoAppService.Remover(obj);
            return setMessageAndRedirect(atividadeReturn.ValidationResult.Message, "Index");
        }

        public ActionResult GerarExcel()
        {
            var osvm = _OrdemServicoAppService.GetOrdemVigente();
            var ret = ToExcel(Response, ToDataTable(osvm), "RolAtividades.xls", 2);
            return View("");
        }


        public DataTable ToDataTable(OrdemServicoViewModel osvm)
        {
            DataTable dataTable = new DataTable("RolAtividades");

            //dados header
            dataTable.Columns.Add("Grupo de Atividades");
            dataTable.Columns.Add("Atividade");
            dataTable.Columns.Add("Faixa");
            dataTable.Columns.Add("Tempo de duração da atividade presencial");
            dataTable.Columns.Add("Tempo de duração da atividade no PGD");
            dataTable.Columns.Add("Percentual mínimo de redução");

            DataRow row;

            foreach (GrupoAtividadeViewModel g in osvm.Grupos.OrderBy(g => g.NomGrupoAtividade))
            {
                foreach (AtividadeViewModel a in g.Atividades.OrderBy(a => a.NomAtividade))
                {
                    foreach (TipoAtividadeViewModel t in a.Tipos.OrderBy(t => t.Faixa))
                    {
                        row = dataTable.NewRow();
                        row["Grupo de Atividades"] = g.NomGrupoAtividade;
                        row["Atividade"] = a.NomAtividade;
                        row["Faixa"] = t.FaixaTextoExplicativo;
                        row["Tempo de duração da atividade presencial"] = t.DuracaoFaixaPresencial;
                        row["Tempo de duração da atividade no PGD"] = t.DuracaoFaixa;
                        row["Percentual mínimo de redução"] = a.PctMinimoReducao + "%";
                        dataTable.Rows.Add(row);
                    }
                }
            }
            return dataTable;
        }

    }
}
