using PGD.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PGD.Application.Interfaces;

namespace PGD.UI.Mvc.Controllers
{
    public class RelatorioConsolidadoController : BaseController
    {
        IRelatorioAppService _relatorioAppService;
        public RelatorioConsolidadoController(IUsuarioAppService usuarioAppService, IRelatorioAppService relatorioAppService) : base(usuarioAppService)
        {
            _relatorioAppService = relatorioAppService;
        }

        // GET: RelatorioConsolidado
        public ActionResult Index()
        {
            var rlConsolidado = new RelatorioConsolidadoViewModel();
            rlConsolidado.listaAtividadesPgd = new List<RelatorioAtividadesPgdViewModel>();
            for (int i = 0; i <= 6; i++)
            {
                var rlt = new RelatorioAtividadesPgdViewModel();

                switch (i)
                {
                    case 1:
                        rlt.nomeUnidade = "Secretaria-Executiva";

                        break;
                    case 2:
                        rlt.nomeUnidade = "Secretaria Federal de Controle Interno";
                        break;
                    case 3:
                        rlt.nomeUnidade = "Corregedoria-Geral da União";
                        break;
                    case 4:
                        rlt.nomeUnidade = "Ouvidoria-Geral da União";
                        break;
                    case 5:
                        rlt.nomeUnidade = "Secretaria de Transparência e Prevenção da Corrupção";
                        break;
                    case 6:
                        rlt.nomeUnidade = "Controladorias Regionais";
                        break;
                }
                if (i > 0)
                    rlConsolidado.listaAtividadesPgd.Add(rlt);

            }

            var relatorio = _relatorioAppService.RelatorioAtividadePgdPeriodo(rlConsolidado);

            return View(relatorio);
        }
        public ActionResult GerarRelatorioConsolidado(string datainicio, string datafim)
        {
            var rltConsolidado = new RelatorioConsolidadoViewModel();
            var dtInicio = new DateTime();
            var dtFim = new DateTime();
            if (datainicio != "")
            {
                dtInicio = Convert.ToDateTime(datainicio);
                rltConsolidado.DataInicial = dtInicio;
            }
            if (datafim != "")
            {
                dtFim = Convert.ToDateTime(datafim);
                rltConsolidado.DataFinal = dtFim;
            }

            var relatorio = _relatorioAppService.RelatorioAtividadePgdPeriodo(rltConsolidado);
            var teste = relatorio.listaAtividadesPgd.Sum(a => a.PercentEntreguePz);

            relatorio.TextoInicialData = "O presente relatório contempla as atividades realizadas no Programa de Gestão de Demandas - PGD no período de " + datainicio + " a " + datafim;

            // return new Rotativa.MVC.PartialViewAsPdf("_RelatorioConsolidado", relatorio) { FileName = "TestPartialViewAsPdf.pdf" };
            //return new Rotativa.MVC.PartialViewAsPdf("http://www.Google.com") { FileName = "UrlTest.pdf" };

            //GerarPDF(relatorio);
            return PartialView("_RelatorioConsolidado", relatorio);

        }

    }
}