using PGD.Application.Interfaces;
using PGD.Application.ViewModels;
using PGD.Domain.Interfaces.Service;
using PGD.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGD.Application
{
    public class RelatoriosAppService : ApplicationService, IRelatorioAppService
    {
        private readonly IPactoAppService _pactoService;
        private readonly IProdutoService _produtoService;
        private readonly IGrupoAtividadeAppService _grupoAtividadeService;
        private readonly IUnidadeService _unidadeService;
        private readonly IUsuarioAppService _Usuarioservice;

        public RelatoriosAppService(IUsuarioAppService usuarioAppService, IPactoAppService pactoService, IProdutoService produtoService, IGrupoAtividadeAppService grupoAtividadeService, IUnidadeService unidadeService, IUnitOfWork uow)
            : base(uow)
        {
            _pactoService = pactoService;
            _produtoService = produtoService;
            _grupoAtividadeService = grupoAtividadeService;
            _unidadeService = unidadeService;
            _Usuarioservice = usuarioAppService;
        }
        public RelatorioConsolidadoViewModel RelatorioAtividadePgdPeriodo(RelatorioConsolidadoViewModel obj)
        {
            var rltConsolidado = new RelatorioConsolidadoViewModel();
            var rltAtividadesPgd = new RelatorioAtividadesPgdViewModel();
            rltConsolidado.listaAtividadesPgd = new List<RelatorioAtividadesPgdViewModel>();
            rltConsolidado.listProdutosPgd = new List<ProdutoViewModel>();
            rltConsolidado.listGrupoAtividades = new List<GrupoAtividadeViewModel>();

            var lisPactos = _pactoService.ObterTodos().Where(a => a.DataPrevistaInicio >= obj.DataInicial && a.DataPrevistaInicio <= obj.DataFinal).ToList();

            //Secretaria-Executiva
            var pctScExec = lisPactos.Where(p => p.UnidadeExercicio == 10).ToList();
            rltConsolidado.listaAtividadesPgd.Add(RetornaAtividadePGD(pctScExec, "Secretaria-Executiva", 1));

            //Secretaria Federal de Controle Interno
            var pctSFCI = lisPactos.Where(p => p.UnidadeExercicio == 59).ToList();
            rltConsolidado.listaAtividadesPgd.Add(RetornaAtividadePGD(pctSFCI, "Secretaria Federal de Controle Interno", 2));

            //Corregedoria-Geral da União
            var pctCGU = lisPactos.Where(p => p.UnidadeExercicio == 191).ToList();
            rltConsolidado.listaAtividadesPgd.Add(RetornaAtividadePGD(pctCGU, "Corregedoria-Geral da União", 3));

            //Ouvidoria-Geral da União
            var pctOGU = lisPactos.Where(p => p.UnidadeExercicio == 183).ToList();
            rltConsolidado.listaAtividadesPgd.Add(RetornaAtividadePGD(pctOGU, "Ouvidoria-Geral da União", 4));

            //Secretaria de Transparência e Prevenção da Corrupção
            var pctSTPC = lisPactos.Where(p => p.UnidadeExercicio == 216).ToList();
            rltConsolidado.listaAtividadesPgd.Add(RetornaAtividadePGD(pctSTPC, "Secretaria de Transparência e Prevenção da Corrupção", 5));

            //Controladorias Regionais
            int[] idListUnidades = { 228, 231, 234, 237, 240, 243, 246, 249, 252, 255, 258, 261, 262, 263, 264, 269, 275, 280, 285, 292, 297, 302, 307, 312, 318, 325 };
            var pctCR = lisPactos.Where(p => idListUnidades.Contains(p.UnidadeExercicio)).ToList();
            rltConsolidado.listaAtividadesPgd.Add(RetornaAtividadePGD(pctCR, "Controladorias Regionais", 6));

            foreach (var pct in lisPactos)
            {

                var listIntenaPdrt = _produtoService.ObterTodos(pct.IdPacto).Where(a => a.Avaliacao != 0 && a.Avaliacao != 6);
                foreach (var itemPdrt in listIntenaPdrt)
                {
                    var pdrt = new ProdutoViewModel();
                    pdrt.GrupoAtividade = new GrupoAtividadeViewModel();
                    pdrt.Atividade = new AtividadeViewModel();

                    pdrt.GrupoAtividade.NomGrupoAtividade = itemPdrt.GrupoAtividade.NomGrupoAtividade;
                    pdrt.Atividade.NomAtividade = itemPdrt.Atividade.NomAtividade;
                    pdrt.NomeFaixa = itemPdrt.TipoAtividade.Faixa;
                    pdrt.Avaliacao = itemPdrt.Avaliacao;
                    pdrt.IdTipoAtividade = itemPdrt.IdTipoAtividade;
                    pdrt.IdGrupoAtividade = itemPdrt.IdGrupoAtividade;
                    pdrt.IdAtividade = itemPdrt.IdAtividade;

                    rltConsolidado.listProdutosPgd.Add(pdrt);
                }
            }
            rltConsolidado.listGrupoAtividades = _grupoAtividadeService.ObterTodos().ToList();

            return rltConsolidado;
        }
        public RelatorioAtividadesPgdViewModel RetornaAtividadePGD(List<PactoViewModel> listPct, string nomeUnidade, int ordem)
        {
            var rltPgd = new RelatorioAtividadesPgdViewModel();
            var listaCpf = new List<string>();
            rltPgd.IdRltAtividadePgd = ordem;
            rltPgd.nomeUnidade = nomeUnidade;
            rltPgd.QtdPctCelebrados = listPct.Count();
            foreach (var nPct in listPct)
            {
                var cpfInserir = nPct.CpfUsuario;
                if (!listaCpf.Contains(cpfInserir))
                {
                    listaCpf.Add(cpfInserir);
                }
            }
            rltPgd.QtdServPgd = listaCpf.Count();
            rltPgd.QtdPctEntreguePrazo = listPct.Where(a => a.EntregueNoPrazo == 1).ToList().Count();
            rltPgd.PercentEntreguePz = rltPgd.QtdPctEntreguePrazo > 0 ? (rltPgd.QtdPctEntreguePrazo / rltPgd.QtdPctCelebrados) * 100 : 0;

            return rltPgd;
        }
        public RelatorioAlSimultaneaViewModel RetornaRelatorioAlocacaoSimultanea(SearchFlPontoViewModel searchFl)
        {
            var rltAlcSilmu = new RelatorioAlSimultaneaViewModel();
            rltAlcSilmu.searchSimultanea = new SearchFlPontoViewModel();
            rltAlcSilmu.lstPactos = new List<PactoViewModel>();
            rltAlcSilmu.listDates = new List<DateTime>();
            rltAlcSilmu.lisQtdServidor = new List<QuantidadeServidorViewModel>();
            var lisIntUnidades = new List<int>();


            var pct = new PactoViewModel();
            ObterUnidadesEPactosRelatorioAlocacaoSimultanea(searchFl, rltAlcSilmu, pct);

            if (searchFl.UnidadeId == null || searchFl.UnidadeId == 0)
            {
                FiltrarRelatorioAlocacaoSimultaneaPorUnidade(rltAlcSilmu, lisIntUnidades);
            }
            else
            {
                var qtdServidor = new QuantidadeServidorViewModel();
                qtdServidor.idUnidade = searchFl.UnidadeId.Value;
                qtdServidor.qtdServidorUnidade = _Usuarioservice.ObterTodos(searchFl.UnidadeId.Value).Count();
                rltAlcSilmu.lisQtdServidor.Add(qtdServidor);
            }

            CalcularDatasRelatorioAlocacaoSimultanea(rltAlcSilmu);
            return rltAlcSilmu;
        }

        private void CalcularDatasRelatorioAlocacaoSimultanea(RelatorioAlSimultaneaViewModel rltAlcSilmu)
        {
            DateTime nvData;
            if (rltAlcSilmu.lstPactos.Count > 0)
            {
                var minDate = rltAlcSilmu.lstPactos.Select(b => b.DataPrevistaInicio).Min();
                var maxDate = rltAlcSilmu.lstPactos.Select(b => b.DataPrevistaTermino).Max();

                nvData = minDate;
                rltAlcSilmu.listDates.Add(nvData);
                do
                {
                    nvData = nvData.AddDays(1);
                    rltAlcSilmu.listDates.Add(nvData);

                } while (nvData < maxDate);

            }
        }

        private void FiltrarRelatorioAlocacaoSimultaneaPorUnidade(RelatorioAlSimultaneaViewModel rltAlcSilmu, List<int> lisIntUnidades)
        {
            foreach (var pctItem in rltAlcSilmu.lstPactos)
            {
                if (lisIntUnidades.Count > 0)
                {
                    if (!lisIntUnidades.Contains(pctItem.UnidadeExercicio))
                        lisIntUnidades.Add(pctItem.UnidadeExercicio);
                }
                else
                    lisIntUnidades.Add(pctItem.UnidadeExercicio);
            }
            if (lisIntUnidades.Count > 0)
            {
                foreach (var undItem in lisIntUnidades)
                {
                    var qtdServidor = new QuantidadeServidorViewModel();
                    qtdServidor.idUnidade = undItem;
                    qtdServidor.qtdServidorUnidade = _Usuarioservice.ObterTodos(undItem).ToList().Count();
                    rltAlcSilmu.lisQtdServidor.Add(qtdServidor);
                }
            }
        }

        private void ObterUnidadesEPactosRelatorioAlocacaoSimultanea(SearchFlPontoViewModel searchFl, RelatorioAlSimultaneaViewModel rltAlcSilmu, PactoViewModel pct)
        {
            if (searchFl.DataInicial != null)
                pct.DataPrevistaInicio = searchFl.DataInicial.Value;
            if (searchFl.DataFinal != null)
                pct.DataPrevistaTermino = searchFl.DataFinal.Value;
            if (searchFl.UnidadeId != null)
                pct.UnidadeExercicio = searchFl.UnidadeId.Value;
            else
            {
                rltAlcSilmu.searchSimultanea.UnidadeId = 0;
                pct.UnidadeExercicio = 0;
            }

            rltAlcSilmu.searchSimultanea.lstUnidade = _unidadeService.ObterUnidades().ToList();
            rltAlcSilmu.lstPactos = _pactoService.ObterTodos(pct, false).Where(a => a.SituacaoPacto.IdSituacaoPacto == 2 || a.SituacaoPacto.IdSituacaoPacto == 3).ToList();
        }
    }
}
