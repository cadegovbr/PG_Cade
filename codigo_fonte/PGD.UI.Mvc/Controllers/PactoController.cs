using DomainValidation.Validation;
using PGD.Application.Interfaces;
using PGD.Application.ViewModels;
using PGD.Domain.Entities;
using PGD.Domain.Entities.RH;
using PGD.Domain.Enums;
using PGD.Domain.Interfaces.Service;
using PGD.UI.Mvc.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using PGD.Application.Util;
using PGD.Application.ViewModels.Filtros;
using PGD.Domain.Filtros;
using Perfil = PGD.Domain.Enums.Perfil;

namespace PGD.UI.Mvc.Controllers
{
    public class PactoController : BaseController
    {
        readonly IPactoAppService _Pactoservice;
        readonly IAvaliacaoProdutoService _avaliacaoProdutoService;
        readonly IProdutoAppService _Produtoservice;
        readonly IUsuarioAppService _Usuarioservice;
        PactoViewModel _pactoVM;
        readonly IGrupoAtividadeAppService _grupoAtividadeService;
        readonly IAtividadeAppService _atividadeService;
        readonly IOrdemServicoAppService _ordemServicoService;
        readonly IHistoricoAppService _historicoService;
        readonly ILogService _logService;
        readonly ICronogramaAppService _cronogramaService;
        readonly IJustificativaAppService _justificativaService;
        readonly ISituacaoPactoAppService _situacaoPactoService;
        readonly IIniciativaPlanoOperacionalAppService _iniciativaPOAppService;
        readonly IAvaliacaoProdutoAppService _avaliacaoProdutoAppService;
        readonly ITipoPactoAppService _tipoPactoAppService;
        readonly IUnidade_TipoPactoAppService _unidade_tipoPactoAppService;
        readonly INotificadorAppService _notificadorAppService;
        readonly INivelAvaliacaoAppService _nivelAvaliacaoAppService;
        readonly IOS_CriterioAvaliacaoAppService _ordemServico_CriterioAvaliacaoAppService;
        readonly IOS_ItemAvaliacaoAppService _ordemServico_ItemAvaliacaoAppService;
        readonly INotaAvaliacaoAppService _notaAvaliacaoAppService;
        readonly IAnexoProdutoService _anexoProdutoService;
        readonly ISituacaoProdutoAppService _situacaoProdutoAppService;

        List<Unidade> unidadesSubordinadas;
        public OrdemServicoViewModel OrdemServico
        {
            get
            {
                TempData.Keep("OSPACTO");
                return (OrdemServicoViewModel)TempData?["OSPACTO"];
            }
            set { TempData["OSPACTO"] = value; }
        }

#pragma warning disable S107 // Methods should not have too many parameters
        public PactoController(ILogService logService, IUsuarioAppService usuarioAppService, IPactoAppService pactoservice, IUnidadeService unidadeService,
            IGrupoAtividadeAppService grupoAtividadeService, IAtividadeAppService atividadeService, IOrdemServicoAppService ordemSericoService,
            PactoViewModel pactoVM, IProdutoAppService produtoService, IHistoricoAppService historicoService, ICronogramaAppService cronogramaAppService,
            IJustificativaAppService justificativaService, ISituacaoPactoAppService situacaoPactoService, IIniciativaPlanoOperacionalAppService iniciativaPOAppService,
            IAvaliacaoProdutoService avaliacaoProdutoService, IAvaliacaoProdutoAppService avaliacaoProdutoAppService, ITipoPactoAppService tipoPactoAppService,
            IUnidade_TipoPactoAppService unidade_tipoPactoAppService, INotificadorAppService notificadorAppService,
            INivelAvaliacaoAppService nivelAvaliacaoAppService, IOS_CriterioAvaliacaoAppService ordemServico_CriterioAvaliacaoAppService,
            IOS_ItemAvaliacaoAppService ordemServico_ItemAvaliacaoAppService, INotaAvaliacaoAppService notaAvaliacaoAppService, IAnexoProdutoService anexoProdutoService, ISituacaoProdutoAppService situacaoProdutoAppService)
#pragma warning restore S107 // Methods should not have too many parameters
            : base(usuarioAppService)
        {
            _Pactoservice = pactoservice;
            _avaliacaoProdutoService = avaliacaoProdutoService;
            _Usuarioservice = usuarioAppService;
            _unidadeService = unidadeService;
            _pactoVM = pactoVM;
            _grupoAtividadeService = grupoAtividadeService;
            _atividadeService = atividadeService;
            _ordemServicoService = ordemSericoService;
            _Produtoservice = produtoService;
            _historicoService = historicoService;
            _cronogramaService = cronogramaAppService;
            _logService = logService;
            _justificativaService = justificativaService;
            _situacaoPactoService = situacaoPactoService;
            _iniciativaPOAppService = iniciativaPOAppService;
            _avaliacaoProdutoAppService = avaliacaoProdutoAppService;
            _tipoPactoAppService = tipoPactoAppService;
            _unidade_tipoPactoAppService = unidade_tipoPactoAppService;
            _notificadorAppService = notificadorAppService;
            _nivelAvaliacaoAppService = nivelAvaliacaoAppService;
            _ordemServico_CriterioAvaliacaoAppService = ordemServico_CriterioAvaliacaoAppService;
            _ordemServico_ItemAvaliacaoAppService = ordemServico_ItemAvaliacaoAppService;
            _notaAvaliacaoAppService = notaAvaliacaoAppService;
            _anexoProdutoService = anexoProdutoService;
            _situacaoProdutoAppService = situacaoProdutoAppService;
        }

        [HttpPost]
        public string Assinar(int idpacto)
        {

            var pacto = _Pactoservice.BuscarPorId(idpacto);
            var user = getUserLogado();
            ViewBag.CpfUsuarioLogado = RetornaCpfCorrigido(user.CPF);


            _pactoVM.podeEditar = false;
            return "Plano de Trabalho assinado com sucesso.";

        }

        [HttpPost]
        public string Negar(int idpacto)
        {

            var pacto = _Pactoservice.BuscarPorId(idpacto);
            var user = getUserLogado();
            ViewBag.CpfUsuarioLogado = RetornaCpfCorrigido(user.CpfUsuario);
            pacto.CpfUsuarioCriador = RetornaCpfCorrigido(user.CpfUsuario);

            _Pactoservice.AtualizarStatus(pacto, user, eAcaoPacto.Negando);

            return "Plano de trabalho negado com sucesso.";


        }

        // GET: Pacto
        public ActionResult Index()
        {
            
            var user = getUserLogado();
            var searchpacto = new SearchPactoViewModel
            {
                ObterPactosUnidadesSubordinadas = user.IsDirigente
            };

            return Index(searchpacto);
        }

        [HttpPost]
        public ActionResult Index(SearchPactoViewModel obj)
        {
            var PactoCompleto = PreparaPactoViewModel(obj);
            if (PactoCompleto?.lstPactos?.Count != null && PactoCompleto.lstPactos.Count <= 0)
            {
                var lstErros = new List<ValidationError>() { new ValidationError("Nenhum registro encontrado.") };
                setModelErrorList(lstErros);
            }

            ConfigurarNomesServidoresPesquisa();
            ConfigurarSituacoes();
            ConfigurarTipos();
            ConfigurarSituacaoProduto();


            return View(PactoCompleto);
        }

        [HttpPost]
        public ActionResult GerarExcel(SearchPactoViewModel obj)
        {
            var PactoCompleto = PreparaPactoViewModel(obj);
            var ret = ToExcel(Response, ToDataTable(PactoCompleto));
            return View("");
        }

        public PactoCompletoViewModel PreparaPactoViewModel(SearchPactoViewModel obj)
        {

            var idSituacao = obj.idSituacao;
            var pactoViewModel = new PactoViewModel();
            var PactoCompleto = new PactoCompletoViewModel();
            var unidades = _unidadeService.ObterUnidades().ToList();
            Unidade retornoUnidade;
            var user = getUserLogado();
            bool dirigente = true;


            PactoCompleto.Searchpacto = obj;
            if (obj.DataInicial != null) pactoViewModel.DataPrevistaInicio = (DateTime)obj.DataInicial;
            pactoViewModel.DataPrevistaTermino = obj.DataFinal;
            pactoViewModel.Nome = obj.NomeServidor;
            pactoViewModel.IdSituacaoPacto = idSituacao.GetValueOrDefault();
            pactoViewModel.IdTipoPacto = obj.idTipoPacto.GetValueOrDefault();
            pactoViewModel.IdPacto = obj.IdPacto.GetValueOrDefault();

            if (obj.UnidadeId > 0)
            {
                retornoUnidade = unidades.FirstOrDefault(x => x.IdUnidade == obj.UnidadeId);
                if (retornoUnidade != null)
                {
                    pactoViewModel.UnidadeDescricao = retornoUnidade.Nome;
                    pactoViewModel.UnidadeExercicio = obj.UnidadeId.Value;
                }
            }
            var retorno = _Pactoservice.ObterTodos(pactoViewModel, obj.ObterPactosUnidadesSubordinadas);
            dirigente = user.IsDirigente;

            if (retorno != null)
            {
                var pactoViewModels = retorno.ToList();
                foreach (var item in pactoViewModels)
                {

                    podePermissoes(item, user, dirigente);
                    retornoUnidade = unidades.FirstOrDefault(x => x.IdUnidade == item.UnidadeExercicio);
                    if (retornoUnidade != null)
                    {
                        item.UnidadeDescricao = retornoUnidade.Nome;
                    }
                }

                PactoCompleto.Searchpacto.lstUnidade = unidades;
                PactoCompleto.lstPactos = pactoViewModels.Where(p => p.podeVisualizar).ToList();
            }
            PactoCompleto.Searchpacto = obj;
            return PactoCompleto;

        }

        public ActionResult Deletar(int? pactoid)
        {
            var pacto = _Pactoservice.BuscarPorId(pactoid.Value);
            var user = getUserLogado();

            if (pacto != null)
            {
                var pactoRetorno = _Pactoservice.AtualizarStatus(pacto, user, eAcaoPacto.Excluindo);
                pacto = pactoRetorno;
                if (pactoRetorno.ValidationResult.IsValid)
                    return setMessageAndRedirect("Plano de Trabalho excluído com sucesso!", "Index");
            }

            return setMessageAndRedirect(pacto.ValidationResult.Erros, "Index");
        }

        public ActionResult Consultar(int pactoid)
        {

            var pacto = _Pactoservice.BuscarPorId(pactoid);
            return setMessageAndRedirect("Plano de Trabalho Consultado", "Index");
        }

        public ActionResult Editar(int pactoid)
        {
            var pacto = _Pactoservice.BuscarPorId(pactoid);
            return setMessageAndRedirect("Plano de Trabalho Editar", "Index");

        }

        public DataTable ToDataTable(PactoCompletoViewModel items)
        {
            DataTable dataTable = new DataTable("Pactos");

            //dados header
            dataTable.Columns.Add("Código do Plano de Trabalho");
            dataTable.Columns.Add("Nome");
            dataTable.Columns.Add("Data Prevista Início");
            dataTable.Columns.Add("Data Prevista Término");
            dataTable.Columns.Add("Situação");
            dataTable.Columns.Add("Unidade");

            DataRow row;

            var unidades = _unidadeService.ObterUnidades().ToList();

            foreach (PactoViewModel p in items.lstPactos)
            {

                var retornoSituacao = unidades.FirstOrDefault(x => x.IdUnidade == p.UnidadeExercicio);
                if (retornoSituacao != null) p.UnidadeDescricao = retornoSituacao.Nome;

                row = dataTable.NewRow();
                row["Código do Plano de Trabalho"] = p.IdPacto;
                row["Nome"] = p.Nome;
                row["Data Prevista Início"] = p.DataPrevistaInicio;
                row["Data Prevista Término"] = p.DataPrevistaTermino;
                row["Situação"] = p.SituacaoPactoDescricao;
                row["Unidade"] = p.UnidadeDescricao;
                dataTable.Rows.Add(row);
            }

            return dataTable;
        }


        public ActionResult Solicitar(int? id = null, int? idTipoPacto = null, int? abrircronograma = null, bool contemErro = false)
        {
            if (contemErro)
            {
                var lstErros = new List<ValidationError>() { new ValidationError("Ação Inválida! É necessário informar o produto que será gerado pelo pacto.") };
                setModelErrorList(lstErros);
            }

            
           

            id = id == 0 ? null : id;
            var user = getUserLogado();
            ViewBag.CpfUsuarioLogado = RetornaCpfCorrigido(user.CPF);
            ViewBag.isDirigente = user.IsDirigente;
            ViewBag.AbrirCronograma = abrircronograma;
            _pactoVM.IdTipoPacto = idTipoPacto.GetValueOrDefault();
            _pactoVM.ehPGDProjeto = idTipoPacto.HasValue && idTipoPacto == (int)PGD.Domain.Enums.eTipoPacto.PGD_Projeto;

            ConfigurarIniciativasPlanoOperacional();
            ConfigurarSituacaoProduto();

            if (id != null)
            {
                _pactoVM = _Pactoservice.BuscarPorId(id.Value);


                if (_pactoVM.IndVisualizacaoRestrita && !_Pactoservice.PodeVisualizar(_pactoVM, user, user.IsDirigente, UnidadePactoESubordinadaUnidadeUsuario(_pactoVM, user)))
                {
                    return setMessageAndRedirect("Plano de Trabalho PGD marcado como reservado", "Index");
                }

                // var unidades = _unidadeService.ObterUnidades().ToList();
                // var retornoSituacao = unidades.FirstOrDefault(x => x.IdUnidade == _pactoVM.UnidadeExercicio);
                _pactoVM.UnidadeDescricao = _unidadeService.Buscar(new UnidadeFiltro {Id = _pactoVM.UnidadeExercicio}).Lista.FirstOrDefault()?.Nome;
                _pactoVM.StatusAssinatura = _Pactoservice.BuscaStatusAssinatura(_pactoVM);

                if (_pactoVM.IdPacto == 0)
                {
                    _pactoVM.UnidadeUsuarioPermitePactoExecucaoNoExterior = _unidade_tipoPactoAppService.BuscarPorIdUnidadeTipoPacto(_pactoVM.UnidadeExercicio, _pactoVM.IdTipoPacto)?.IndPermitePactoExterior ?? false;
                }
                else
                {
                    _pactoVM.UnidadeUsuarioPermitePactoExecucaoNoExterior = _pactoVM.PactoExecutadoNoExterior;
                }

                ConfigurarNomesServidoresUnidadesSolicitacao(ViewBag.isDirigente, _pactoVM.CpfUsuario, id.GetValueOrDefault(), idTipoPacto.GetValueOrDefault());

                AtualizaOrdemServico(_pactoVM.OrdemServico.IdOrdemServico);
                ConfigurarGruposAtividades(OrdemServico, _pactoVM.IdTipoPacto);
                ConfigurarSituacaoProduto();

                if (_pactoVM.Produtos.Count > 0)
                {
                    for (int i = 0; i < _pactoVM.Produtos.Count; i++)
                    {
                        var pdrt = _pactoVM.Produtos[i];
                        var grupo = OrdemServico.Grupos.FirstOrDefault(x => x.IdGrupoAtividade == pdrt.IdGrupoAtividade);
                        var atividade = grupo.Atividades.FirstOrDefault(a => a.IdAtividade == pdrt.IdAtividade);
                        var qtdFaixa = atividade.Tipos.Count();
                        pdrt.QtdFaixas = qtdFaixa;
                        ConfigurarCargaHorariaFormatada(pdrt);
                        ConfigurarTotalAvaliado(pdrt);
                        pdrt.Index = i + 1;
                        pdrt.IdOrdemServico = _pactoVM.IdOrdemServico;
                    }

                }
                TempData[GetNomeVariavelTempData("Produtos", id.GetValueOrDefault())] = _pactoVM.Produtos;

                var pactosConcorrentes = _Pactoservice.GetPactosConcorrentes(_pactoVM.DataPrevistaInicio, DateTime.Now.AddYears(1),
                                                                             _pactoVM.CpfUsuario, _pactoVM.IdPacto);

                //setando permissoes para editar dias cronograma e horas já usadas por outros pactos.
                _pactoVM.Cronogramas.ForEach(c =>
                        {
                            c.PodeEditar = _cronogramaService.PodeEditarDiaCronograma(_pactoVM.CpfUsuario, getUserLogado(), c.DataCronograma);
                            c.HorasUsadasPorOutroPacto = TimeSpan.FromHours(Convert.ToDouble(_cronogramaService.GetQuantidadeHorasNoDia(c.DataCronograma, pactosConcorrentes)));
                        });

                _pactoVM.CargaHorariaTotal =
                    (_pactoVM.Produtos ?? new List<ProdutoViewModel>()).Sum(x => x.CargaHorariaProduto * x.QuantidadeProduto);

                TempData[GetNomeVariavelTempData("Cronogramas", id.GetValueOrDefault())] = new CronogramaPactoViewModel()
                {
                    DataInicial = _pactoVM.DataPrevistaInicio,
                    HorasTotais = Convert.ToDouble(_pactoVM.CargaHorariaTotal),
                    HorasDiarias = _pactoVM.CargaHorariaDiaria,
                    IdPacto = _pactoVM.IdPacto,
                    Cronogramas = _pactoVM.Cronogramas,
                    DataInicioSuspensao = _pactoVM.SuspensoAPartirDe,
                    DataFimSuspensao = _pactoVM.SuspensoAte
                };
            }
            else
            {
                TempData[GetNomeVariavelTempData("Produtos", id.GetValueOrDefault())] = new List<ProdutoViewModel>();
                _pactoVM.DataPrevistaInicio = DateTime.Today;
                _pactoVM.Produtos = new List<ProdutoViewModel>();
                _pactoVM.DataPrevistaTermino = null;
                _pactoVM.SituacaoPacto = _situacaoPactoService.ObterTodos().Where(s => s.IdSituacaoPacto == (int)eSituacaoPacto.PendenteDeAssinatura).SingleOrDefault();
                _pactoVM.CargaHorariaDiaria = TimeSpan.FromHours(8);
                _pactoVM.IdTipoPacto = idTipoPacto.HasValue ? idTipoPacto.Value : 0;
                _pactoVM.TipoPacto = idTipoPacto.HasValue ? _tipoPactoAppService.ObterTodos().Where(s => s.IdTipoPacto == idTipoPacto.Value).SingleOrDefault() : null;
                podePermissoes(_pactoVM, user, ViewBag.isDirigente);
                ConfigurarNomesServidoresUnidadesSolicitacao(ViewBag.isDirigente, getUserLogado().CPF, id.GetValueOrDefault(), _pactoVM.IdTipoPacto);

                #region Dropdown
                AtualizaOrdemServico(id);
                if (OrdemServico == null)
                {
                    return setMessageAndRedirect("Não há lista de atividades vigente", "index");
                }
                ConfigurarGruposAtividades(OrdemServico, _pactoVM.IdTipoPacto);

                _pactoVM.OrdemServico = OrdemServico;

                var perfil = user?.Perfis?.FirstOrDefault();
                if (user.IdPerfilSelecionado == (int)Domain.Enums.Perfil.Solicitante)
                {
                    _pactoVM.Nome = user.Nome;
                    _pactoVM.CpfUsuario = RetornaCpfCorrigido(user.CPF);
                    _pactoVM.MatriculaSIAPE = user.Matricula;

                    _pactoVM.UnidadeDescricao = user.nomeUnidade;
                    _pactoVM.UnidadeUsuarioPermitePactoExecucaoNoExterior = _unidade_tipoPactoAppService.BuscarPorIdUnidadeTipoPacto(user.IdUnidadeSelecionada ?? 0, _pactoVM.IdTipoPacto)?.IndPermitePactoExterior ?? false;
                }
                _pactoVM.UnidadeExercicio = user.Unidade;
                _pactoVM.podeEditar = true;

                #endregion

                return View(_pactoVM);

            }

            podePermissoes(_pactoVM, user, ViewBag.isDirigente);
            return View(_pactoVM);
        }

        private void AtualizaOrdemServico(int? id)
        {
            OrdemServico = id.HasValue && id > 0 ? _ordemServicoService.ObterPorId(id.Value) : _ordemServicoService.GetOrdemVigente();
        }

        private static void ConfigurarCargaHorariaFormatada(ProdutoViewModel pdrt)
        {
            var tsCarga = TimeSpan.FromHours((double)pdrt.CargaHorariaProduto * pdrt.QuantidadeProduto);
            string minutes = tsCarga.Minutes < 10 ? "0" + tsCarga.Minutes : tsCarga.Minutes.ToString();
            pdrt.CargaHorariaTotalProdutoFormatada = $"{Math.Floor(tsCarga.TotalHours)}:{minutes}";
        }

        private void ConfigurarTotalAvaliado(ProdutoViewModel pdrt)
        {
            IEnumerable<AvaliacaoProduto> avaliacoesProduto = _avaliacaoProdutoService.ObterAvaliacoesPorProduto(pdrt.IdProduto);
            pdrt.QuantidadeProdutoAvaliado = avaliacoesProduto.Sum(p => p.QuantidadeProdutosAvaliados);

            var tsCarga = TimeSpan.FromHours((double)pdrt.CargaHorariaProduto * pdrt.QuantidadeProdutoAvaliado);
            string minutes = tsCarga.Minutes < 10 ? "0" + tsCarga.Minutes : tsCarga.Minutes.ToString();
            pdrt.CargaHorariaHomologada = $"{Math.Floor(tsCarga.TotalHours)}:{minutes}";
        }

        private void ConfigurarHistoricoAvaliacoes(AvaliacaoProdutoViewModel avaliacaoProdutoVM)
        {
            AvaliacaoProduto avaliacaoProduto = _avaliacaoProdutoService.ObterPorId(avaliacaoProdutoVM.IdAvaliacaoProduto);
            avaliacaoProdutoVM.NomeAvaliador = _usuarioAppService.ObterPorCPF(avaliacaoProduto.CPFAvaliador).Nome;
        }

        private void ConfigurarNomesServidoresUnidadesSolicitacao(bool isDirigente, string cpfUsuario, int idPacto, int idTipoPacto)
        {

            var userLogado = getUserLogado();
            
            if (isDirigente)
            {
                var usuarios = _usuarioAppService.Buscar(new UsuarioFiltroViewModel
                {
                    IdUnidade = userLogado.IdUnidadeSelecionada
                })?.Lista ?? new List<UsuarioViewModel>();

                usuarios = idPacto == 0 && usuarios.Any(x => x.CPF == userLogado.CPF)
                    ? usuarios.Where(x => x.CPF != userLogado.CPF).ToList() : usuarios;

                if (!string.IsNullOrEmpty(cpfUsuario) && (usuarios.All(u => u.CPF != cpfUsuario) && (userLogado.CPF != cpfUsuario || idPacto != 0)))
                {
                    var usuario = _usuarioAppService.Buscar(new UsuarioFiltroViewModel
                    {
                        Cpf = cpfUsuario
                    })?.Lista.FirstOrDefault();
                    
                    if(usuario != null) usuarios.Add(usuario);
                }

                TempData["NomesSubordinados"] = usuarios;
            }
            else
            {
                TempData["NomesSubordinados"] = _usuarioAppService.Buscar(new UsuarioFiltroViewModel
                {
                    Cpf = cpfUsuario
                })?.Lista ?? new List<UsuarioViewModel>();
            }

            List<Unidade> unidadesHabilitadas = _unidadeService.Buscar(new UnidadeFiltro
            {
                IdTipoPacto = idTipoPacto,
                IdUsuario = isDirigente ? userLogado?.IdUsuario : null
            }).Lista ?? new List<Unidade>();

            if (idPacto > 0)
            {
                var pactoVm = _Pactoservice.BuscarPorId(idPacto);

                if (unidadesHabilitadas.FirstOrDefault(u => u.IdUnidade == pactoVm.UnidadeExercicio) == null)
                {
                    var unidadePacto = _unidadeService.Buscar(new UnidadeFiltro { Id = pactoVm.UnidadeExercicio }).Lista.FirstOrDefault();
                    unidadesHabilitadas.Add(unidadePacto);
                }
            }

            TempData[GetNomeVariavelTempData("Unidades", idPacto)] = unidadesHabilitadas.AsEnumerable();

        }

        private void ConfigurarNomesServidoresPesquisa()
        {
            TempData["NomesSubordinados"] = _usuarioAppService.ObterTodos();
        }

        private List<IniciativaPlanoOperacionalViewModel> ConfigurarIniciativasPlanoOperacional()
        {
            var todasIniciativas = _iniciativaPOAppService.ObterTodos();
            TempData["IniciativasPlanoOperacional"] = todasIniciativas.OrderBy(i => i.CodigoDecimal).ToList();
            return todasIniciativas;
        }

        private void ConfigurarGruposAtividades(OrdemServicoViewModel osvm, int idTipoPacto)
        {
            if (idTipoPacto == 0)
            {
                TempData["GruposAtividades"] = osvm.Grupos.ToList();
            }
            else
            {
                TempData["GruposAtividades"] = osvm.Grupos
                    .Where(g => g.TiposPacto.Select(t => t.IdTipoPacto).Contains(idTipoPacto))
                    .OrderBy(g => g.NomGrupoAtividade)
                    .ToList();
            }
        }

        private void ConfigurarSituacaoProduto()

            
        {
            TempData["SituacaoProduto"] = _situacaoProdutoAppService.ObterTodos();
        }

        private void ConfigurarJustificativas()
        {
            TempData["Justificativas"] = _justificativaService.ObterTodos();
        }

        private void ConfigurarSituacoes()
        {
            TempData["Situacoes"] = _situacaoPactoService.ObterTodos().OrderBy(s => s.DescSituacaoPacto).ToList();
        }

        private void ConfigurarTipos()
        {
            TempData["TiposPacto"] = _tipoPactoAppService.ObterTodos().OrderBy(s => s.DescTipoPacto).ToList();
        }

        private void ConfigurarNiveisAvaliacao()
        {
            TempData["NiveisAvaliacao"] = _nivelAvaliacaoAppService.ObterTodos().OrderBy(s => s.IdNivelAvaliacao).ToList();
        }

        private void podePermissoes(PactoViewModel _pactoVM, UsuarioViewModel user, bool isDirigente)
        {
            bool unidadePactoESubordinadaUnidadeUsuario = UnidadePactoESubordinadaUnidadeUsuario(_pactoVM, user);
            _pactoVM.podeVisualizar = _Pactoservice.PodeVisualizar(_pactoVM, user, user.IsDirigente, unidadePactoESubordinadaUnidadeUsuario);

            // OBS.: pediram para liberar para qualquer unidade, sendo chefe, paliativamente, 
            // até que seja resolvido o bug que o substituto nao consegue acessar pactos da unidade do chefe.
            unidadePactoESubordinadaUnidadeUsuario = true;

            if (unidadePactoESubordinadaUnidadeUsuario)
            {
                _pactoVM.podeAssinar = _Pactoservice.PodeAssinar(_pactoVM, user, isDirigente, unidadePactoESubordinadaUnidadeUsuario);
                _pactoVM.podeAvaliar = _Pactoservice.PodeAvaliar(_pactoVM, user, isDirigente, unidadePactoESubordinadaUnidadeUsuario);
                _pactoVM.podeDeletar = _Pactoservice.PodeDeletar(_pactoVM, user, isDirigente, unidadePactoESubordinadaUnidadeUsuario);
                _pactoVM.podeEditar = _Pactoservice.PodeEditar(_pactoVM, user, isDirigente, unidadePactoESubordinadaUnidadeUsuario);
                _pactoVM.podeInterromper = _Pactoservice.PodeInterromper(_pactoVM, user, isDirigente, unidadePactoESubordinadaUnidadeUsuario);
                _pactoVM.podeNegar = _Pactoservice.PodeNegar(_pactoVM, user, isDirigente, unidadePactoESubordinadaUnidadeUsuario);
                _pactoVM.podeSuspender = _Pactoservice.PodeSuspender(_pactoVM, user, isDirigente, unidadePactoESubordinadaUnidadeUsuario);
                _pactoVM.podeEditarAndamento = _Pactoservice.PodeEditarEmAndamento(_pactoVM, user, isDirigente, unidadePactoESubordinadaUnidadeUsuario);
                _pactoVM.podeCancelarAvaliacao = _Pactoservice.PodeCancelarAvaliacao(_pactoVM, user, isDirigente, unidadePactoESubordinadaUnidadeUsuario);
                _pactoVM.podeVisualizarPactuadoAvaliado = _Pactoservice.PodeVisualizarPactuadoAvaliado(_pactoVM);
                bool podeEditarObservacaoProduto = _Pactoservice.PodeEditarObservacaoProduto(_pactoVM, user, isDirigente, unidadePactoESubordinadaUnidadeUsuario);
                bool podeVisualizadAvaliacaoProduto = _Pactoservice.PodeVisualizarAvaliacaoProduto(_pactoVM, user, isDirigente, unidadePactoESubordinadaUnidadeUsuario);
                _pactoVM.Produtos?.ForEach(p =>
                {
                    p.PodeEditarAndamento = _pactoVM.podeEditarAndamento;
                    p.PodeEditar = _pactoVM.podeEditar;
                    p.PodeEditarObservacaoProduto = podeEditarObservacaoProduto;
                    p.PodeVisualizarAvaliacaoProduto = podeVisualizadAvaliacaoProduto;
                    p.PodeVisualizarPactuadoAvaliado = _pactoVM.podeVisualizarPactuadoAvaliado;
                });
            }
            else
            {
                _pactoVM.podeAssinar = false;
                _pactoVM.podeAvaliar = false;
                _pactoVM.podeDeletar = false;
                _pactoVM.podeEditar = false;
                _pactoVM.podeInterromper = false;
                _pactoVM.podeNegar = false;
                _pactoVM.podeSuspender = false;
                _pactoVM.podeEditarAndamento = false;
                _pactoVM.podeVisualizarPactuadoAvaliado = false;
            }
            _pactoVM.podeVoltarSuspensao = _Pactoservice.PodeVoltarSuspensao(_pactoVM, user, isDirigente, unidadePactoESubordinadaUnidadeUsuario);
            _pactoVM.podeRestringirVisibilidadePacto = user.IsDirigente && user.IdUnidadeSelecionada == _pactoVM.UnidadeExercicio;

        }

        private bool UnidadePactoESubordinadaUnidadeUsuario(PactoViewModel _pactoVM, UsuarioViewModel user)
        {
            IEnumerable<Unidade> unidadesSubordinadas = ObterUnidadesSubordinadas(user);
            bool unidadePactoESubordinadaUnidadeUsuario = _pactoVM.UnidadeExercicio == 0 || unidadesSubordinadas.Any(us => us.IdUnidade == _pactoVM.UnidadeExercicio);
            return unidadePactoESubordinadaUnidadeUsuario;
        }

        private IEnumerable<Unidade> ObterUnidadesSubordinadas(UsuarioViewModel user)
        {
            if (unidadesSubordinadas == null) unidadesSubordinadas = _unidadeService.ObterUnidadesSubordinadas(user.Unidade).ToList();
            return unidadesSubordinadas;
        }

        [HttpPost]
        public ActionResult Solicitar(PactoViewModel pactoViewModel)
        {
            if (pactoViewModel.Produtos.Count == 0)
            {
                return setMessageAndRedirect(
                    "Solicitar",
                    new RouteValueDictionary { { "idTipoPacto", pactoViewModel.IdTipoPacto.ToString()}, { "contemErro", true }
                    });
            }

           var validarPactoConcorrente = _Pactoservice.GetPactosConcorrentes(pactoViewModel.DataPrevistaInicio, DateTime.Now.AddYears(1),
                                                                             pactoViewModel.CpfUsuario, pactoViewModel.IdPacto);
            ConfigurarSituacaoProduto();
          


            ModelState.Where(x => x.Key.Contains("Produtos[")).ToList().ForEach(x =>
            {
                ModelState[x.Key].Errors.Clear();
            });

            string acao = pactoViewModel.Acao;
            if ((acao.Equals("Salvando") || acao.Equals("Assinando")) && ModelState.IsValid)
            {
                pactoViewModel = SalvarSolicitar(pactoViewModel);
                if (pactoViewModel.ValidationResult != null && pactoViewModel.ValidationResult.IsValid)
                {
                    string mensagem = pactoViewModel.ValidationResult.Message;
                    if (acao.Equals("Assinando"))
                    {
                        mensagem = Assinar(pactoViewModel.IdPacto);
                    }
                    else
                    {
                        mensagem = "Plano N° " + pactoViewModel.IdPacto + " salvo com sucesso!";
                    }
                   
                     return setMessageAndRedirect(mensagem,
                   "Solicitar",
                    new RouteValueDictionary { { "id", pactoViewModel.IdPacto }, { "idTipoPacto", pactoViewModel.IdTipoPacto.ToString()}
                    });
                   
                }
                else
                {
                    setMessage(pactoViewModel.ValidationResult);
                    return View(pactoViewModel);
                }
            }
            else if (acao.Equals("Negando"))
            {
                return setMessageAndRedirect(Negar(pactoViewModel.IdPacto), "Index");
            }
            else
            {
                setMessage("Ação Inválida");
                return View(pactoViewModel);
            }
        }

        public ActionResult CancelarAvaliacao(AvaliacaoProdutoViewModel apvm)
        {

            var avaliacaoProduto = _avaliacaoProdutoAppService.ObterPorId(apvm.IdAvaliacaoProduto);
            var pacto = _Pactoservice.BuscarPorId(apvm.IdPacto);
            var user = getUserLogado();
            ViewBag.CpfUsuarioLogado = RetornaCpfCorrigido(user.CpfUsuario);

            eAcaoPacto acao = (avaliacaoProduto.TipoAvaliacao == (int)eTipoAvaliacao.Total) ? eAcaoPacto.CancelandoAvaliacao : eAcaoPacto.CancelandoAvaliacaoParcialmente;

            var pactoVm = _Pactoservice.CancelarAvaliacao(pacto, avaliacaoProduto, user, acao);


            if (!pactoVm.ValidationResult.IsValid)
            {
                ModelState.Clear();
                setModelError(pactoVm.ValidationResult);
                return RetornarErrosModelState();
            }

            if (avaliacaoProduto.TipoAvaliacao == (int)eTipoAvaliacao.Total)
            {
                setMessage("Avaliação de plano de trabalho cancelada com sucesso.");
            }
            else
            {
                setMessage("Avaliação Parcial de plano de trabalho cancelada com sucesso.");
            }

            ConfigurarQuantidadeProdutos(apvm);
            ConfigurarJustificativas();
            return PartialView("_AvaliarProdutoFormPartial", apvm);

        }

        public PactoViewModel SalvarSolicitar(PactoViewModel pactoViewModel)
        {
            if (pactoViewModel.PossuiCargaHoraria != null && pactoViewModel.PossuiCargaHoraria == false)
                pactoViewModel.CargaHorariaDiaria = TimeSpan.FromHours(8);
           
           
            var user = getUserLogado();
            AtualizaOrdemServico(pactoViewModel.IdOrdemServico);
            ConfigurarGruposAtividades(OrdemServico, pactoViewModel.IdTipoPacto);
            // ConfigurarListaProdutos(pactoViewModel, OrdemServico);

            ViewBag.isDirigente = user.IsDirigente;

            PactoViewModel pactoBuscado = null;

            if (pactoViewModel.IdPacto != 0)
            {
                //Atualizar Pacto
                pactoBuscado = _Pactoservice.BuscarPorId(pactoViewModel.IdPacto);
                pactoViewModel.PossuiCargaHoraria = pactoBuscado.PossuiCargaHoraria;

                if (pactoBuscado != null)
                {
                    pactoViewModel.CpfUsuarioCriador = pactoBuscado.CpfUsuarioCriador;
                    if (pactoViewModel.DataPrevistaInicio == DateTime.MinValue)
                        pactoViewModel.DataPrevistaInicio = pactoBuscado.DataPrevistaInicio;
                }

            }
            else
            {
                pactoViewModel.CpfUsuarioCriador = RetornaCpfCorrigido(user.CPF);
            }

            pactoViewModel.IdOrdemServico = OrdemServico.IdOrdemServico.Value;
            pactoViewModel.IdSituacaoPacto = (int)eSituacaoPacto.PendenteDeAssinatura;

            if (pactoViewModel.IdTipoPacto != (int)PGD.Domain.Enums.eTipoPacto.PGD_Projeto)
            {
                pactoViewModel.TAP = String.Empty;
            }

            Unidade_TipoPactoViewModel unidade_TipoPacto = _unidade_tipoPactoAppService.BuscarPorIdUnidadeTipoPacto(pactoViewModel.UnidadeExercicio, pactoViewModel.IdTipoPacto);

            if ((!unidade_TipoPacto?.IndPermitePactoExterior ?? false))
            {
                pactoViewModel.PactoExecutadoNoExterior = false;
                pactoViewModel.ProcessoSEI = String.Empty;
            }
            else if ((unidade_TipoPacto?.IndPermitePactoExterior ?? false) && (!pactoViewModel.PactoExecutadoNoExterior.Value))
            {
                pactoViewModel.ProcessoSEI = String.Empty;
            }

            pactoViewModel.TipoPacto = null;

            var tempCronogramas = (CronogramaPactoViewModel)TempData[GetNomeVariavelTempData("Cronogramas", pactoViewModel.IdPacto)];
            if (tempCronogramas == null || tempCronogramas.Cronogramas?.Count == 0)
            {
                tempCronogramas = new CronogramaPactoViewModel()
                {
                    DataInicial = pactoViewModel.DataPrevistaInicio,
                    HorasTotais = Convert.ToDouble(pactoViewModel.CargaHorariaTotal),
                    HorasDiarias = pactoViewModel.CargaHorariaDiaria,
                    IdPacto = pactoViewModel.IdPacto,
                    Cronogramas = _cronogramaService.CalcularCronogramas(Convert.ToDouble(pactoViewModel.CargaHorariaTotal),
                                                                            pactoViewModel.CargaHorariaDiaria,
                                                                            pactoViewModel.DataPrevistaInicio,
                                                                            CPFUsuario: pactoViewModel.CpfUsuario,
                                                                            usuarioLogado: getUserLogado(),
                                                                            idPacto: pactoViewModel.IdPacto,
                                                                            dataInicioSuspensao: pactoViewModel.SuspensoAPartirDe,
                                                                            dataFimSuspensao: pactoViewModel.SuspensoAte,
                                                                            cronogramaExistente: tempCronogramas),

                    DataInicioSuspensao = pactoViewModel.SuspensoAPartirDe,
                    DataFimSuspensao = pactoViewModel.SuspensoAte

                };

            }

            TempData[GetNomeVariavelTempData("Cronogramas", pactoViewModel.IdPacto)] = tempCronogramas;

            pactoViewModel.Cronogramas = tempCronogramas.Cronogramas;
            string messages = string.Join("; ", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));

            if (ModelState.IsValid)
            {

                pactoViewModel.Produtos.ForEach(x =>
                {
                    x.IniciativasPlanoOperacionalProduto = x.IniciativasPlanoOperacionalProduto ??
                                                           new List<IniciativaPlanoOperacionalProdutoViewModel>();

                    x.IniciativasPlanoOperacionalSelecionadas.ForEach(y =>
                    {
                        x.IniciativasPlanoOperacionalProduto.Add(new IniciativaPlanoOperacionalProdutoViewModel { IdIniciativaPlanoOperacional = y });
                    });
                });

                // Inclusao
                if (pactoViewModel.IdPacto == 0)
                {
                    var perfil = user?.Perfis?.FirstOrDefault();
                    if (perfil != null && perfil.Value == Domain.Enums.Perfil.Solicitante)
                        pactoViewModel.CpfUsuario = RetornaCpfCorrigido(user.CPF);

                    var acao = pactoViewModel.Acao;

                    pactoViewModel = _Pactoservice.Adicionar(pactoViewModel, user.IsDirigente, user);

                    if (pactoViewModel.ValidationResult.IsValid && pactoViewModel.IdPacto != 0)
                    {
                        pactoBuscado = _Pactoservice.BuscarPorId(pactoViewModel.IdPacto);

                        String operEmail = perfil != null && perfil.Value == Domain.Enums.Perfil.Solicitante ?
                            Domain.Enums.Operacao.Inclusão.ToString() :
                            Domain.Enums.Operacao.Inclusão_Pela_Chefia.ToString();

                        // Quando cadastrado por solicitante: Notificar a chefia pelo cadastro de pacto de subordinado que requer conferência e autorização, com cópia ao próprio solicitante.
                        // Quando cadastrado por chefe: notifica quem cadastrou e pra quem cadastrou. 
                        if (!_notificadorAppService.TratarNotificacaoPacto(pactoBuscado, user, operEmail))
                        {
                            pactoViewModel.ValidationResult.Message = "Plano de trabalho incluído com sucesso, mas não foi possível enviar e-mail para um ou mais interessados.";
                        }
                    }
                    if (pactoViewModel.ValidationResult.IsValid && pactoViewModel.IdPacto != 0 && acao == "Assinando")
                    {
                        pactoViewModel.Acao = "Assinando";
                        pactoViewModel = _Pactoservice.Atualizar(pactoViewModel, user, eAcaoPacto.Assinando);
                    }

                }
                else
                {
                    // Alteracao
                    pactoViewModel.Produtos.ForEach(x => x.IdPacto = pactoViewModel.IdPacto);
                    pactoViewModel.Cronogramas.ForEach(x => x.IdPacto = pactoViewModel.IdPacto);

                    var acao = pactoViewModel.Acao;

                    if (acao == "Assinando")
                    {
                        pactoViewModel = _Pactoservice.Atualizar(pactoViewModel, user, eAcaoPacto.Assinando);
                    }
                    else
                    {
                        pactoViewModel = _Pactoservice.Atualizar(pactoViewModel, user, eAcaoPacto.Editando);
                    }

                    if (pactoViewModel.ValidationResult.IsValid)
                    {
                        int idSituacaoAnterior = pactoBuscado.IdSituacaoPacto;
                        pactoBuscado = _Pactoservice.BuscarPorId(pactoViewModel.IdPacto);
                        pactoBuscado.IdSituacaoPactoAnteriorAcao = idSituacaoAnterior;

                        if (acao == "Assinando")
                        {
                            //Notificar o solicitante da assinatura do pacto pela chefia.
                            if (user.IsDirigente)
                            {
                                if (!_notificadorAppService.TratarNotificacaoPacto(pactoBuscado, user, Domain.Enums.Operacao.Assinatura.ToString()))
                                {
                                    pactoViewModel.ValidationResult.Message = "Plano de Trabalho alterado com sucesso, mas não foi possível enviar e-mail para um ou mais interessados.";
                                }
                            }

                        }
                        else // "Salvando"
                        {
                            //Notificar a chefia pelo cadastro de pacto de subordinado que requer conferência e autorização, com cópia ao próprio solicitante.
                            if (!_notificadorAppService.TratarNotificacaoPacto(pactoBuscado, user, Domain.Enums.Operacao.Alteração.ToString()))
                            {
                                pactoViewModel.ValidationResult.Message = "Plano de Trabalho alterado com sucesso, mas não foi possível enviar e-mail para um ou mais interessados.";
                            }
                        }
                    }
                }

                if (!pactoViewModel.ValidationResult.IsValid)
                {
                    setModelErrorList(pactoViewModel.ValidationResult.Erros);
                }
                else
                {
                    TempData[GetNomeVariavelTempData("Cronogramas", pactoViewModel.IdPacto)] = null;
                }
            }

            pactoViewModel = ConfiguraPactoViewModelAposSolicitacao(pactoViewModel, user);

            return pactoViewModel;
        }


        private PactoViewModel ConfiguraPactoViewModelAposSolicitacao(PactoViewModel pactoViewModel, UsuarioViewModel user)
        {
            pactoViewModel = _Pactoservice.OrdemVigenteProdutos(OrdemServico, pactoViewModel);
            bool isDirigente = user.IsDirigente;
            podePermissoes(pactoViewModel, user, isDirigente);
            ConfigurarGruposAtividades(OrdemServico, pactoViewModel.IdTipoPacto);
            ConfigurarNomesServidoresUnidadesSolicitacao(isDirigente, pactoViewModel.CpfUsuario, pactoViewModel.IdPacto, pactoViewModel.IdTipoPacto);
            pactoViewModel.PossuiCargaHoraria = pactoViewModel.CargaHorariaDiaria != TimeSpan.FromHours(8);
            ConfigurarIniciativasPlanoOperacional();
            pactoViewModel.TipoPacto = _tipoPactoAppService.ObterTodos().SingleOrDefault(t => t.IdTipoPacto == pactoViewModel.IdTipoPacto);

            Unidade_TipoPactoViewModel unidade_tipoPacto = _unidade_tipoPactoAppService.BuscarPorIdUnidadeTipoPacto(pactoViewModel.UnidadeExercicio, pactoViewModel.IdTipoPacto);

            if (unidade_tipoPacto != null)
            {
                pactoViewModel.UnidadeUsuarioPermitePactoExecucaoNoExterior = _unidade_tipoPactoAppService.BuscarPorIdUnidadeTipoPacto(pactoViewModel.UnidadeExercicio, pactoViewModel.IdTipoPacto).IndPermitePactoExterior;
            }
            else
            {
                pactoViewModel.UnidadeUsuarioPermitePactoExecucaoNoExterior = pactoViewModel.PactoExecutadoNoExterior;
            }

            return pactoViewModel;
        }

        private void ConfigurarCargaHorariaFormatada(PactoViewModel pactoViewModel)
        {
            pactoViewModel.Produtos.ForEach(p => ConfigurarCargaHorariaFormatada(p));
        }

        private void ConfigurarOrigemAcao(PactoViewModel pactoViewModel)
        {
            pactoViewModel.Avaliacoes.ForEach(p => p.IdOrigemAcao = pactoViewModel.IdOrigemAcao);
        }

        private void ConfigurarTotalAvaliado(PactoViewModel pactoViewModel)
        {
            pactoViewModel.Produtos.ForEach(p => ConfigurarTotalAvaliado(p));
        }

        private void ConfigurarHistoricoAvaliacoes(List<AvaliacaoProdutoViewModel> avaliacaoProdutoViewModels)
        {
            avaliacaoProdutoViewModels.ForEach(p => ConfigurarHistoricoAvaliacoes(p));
        }

        private void ConfigurarListaProdutos(PactoViewModel pactoViewModel, OrdemServicoViewModel ordemServico)
        {
            List<ProdutoViewModel> listPdrt = (List<ProdutoViewModel>)TempData[GetNomeVariavelTempData("Produtos", pactoViewModel.IdPacto)];
            TempData[GetNomeVariavelTempData("Produtos", pactoViewModel.IdPacto)] = listPdrt;
            pactoViewModel.Produtos = listPdrt;
        }

        private List<UsuarioViewModel> ListarNomesPorUnidade()
        {
            var user = getUserLogado();
            var listSemSolicitante = new List<UsuarioViewModel>();

            var listUser = _usuarioAppService.ObterTodos(user.Unidade, true).Where(x => x.CPF != user.CPF).ToList();
            foreach (var item in listUser)
            {
                //var perfil = _Usuarioservice.ObterPerfis(item);
                var perfil = new List<Domain.Enums.Perfil>();

                if (!perfil.Contains(Domain.Enums.Perfil.Consulta))
                {
                    listSemSolicitante.Add(item);
                }
            }

            return listSemSolicitante;
        }

        public JsonResult GetAtividades(string id, int? idOrdemServico)
        {
            var atividades = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(id))
            {
                OrdemServico = idOrdemServico.HasValue && idOrdemServico.Value > 0 ? _ordemServicoService.ObterPorId(idOrdemServico.Value) : _ordemServicoService.GetOrdemVigente();
                var grupo = OrdemServico.Grupos.First(x => x.IdGrupoAtividade == int.Parse(id));
                grupo.Atividades.OrderBy(x => x.NomAtividade).ToList().ForEach(x => atividades.Add(new SelectListItem { Text = x.NomAtividade, Value = x.IdAtividade.ToString() }));
                if (atividades.Count > 1)
                    atividades.Insert(0, new SelectListItem { Text = "-- Selecione --", Value = "" });
            }
  
            return Json(atividades);
        }
        public JsonResult GetFaixas(string idGrupo, string idAtividade, string idPacto, int idProduto, int? idOrdemServico)
        {
            var faixas = new List<SelectListItem>();

            if (!string.IsNullOrEmpty(idGrupo) && !string.IsNullOrEmpty(idAtividade))
            {
                OrdemServico = !idOrdemServico.HasValue || idOrdemServico == 0 ? _ordemServicoService.GetOrdemVigente() : _ordemServicoService.ObterPorId(idOrdemServico.Value);
                var grupo = OrdemServico.Grupos.FirstOrDefault(x => x.IdGrupoAtividade == int.Parse(idGrupo));
                var atividade = grupo.Atividades.FirstOrDefault(x => x.IdAtividade == int.Parse(idAtividade));
                var valorMinimoFaixa = GetValorMinimoFaixa(idGrupo, idAtividade, idPacto, idProduto);
                // Cade - alterado para permitir diminuir valor do Nível de complexidade
                // atividade.Tipos.Where(t => Convert.ToDouble(t.DuracaoFaixa) >= valorMinimoFaixa).OrderBy(x => x.Faixa).ToList().ForEach(x => faixas.Add(new SelectListItem { Text = x.FaixaTextoExplicativo, Value = x.IdTipoAtividade.ToString() }));
                atividade.Tipos.Where(t => Convert.ToDouble(t.DuracaoFaixa) >= 0).OrderBy(x => x.Faixa).ToList().ForEach(x => faixas.Add(new SelectListItem { Text = x.FaixaTextoExplicativo, Value = x.IdTipoAtividade.ToString() }));
                if (faixas.Count > 1)
                    faixas.Insert(0, new SelectListItem { Text = "-- Selecione --", Value = "" });

            }
            return Json(faixas);
        }

        private double GetValorMinimoFaixa(string idGrupo, string idAtividade, string idPacto, int idProduto)
        {
            double duracaoMinima = 0;

            if (!string.IsNullOrWhiteSpace(idPacto) && idProduto > 0)
            {
                int intIdPacto = int.Parse(idPacto);
                if (intIdPacto > 0)
                {
                    var pacto = _Pactoservice.ObterPorId(intIdPacto);

                    if (getUserLogado().CpfUsuario != pacto.CpfUsuario && getUserLogado().IsDirigente)
                    {
                        var produtosExistentes =
                            pacto.Produtos.Where(p => p.IdGrupoAtividade == int.Parse(idGrupo) && p.IdAtividade == int.Parse(idAtividade));

                        if (produtosExistentes.Count() > 0)
                        {
                            duracaoMinima = produtosExistentes.Select(p => Convert.ToDouble(p.TipoAtividade.DuracaoFaixa)).Max();
                        }
                    }
                }
            }
            return duracaoMinima;
        }

        public JsonResult GetFaixaValor(string idGrupo, string idAtividade, string idFaixa, int? idOrdemServico)
        {
            var resultado = "0";
            if (!string.IsNullOrEmpty(idGrupo) && !string.IsNullOrEmpty(idAtividade) && !string.IsNullOrEmpty(idFaixa))
            {
                OrdemServico = !idOrdemServico.HasValue || idOrdemServico == 0 ? _ordemServicoService.GetOrdemVigente() : _ordemServicoService.ObterPorId(idOrdemServico.Value);
                var grupo = OrdemServico.Grupos.FirstOrDefault(x => x.IdGrupoAtividade == int.Parse(idGrupo));
                var atividade = grupo.Atividades.FirstOrDefault(x => x.IdAtividade == int.Parse(idAtividade));
                var faixa = atividade.Tipos.FirstOrDefault(x => x.IdTipoAtividade == int.Parse(idFaixa));
                if (faixa != null)
                    resultado = faixa.DuracaoFaixa;
            }
            return Json(resultado);
        }

        [HttpPost]
        public ActionResult Interromper(PactoViewModel pactoVM)
        {
            var user = getUserLogado();
            ViewBag.isDirigente = user.IsDirigente;
            ConfigurarJustificativas();
            var pacto = _Pactoservice.BuscarPorId(pactoVM.IdPacto);
            var resultadoValidacao = _Pactoservice.ValidarDataHoraSuspensaoInterrupcao(pacto, pactoVM.DataInterrupcao.GetValueOrDefault(), pactoVM.HorasMantidasParaDataInterrupcao, Domain.Enums.Operacao.Interrupção);

            pacto.Motivo = pactoVM.Motivo;
            pacto.HorasMantidasParaDataInterrupcao = pactoVM.HorasMantidasParaDataInterrupcao;
            pacto.DataInterrupcao = pactoVM.DataInterrupcao;

            if (resultadoValidacao != null)
            {
                ModelState.Clear();
                ModelState.AddModelError("validacaoInterrupcao", resultadoValidacao.ErrorMessage);
                return View(pacto);
            }
            else
            {
                List<CronogramaViewModel> diasRemovidos;
                if (pactoVM.HorasMantidasParaDataInterrupcao > TimeSpan.FromHours(0))
                {
                    diasRemovidos = pacto.Cronogramas.Where(c => c.DataCronograma > pactoVM.DataInterrupcao).ToList();
                    var diaInterrupcao = pacto.Cronogramas.First(c => c.DataCronograma == pactoVM.DataInterrupcao);
                    diaInterrupcao.HorasCronograma = pactoVM.HorasMantidasParaDataInterrupcao;
                }
                else
                {
                    diasRemovidos = pacto.Cronogramas.Where(c => c.DataCronograma >= pactoVM.DataInterrupcao).ToList();
                }

                diasRemovidos.ForEach(d => pacto.Cronogramas.Remove(d));

                var pactoResultado = _Pactoservice.AtualizarStatus(pacto, user, eAcaoPacto.Interrompendo, false);

                var oper = Domain.Enums.Operacao.Interrupção.ToString();

                if (pactoResultado.ValidationResult != null && pactoResultado.ValidationResult.IsValid && !pactoResultado.ValidationResult.Erros.Any())
                {
                    PactoViewModel pactoBuscado = _Pactoservice.BuscarPorId(pactoResultado.IdPacto);
                    //Destinatários da mensagem: usuário criador do pacto

                    if (!_notificadorAppService.TratarNotificacaoPacto(pactoBuscado, user, oper))
                    {

                        return setMessageAndRedirect("Plano de trabalho interrompido com sucesso, mas não foi possível enviar e-mail para um ou mais interessados.", "Index");
                    }
                    else
                    {
                        return setMessageAndRedirect("Interrupção realizada com sucesso", "Index");
                    }
                }
                else
                {
                    ModelState.Clear();
                    setModelError(pactoResultado.ValidationResult);
                    return View(pacto);
                }

            }

        }

        
        public ActionResult AvaliarProduto(int idPacto, int idOrigemAcao)
        {
            var pactoVM = _Pactoservice.BuscarPorId(idPacto);

            ConfigurarJustificativas();
            ConfigurarNiveisAvaliacao();
            ConfigurarCargaHorariaFormatada(pactoVM);
            ConfigurarTotalAvaliado(pactoVM);
            ConfigurarHistoricoAvaliacoes(pactoVM.Avaliacoes);

            pactoVM.IdOrigemAcao = idOrigemAcao;
            ConfigurarOrigemAcao(pactoVM);

            var user = getUserLogado();
            pactoVM.modoSomenteLeitura = !user.IsDirigente;

            return View("AvaliarProduto", pactoVM);
        }

        [HttpPost]
        public ActionResult AvaliarProduto(AvaliacaoProdutoViewModel apvm)
        {
            var user = getUserLogado();
            ViewBag.isDirigente = user.IsDirigente;

            var pactoVM = _Pactoservice.BuscarPorId(apvm.IdPacto);
            var produtoVM = pactoVM.Produtos.FirstOrDefault(p => p.IdProduto == apvm.IdProduto);

            if (produtoVM.QuantidadeProdutosAAvaliar <= 0)
            {
                ModelState.Clear();
                ValidationResult resultado = new ValidationResult();
                resultado.Add(new ValidationError("Produto já avaliado. Verifique o histórico de avaliações."));
                setModelError(resultado);
                return RetornarErrosModelState();
            }

            apvm.CPFAvaliador = user.CPF;
            apvm.DataAvaliacao = DateTime.Now;

            TratarAvaliacaoDetalhada(apvm);

            produtoVM.Avaliacoes.Add(apvm);

            eAcaoPacto tipoAcao = ObterTipoAcaoAvaliacao(pactoVM);

            eTipoAvaliacao tipoAvaliacao = ObterTipoAvaliacao(apvm, pactoVM);
            apvm.TipoAvaliacao = (int)tipoAvaliacao;

            Domain.Enums.Operacao operacao = (tipoAcao == eAcaoPacto.Avaliando) ? Domain.Enums.Operacao.Avaliacao : Domain.Enums.Operacao.AvaliacaoParcial;

            var pactoResultado = _Pactoservice.AtualizarStatus(pactoVM, user, tipoAcao, false);

            if (!pactoResultado.ValidationResult.IsValid)
            {
                ModelState.Clear();
                setModelError(pactoResultado.ValidationResult);
                return RetornarErrosModelState();
            }

            //Notificar a o solicitante da avaliação do pacto pela chefia
            if (!_notificadorAppService.TratarNotificacaoPacto(pactoVM, user, operacao.ToString(), apvm))
            {
                if (tipoAcao == eAcaoPacto.Avaliando)
                {
                    setMessage("Plano de trabalho avaliado com sucesso, mas não foi possível enviar e-mail para um ou mais interessados.");
                }
                else
                {
                    setMessage("Plano de trabalho avaliado parcialmente com sucesso, mas não foi possível enviar e-mail para um ou mais interessados.");
                }

            }
            else
            {
                if (tipoAcao == eAcaoPacto.AvaliandoParcialmente)
                {
                    setMessage("Avaliação parcial realizada com sucesso");
                }
                else
                {
                    setMessage("Avaliação realizada com sucesso");
                }


            }
            ConfigurarQuantidadeProdutos(apvm);
            ConfigurarJustificativas();
            return PartialView("_AvaliarProdutoFormPartial", apvm);
        }

        private void TratarAvaliacaoDetalhada(AvaliacaoProdutoViewModel apvm)
        {
            List<ItemAvaliadoViewModel> lstItensAvaliados = new List<ItemAvaliadoViewModel>();

            bool selecionouTodosOsItens = true;

            if (apvm.IdNivelAvaliacao == (int)eNivelAvaliacao.Detalhada)
            {
                foreach (string itemAvaliado in apvm.ItensAvaliados)
                {
                    string[] arritemAvaliado = itemAvaliado.ToString().Split('-');

                    try
                    {
                        ItemAvaliadoViewModel itemAvaliadoViewModel = new ItemAvaliadoViewModel()
                        {
                            IdCriterioAvaliacao = Convert.ToInt32(arritemAvaliado[0]),
                            IdItemAvaliacao = Convert.ToInt32(arritemAvaliado[1]),
                        };

                        lstItensAvaliados.Add(itemAvaliadoViewModel);
                    }
                    catch
                    {
                        ItemAvaliadoViewModel itemAvaliadoViewModel = new ItemAvaliadoViewModel()
                        {
                            IdCriterioAvaliacao = 0,
                            IdItemAvaliacao = 0,
                        };

                        lstItensAvaliados.Add(itemAvaliadoViewModel);
                        selecionouTodosOsItens = false;
                    }

                }

                apvm.AvaliacoesDetalhadas = new List<AvaliacaoDetalhadaProdutoViewModel>();

                lstItensAvaliados.ForEach(i =>
                {
                    AvaliacaoDetalhadaProdutoViewModel avaliacaoDetalhada = new AvaliacaoDetalhadaProdutoViewModel()
                    {
                        AvaliacaoProduto = apvm,
                        IdOS_ItemAvaliacao = i.IdItemAvaliacao,
                        IdOS_CriterioAvaliacao = i.IdCriterioAvaliacao
                    };

                    apvm.AvaliacoesDetalhadas.Add(avaliacaoDetalhada);
                });

                if (selecionouTodosOsItens)
                {
                    NotaAvaliacaoViewModel notaAvaliacaoViewModel = _avaliacaoProdutoAppService.CalcularNotaAvaliacaoDetalhada(lstItensAvaliados);
                    apvm.Avaliacao = _notaAvaliacaoAppService.ObterPorId(notaAvaliacaoViewModel.IdNotaAvaliacao).Conceito;
                    apvm.NotaFinalAvaliacaoDetalhada = notaAvaliacaoViewModel.ValorNotaFinal;
                    apvm.Avaliacao = _avaliacaoProdutoAppService.RetornaQualidadeAvaliacaoDetalhada(notaAvaliacaoViewModel);
                }
            }

        }

        private eAcaoPacto ObterTipoAcaoAvaliacao(PactoViewModel pactoVM)
        {
            eAcaoPacto tpAcao;
            int totalProdutosPacto = pactoVM.Produtos.Sum(p => p.QuantidadeProduto);
            int totalProdutosAvaliadosAnteriormente = pactoVM.Avaliacoes.Sum(a => a.QuantidadeProdutosAvaliados);
            if (totalProdutosPacto == totalProdutosAvaliadosAnteriormente)
            {
                tpAcao = eAcaoPacto.Avaliando;
            }
            else
            {
                tpAcao = eAcaoPacto.AvaliandoParcialmente;
            }

            return tpAcao;
        }

        private eTipoAvaliacao ObterTipoAvaliacao(AvaliacaoProdutoViewModel apvm, PactoViewModel pactoVM)
        {
            eTipoAvaliacao tpAvaliacao;

            if (pactoVM.Avaliacoes.Any(a => a.IdAvaliacaoProduto != 0))
            {
                return eTipoAvaliacao.Parcial;
            }
            else
            {
                int totalProdutosPacto = pactoVM.Produtos.Sum(p => p.QuantidadeProduto);

                if (totalProdutosPacto == apvm.QuantidadeProdutosAvaliados)
                {
                    tpAvaliacao = eTipoAvaliacao.Total;
                }
                else
                {
                    tpAvaliacao = eTipoAvaliacao.Parcial;
                }

                return tpAvaliacao;
            }
        }

        public ActionResult Interromper(int idPacto)
        {
            var pacto = _Pactoservice.BuscarPorId(idPacto);
            TempData[GetNomeVariavelTempData("Produtos", idPacto)] = pacto.Produtos;
            ConfigurarJustificativas();
            return View("Interromper", pacto);
        }



        [HttpPost]
        public ActionResult AddProduto(ProdutoViewModel model)
        {
           
            var lista = TempData[GetNomeVariavelTempData("Produtos", model.IdPacto)] as List<ProdutoViewModel> ?? new List<ProdutoViewModel>();


            if (ModelState.IsValid)
            {
                ConfigurarCargaHorariaFormatada(model);
                AtualizaOrdemServico(model.IdOrdemServico);
                ConfigurarSituacaoProduto();

                AtualizarReferenciasProduto(model, OrdemServico);
                if (model.Index == 0)
                {
                    //configura o index para permitir referenciá-lo posteriormente. Não foi utilizado o Id do Produto pq na inclusão o valor sempre é 0.
                    model.Index = lista.Select(p => p.Index).DefaultIfEmpty(0).Max() + 1;
                    lista.Add(model);
                }
                else
                {
                    //alteração
                    int indexLista = lista.IndexOf(lista.First(p => p.Index == model.Index));
                    lista[indexLista] = model;
                }

                TempData[GetNomeVariavelTempData("Produtos", model.IdPacto)] = lista;
            }
            else
            {
                TempData[GetNomeVariavelTempData("Produtos", model.IdPacto)] = lista;
                return RetornarErrosModelState();
            }
            return PartialView("_ProdutosTablePartial", lista);
        }

        private void AtualizarReferenciasProduto(ProdutoViewModel model, OrdemServicoViewModel ordemVigente)
        {
            var grupo = ordemVigente.Grupos.FirstOrDefault(x => x.IdGrupoAtividade == model.IdGrupoAtividade);
            var atividade = grupo.Atividades.FirstOrDefault(x => x.IdAtividade == model.IdAtividade);
            var faixa = atividade.Tipos.FirstOrDefault(x => x.IdTipoAtividade == model.IdTipoAtividade);
            var qtdFaixa = atividade.Tipos.Count();
            var situacaoProduto = model.IdSituacaoProduto;

            model.Atividade = atividade;
            model.GrupoAtividade = grupo;
            model.QtdFaixas = qtdFaixa;
            model.TipoAtividade = faixa;
            model.IdSituacaoProduto = situacaoProduto;

            var iniciativas = ConfigurarIniciativasPlanoOperacional();
            model.IniciativasPlanoOperacionalProduto = new List<IniciativaPlanoOperacionalProdutoViewModel>();
            model.IniciativasPlanoOperacionalSelecionadas.ForEach(i =>
            {
                model.IniciativasPlanoOperacionalProduto.Add(new IniciativaPlanoOperacionalProdutoViewModel()
                {
                    Produto = model,
                    IdIniciativaPlanoOperacional = i
                });
            });
        }

        public PartialViewResult PreparaAlteracaoProduto(int indexProduto, int idPacto, int idTipoPacto)
        {
            List<ProdutoViewModel> lista = (List<ProdutoViewModel>)TempData[GetNomeVariavelTempData("Produtos", idPacto)];

            ProdutoViewModel pvm = lista.FirstOrDefault(m => m.Index == indexProduto);

            pvm.IniciativasPlanoOperacionalProduto.ForEach(i =>
            {
                pvm.IniciativasPlanoOperacionalSelecionadas.Add(i.IdIniciativaPlanoOperacional);
            });

            ConfigurarGruposAtividades(OrdemServico, idTipoPacto);
            ConfigurarIniciativasPlanoOperacional();
            TempData[GetNomeVariavelTempData("Produtos", idPacto)] = lista;

            return PartialView("_ProdutosFormPartial", pvm);
        }

        public PartialViewResult PreparaAlteracaoObservacaoProduto(int indexProduto, int idPacto)
        {
            List<ProdutoViewModel> lista = (List<ProdutoViewModel>)TempData[GetNomeVariavelTempData("Produtos", idPacto)];
            ProdutoViewModel pvm = lista.FirstOrDefault(m => m.Index == indexProduto);
            TempData[GetNomeVariavelTempData("Produtos", idPacto)] = lista;
            return PartialView("_ProdutosObservacaoPartial", pvm);
        }


        public ActionResult ExcluiProduto(int indexProduto, int idPacto, int idTipoPacto)
        {
            List<ProdutoViewModel> lista =
                (List<ProdutoViewModel>)TempData[GetNomeVariavelTempData("Produtos", idPacto)];
            ProdutoViewModel pvm = lista.FirstOrDefault(m => m.Index == indexProduto);
            ConfigurarGruposAtividades(OrdemServico, idTipoPacto);
            ConfigurarSituacaoProduto();
            if (pvm != null) lista.Remove(pvm);
            TempData[GetNomeVariavelTempData("Produtos", idPacto)] = lista;
            return pvm != null
                ? Json(new
                {
                    html = RenderPartialViewToString("~/Views/Pacto/_ProdutosTablePartial.cshtml", lista),
                    hrsRemovidas = pvm.CargaHorariaProduto * pvm.QuantidadeProduto
                }, JsonRequestBehavior.AllowGet)
                : Json(new { html = RenderPartialViewToString("~/Views/Pacto/_ProdutosTablePartial.cshtml", lista), hrsRemovidas = 0 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCargaHorariaTotal(int idPacto)
        {
            List<ProdutoViewModel> lista = (List<ProdutoViewModel>)TempData[GetNomeVariavelTempData("Produtos", idPacto)] ?? new List<ProdutoViewModel>();
            var cargaTotal = lista.Sum(p => p.CargaHorariaProduto * p.QuantidadeProduto);
            TempData[GetNomeVariavelTempData("Produtos", idPacto)] = lista;
            return Json(new { cargaHorariaTotal = cargaTotal });
        }

        [HttpPost]
        public JsonResult GetPermiteExecucaoExterior(int idUnidade, int idTipoPacto)
        {
            var unidade_tipoPacto = _unidade_tipoPactoAppService.BuscarPorIdUnidadeTipoPacto(idUnidade, idTipoPacto);
            return Json(new { permiteExecucaoExterior = unidade_tipoPacto.IndPermitePactoExterior ? 1 : 0 });
        }


        public JsonResult AtualizaDataTerminoECronograma(string datainicio, TimeSpan cargahoraria, string cargahorariapacto,
            string cpfusuario, int idpacto = 0)
        {
            var tempCronogramas = (CronogramaPactoViewModel)TempData[GetNomeVariavelTempData("Cronogramas", idpacto)];
            var dataInicioInformada = DateTime.Parse(datainicio).Date;



            double dblCargaHorariaPacto = Convert.ToDouble(cargahorariapacto);

            List<CronogramaViewModel> cronogramas;
            if (tempCronogramas == null || tempCronogramas.Cronogramas?.Count == 0
                || tempCronogramas.DataInicial.Date != dataInicioInformada
                || tempCronogramas.HorasTotais != dblCargaHorariaPacto
                || tempCronogramas.HorasDiarias != cargahoraria)
            {
                cronogramas = _cronogramaService.CalcularCronogramas(dblCargaHorariaPacto, cargahoraria, DateTime.Parse(datainicio),
                    CPFUsuario: cpfusuario, usuarioLogado: getUserLogado(),
                    idPacto: idpacto,
                    cronogramaExistente: tempCronogramas);

                if (tempCronogramas == null)
                {
                    tempCronogramas = new CronogramaPactoViewModel();
                }


                tempCronogramas.HorasTotais = dblCargaHorariaPacto;
                tempCronogramas.HorasDiarias = cargahoraria;
                tempCronogramas.Cronogramas = cronogramas;
            }
            else
            {
                cronogramas = tempCronogramas.Cronogramas;
            }
            TempData[GetNomeVariavelTempData("Cronogramas", idpacto)] = tempCronogramas;

            var dataFinalCalculada = (cronogramas.Count > 0) ? cronogramas.Max(c => c.DataCronograma) : dataInicioInformada;

            var existemPactosConcorrentes = _Pactoservice.GetPactosConcorrentes(dataInicio: dataInicioInformada, dataFinal: dataFinalCalculada,
                cpfUsuario: cpfusuario, idPacto: idpacto).Count > 0;

            return Json(new { dataTermino = dataFinalCalculada.ToString("dd/MM/yyyy"), existemPactosConcorrentes = existemPactosConcorrentes.ToString().ToLower() });
        }



        public PartialViewResult IniciarSuspensao(int idPactoInicioSuspensao)
        {
            var svm = InicializarSuspenderPactoViewModel(idPactoInicioSuspensao, false);
            return PartialView("_SuspensaoPartial", svm);
        }

        public ActionResult Suspender(SuspenderPactoViewModel suspenderVM)
        {


            var user = this.getUserLogado();

            var pacto = _Pactoservice.BuscarPorId(suspenderVM.IdPacto);

            var retornoValidacao = _Pactoservice.ValidarDataHoraSuspensaoInterrupcao(pacto, suspenderVM.SuspensoAPartirDe.GetValueOrDefault(), suspenderVM.HorasMantidasParaDataSuspensao, Domain.Enums.Operacao.Suspensão);
            ConfigurarJustificativas();
            if (retornoValidacao == null)
            {

                pacto.SuspensoAPartirDe = suspenderVM.SuspensoAPartirDe;
                pacto.SuspensoAte = suspenderVM.SuspensoAte;
                pacto.Motivo = suspenderVM.Motivo;

                var cronogramas = (CronogramaPactoViewModel)TempData[GetNomeVariavelTempData("Cronogramas", suspenderVM.IdPacto)];
                pacto.Cronogramas = cronogramas.Cronogramas;

                pacto.DataPrevistaTermino = suspenderVM.SuspensoAte.HasValue && suspenderVM.SuspensoAte != DateTime.MinValue ?
                    pacto.Cronogramas?.LastOrDefault()?.DataCronograma ?? pacto.DataPrevistaTermino : pacto.DataPrevistaTermino;

                var pactoResultado = _Pactoservice.AtualizarStatus(pacto, user, eAcaoPacto.Suspendendo, false);

                var oper = Domain.Enums.Operacao.Suspensão.ToString();

                if (pactoResultado.ValidationResult.IsValid)
                {
                    PactoViewModel pactoBuscado = _Pactoservice.BuscarPorId(pactoResultado.IdPacto);
                    //Notificar a chefia pelo cadastro de pacto de subordinado que requer conferência e autorização, com cópia ao próprio solicitante.
                    if (!_notificadorAppService.TratarNotificacaoPacto(pactoBuscado, user, oper))
                    {
                        var mensagem = "Plano de trabalho suspenso com sucesso, mas não foi possível enviar e-mail para um ou mais interessados.";
                        setMessage(mensagem);
                    }
                    else
                        setMessage(pactoResultado.ValidationResult.Message);
                }
                else
                {
                    suspenderVM.ClasseMensagem = "alert-danger";

                    if (pactoResultado.ValidationResult.Erros.Any())
                    {
                        suspenderVM.Mensagem = pactoResultado.ValidationResult.Erros.FirstOrDefault()?.Message;
                    }
                    else
                    {
                        suspenderVM.Mensagem = pactoResultado.ValidationResult.Message;
                    }
                    ModelState.AddModelError("", suspenderVM.Mensagem);
                    InicializarSuspenderPactoViewModelDadosBasicos(suspenderVM, pacto);
                }

                TempData[GetNomeVariavelTempData("Cronogramas", suspenderVM.IdPacto)] = cronogramas;
            }
            else
            {
                InicializarSuspenderPactoViewModelDadosBasicos(suspenderVM, pacto);
                suspenderVM.ClasseMensagem = "alert-danger";
                suspenderVM.Mensagem = retornoValidacao.ErrorMessage;
                ModelState.AddModelError("", retornoValidacao.ErrorMessage);
            }

            return PartialView("_SuspensaoPartial", suspenderVM);

        }

        [HttpPost]
        public JsonResult FinalizarSuspensao(SuspenderPactoViewModel suspensaoVM)
        {
            var user = getUserLogado();
            ViewBag.isDirigente = user.IsDirigente;
            int idPacto = suspensaoVM.IdPacto;
            var pacto = _Pactoservice.BuscarPorId(idPacto);

            DateTime? dataPrevistaTerminoAntesSuspensao = pacto.DataPrevistaTermino;

            pacto.SuspensoAte = suspensaoVM.SuspensoAte;
            pacto.Cronogramas = ((CronogramaPactoViewModel)TempData[GetNomeVariavelTempData("Cronogramas", idPacto)]).Cronogramas;
            pacto.DataPrevistaTermino = pacto.Cronogramas.Max(c => c.DataCronograma);

            var retorno = _Pactoservice.AtualizarStatus(pacto, user, eAcaoPacto.VoltandoSuspensao);

            if (retorno.ValidationResult.IsValid)
            {
                PactoViewModel pactoBuscado = _Pactoservice.BuscarPorId(pacto.IdPacto);
                pactoBuscado.DataPrevistaTerminoAntesSuspensao = dataPrevistaTerminoAntesSuspensao;

                //Notificar o solicitante do fim da suspensão do pacto
                if (!_notificadorAppService.TratarNotificacaoPacto(pactoBuscado, user, Domain.Enums.Operacao.VoltandoSuspensão.ToString()))
                {
                    ValidationError ve = new ValidationError("Suspensão do plano de trabalho feita com sucesso, mas não foi possível enviar e-mail para um ou mais interessados.");
                    retorno.ValidationResult.Add(ve);
                    retorno.ValidationResult.Message = "Suspensão do plano de trabalho feita com sucesso, mas não foi possível enviar e-mail para um ou mais interessados.";
                    setMessage(retorno.ValidationResult.Message);
                }
                else
                {

                    if (retorno.ValidationResult.IsValid)
                        setMessage(retorno.ValidationResult.Message);
                }
            }
            else
            {
                setMessage(retorno.ValidationResult);
            }

            return Json(retorno.ValidationResult);

        }

        public JsonResult VoltarSuspensao(int idpacto)
        {
            var user = getUserLogado();
            ViewBag.isDirigente = user.IsDirigente;

            var pacto = _Pactoservice.BuscarPorId(idpacto);
            pacto.SuspensoAte = DateTime.Now;
            var retorno = _Pactoservice.AtualizarStatus(pacto, user, eAcaoPacto.VoltandoSuspensao);

            if (retorno.ValidationResult.IsValid)
            {
                if (retorno.ValidationResult.IsValid)
                    setMessage(retorno.ValidationResult.Message);
            }
            else
            {
                setMessage(retorno.ValidationResult);
            }

            return Json(retorno.ValidationResult);
        }

        public JsonResult VerificaPendencias(string cpfSelecionadoUsr)
        {
            var user = new UsuarioViewModel();
            user.CPF = cpfSelecionadoUsr;
            return Json(_Pactoservice.PossuiPactoPendencias(user));

        }

        public JsonResult GetDadosUsuario(string nomeSelecionado)
        {
            var usuario = _usuarioAppService.ObterPorNome(nomeSelecionado);
            return Json(new { CPF = usuario.CPF, Matricula = usuario.Matricula, UnidadeExercicio = usuario.Unidade, UnidadeDescricao = usuario.nomeUnidade });
        }

        public JsonResult GetDadosUsuarioPorCpf(string cpf)
        {
            cpf = cpf.RemoverMaskCpfCnpj();
            var usuario = _usuarioAppService.ObterPorCPF(cpf);
            return Json(new { usuario.CPF, usuario.Matricula, UnidadeExercicio = usuario.Unidade, UnidadeDescricao = usuario.nomeUnidade, usuario.Nome });
        }

        public JsonResult GetLinkAtividade(int idGrupo, int? idAtividade, int? idOrdemServico)
        {

            if (idAtividade == null)
            {
                return null;
            }
            AtualizaOrdemServico(idOrdemServico ?? 0);
            var grupo = OrdemServico.Grupos.First(x => x.IdGrupoAtividade == idGrupo);
            var atividade = grupo.Atividades.First(a => a.IdAtividade == idAtividade);
            return Json(new { Link = atividade.Link });
        }

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult AtualizaObservacaoProduto(ProdutoViewModel alterarObservacaoProdutoVM)
        {
            var user = getUserLogado();
            ViewBag.isDirigente = user.IsDirigente;
            var produto = _Produtoservice.BuscarPorId(alterarObservacaoProdutoVM.IdPacto, alterarObservacaoProdutoVM.IdProduto);
            produto.Observacoes = alterarObservacaoProdutoVM.Observacoes;
            var result = _Pactoservice.AtualizarObservacaoProduto(produto, user);
            

            return Json(result.ValidationResult);
        }

        [HttpPost]
        public JsonResult AtualizaSituacaoProduto(string idSituacaoProduto, string idPacto, string idProduto)
        {
        
            var user = getUserLogado();
            ViewBag.isDirigente = user.IsDirigente;
            var produto = _Produtoservice.BuscarPorId(int.Parse(idPacto), int.Parse(idProduto));
            if(idSituacaoProduto != "")
            {
                produto.IdSituacaoProduto = int.Parse(idSituacaoProduto);
            }
            else
            {
                produto.IdSituacaoProduto = 0;
            }
           
            var result = _Pactoservice.AtualizarSituacaoProduto(produto, user);
        

            ConfigurarSituacaoProduto();

            return Json("Situação Alterada com Sucesso!");

            //return setMessageAndRedirect("Situação Alterada com Sucesso!", "Index");
        } 

        [HttpPost]
        public JsonResult UploadAnexoProduto([Bind(Include = "IdPacto,IdProduto")] ProdutoViewModel produtoVM)
        {
            var user = getUserLogado();
            ViewBag.isDirigente = user.IsDirigente;
            var produto = _Produtoservice.BuscarPorId(produtoVM.IdPacto, produtoVM.IdProduto);
          
            int arquivosSalvos = 0;
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase arquivo = Request.Files[i];
                //Validações

                //Salva Arquivo
                if(arquivo.ContentLength > 0)
                {
                
                    // Salva referencia no Banco
                    AnexoProduto anexoProduto = new AnexoProduto();
                    anexoProduto.IdProduto = produto.IdProduto;
                    anexoProduto.Nome = arquivo.FileName;
                    anexoProduto.Tamanho = arquivo.ContentLength;
                    anexoProduto.Tipo = Path.GetExtension(arquivo.FileName);
                    anexoProduto = _anexoProdutoService.Adicionar(anexoProduto);
                    
                    if(anexoProduto != null)
                    {
                        var uploadPath = Server.MapPath("~/Content/Uploads");
                        string caminhoArquivo = Path.Combine(uploadPath, Path.GetFileName(anexoProduto.IdProduto.ToString()));
                        arquivo.SaveAs(caminhoArquivo);
                       
                    }
                    arquivosSalvos++;
                }

                
            }
            return Json("Arquivo anexado com sucesso!");
        }




        public PartialViewResult Observacoes(int idPacto, int idProduto)
        {
            var produtoVM = _Produtoservice.BuscarPorId(idPacto, idProduto);
            return PartialView("_ObservacoesPartial", produtoVM);
        }

        //[AuthorizePerfil(Domain.Enums.Perfil.Dirigente, Domain.Enums.Perfil.Solicitante)]
        public PartialViewResult RetornarSuspensao(int idPactoRetornoSuspensao)
        {
            SuspenderPactoViewModel modelCronograma = InicializarSuspenderPactoViewModel(idPactoRetornoSuspensao, true);

            return PartialView("_RetornoSuspensaoPartial", modelCronograma);
        }

        private SuspenderPactoViewModel InicializarSuspenderPactoViewModel(int idPactoSuspensao, bool preencherReinicioPacto = false, DateTime? dataInicioSuspensao = null)
        {
            SuspenderPactoViewModel modelCronograma = new SuspenderPactoViewModel();
            var pacto = _Pactoservice.BuscarPorId(idPactoSuspensao);
            InicializarSuspenderPactoViewModelDadosBasicos(modelCronograma, pacto);
            modelCronograma.SuspensoAPartirDe = pacto.SuspensoAPartirDe.HasValue ? pacto.SuspensoAPartirDe.Value : DateTime.Now.Date;


            TimeSpan quantidadeHorasDiaSuspensao = pacto.Cronogramas.Where(c => c.DataCronograma == pacto.SuspensoAPartirDe).FirstOrDefault()?.HorasCronograma ?? TimeSpan.Zero;

            var cronogramaPactoReferencia = new CronogramaPactoViewModel()
            {
                DataInicial = pacto.DataPrevistaInicio,
                Cronogramas = pacto.Cronogramas,
                HorasTotais = Convert.ToDouble(pacto.CargaHorariaTotal),
                HorasDiarias = pacto.CargaHorariaDiaria,
                DataInicioSuspensao = pacto.SuspensoAPartirDe,
                DataFimSuspensao = DateTime.Today,
                IdPacto = pacto.IdPacto,
                CPFUsuario = pacto.CpfUsuario
            };

            if (preencherReinicioPacto)
            {
                modelCronograma.SuspensoAte = DateTime.Today;
                var cronogramaCalculado = _cronogramaService.CalcularInclusaoSuspensao(cronogramaPactoReferencia: cronogramaPactoReferencia,
                                                                              dataInicioSuspensao: pacto.SuspensoAPartirDe.Value, cpfSolicitante: pacto.CpfUsuarioSolicitante, usuarioLogado: getUserLogado(),
                                                                              dataFimSuspensao: DateTime.Today, horasConsideradasNaDataSuspensao: quantidadeHorasDiaSuspensao);

                cronogramaPactoReferencia.Cronogramas = cronogramaCalculado;
                modelCronograma.DataTerminoPacto = cronogramaCalculado.Last().DataCronograma;
            }
            else
            {
                pacto.SuspensoAPartirDe = dataInicioSuspensao ?? DateTime.Now.Date;
                var cronogramaCalculado = _cronogramaService.CalcularInclusaoSuspensao(cronogramaPactoReferencia: cronogramaPactoReferencia,
                                                                              dataInicioSuspensao: pacto.SuspensoAPartirDe.GetValueOrDefault(), cpfSolicitante: pacto.CpfUsuarioSolicitante, usuarioLogado: getUserLogado(), horasConsideradasNaDataSuspensao: quantidadeHorasDiaSuspensao);
                cronogramaPactoReferencia.Cronogramas = cronogramaCalculado;
            }

            modelCronograma.PodeEditar = true;

            TempData[GetNomeVariavelTempData("Cronogramas", idPactoSuspensao)] = cronogramaPactoReferencia;
            return modelCronograma;
        }

        private static void InicializarSuspenderPactoViewModelDadosBasicos(SuspenderPactoViewModel modelCronograma, PactoViewModel pacto)
        {
            modelCronograma.IdPacto = pacto.IdPacto;
            modelCronograma.DataInicioPacto = pacto.DataPrevistaInicio;
            modelCronograma.CargaHorariaDiaria = pacto.CargaHorariaDiaria;
            modelCronograma.CargaHorariaTotalPacto = pacto.CargaHorariaTotal;
            modelCronograma.CPFUsuario = pacto.CpfUsuario;
        }

        public PartialViewResult PreparaAvaliacaoProduto(int idPacto, int idProduto, int idAvaliacaoProduto = 0, int idOrigemAcao = 0)
        {
            AvaliacaoProdutoViewModel apvm;
            if (idAvaliacaoProduto > 0)
            {
                apvm = _avaliacaoProdutoAppService.ObterPorId(idAvaliacaoProduto);
                apvm.NomeAvaliador = _usuarioAppService.ObterPorCPF(apvm.CPFAvaliador).Nome;
            }
            else
            {
                apvm = new AvaliacaoProdutoViewModel();
                apvm.Produto = _Produtoservice.BuscarPorId(idPacto, idProduto);
                apvm.LocalizacaoProduto = apvm.Produto.Observacoes;
            }

            apvm.IdPacto = apvm.Produto.IdPacto;
            PactoViewModel pacto = _Pactoservice.BuscarPorId(idPacto);
            apvm.podeTerAvaliacaoDetalhada = _ordemServico_CriterioAvaliacaoAppService.BuscarPorIdOS(pacto.IdOrdemServico).ToList()?.Count > 0;

            var pactoVM = _Pactoservice.BuscarPorId(apvm.IdPacto);
            apvm.CriteriosAvaliacao = _ordemServico_CriterioAvaliacaoAppService.BuscarPorIdOS(pactoVM.IdOrdemServico).ToList();
            ConfigurarItensAvaliacao(apvm);

            if (idOrigemAcao == 0)
            {
                apvm.IdOrigemAcao = (int)PGD.Domain.Enums.eOrigem.SolicitacaoPacto;
            }
            else
            {
                apvm.IdOrigemAcao = idOrigemAcao;
            }

            ConfigurarJustificativas();
            ConfigurarCargaHorariaFormatada(apvm.Produto);
            ConfigurarTotalAvaliado(apvm.Produto);
            ConfigurarQuantidadeProdutos(apvm);

            var user = getUserLogado();
            ConfigurarFormulario(apvm, idAvaliacaoProduto, !user.IsDirigente);

            return PartialView("_AvaliarItensPartial", apvm);
        }

        private static void ConfigurarItensAvaliacao(AvaliacaoProdutoViewModel apvm)
        {
            foreach (OS_CriterioAvaliacaoViewModel criterio in apvm.CriteriosAvaliacao)
            {
                foreach (ItemAvaliacaoViewModel itemAvaliacao in criterio.ItensAvaliacao)
                {
                    itemAvaliacao.IdCriterioAvaliacaoIdItemAvaliacao = criterio.IdCriterioAvaliacao + "-" + itemAvaliacao.IdItemAvaliacao;
                }
            }
        }

        private void ConfigurarFormulario(AvaliacaoProdutoViewModel apvm, int idAvaliacaoProduto, bool modoSomenteLeitura = true)
        {
            apvm.HabilitarCampos = idAvaliacaoProduto == 0 && !modoSomenteLeitura;
            apvm.ModoSomenteLeitura = modoSomenteLeitura;
        }

        private void ConfigurarQuantidadeProdutos(AvaliacaoProdutoViewModel apvm)
        {
            Dictionary<int, int> opcoesQuantidadeProdutos = new Dictionary<int, int>();

            if (apvm.IdAvaliacaoProduto == 0) apvm.QuantidadeProdutosAvaliados = apvm.Produto.QuantidadeProduto - apvm.Produto.QuantidadeProdutoAvaliado;

            int quantidadeProdutosDisponiveis = apvm.QuantidadeProdutosAvaliados;

            if (apvm.IdAvaliacaoProduto > 0)
            {
                quantidadeProdutosDisponiveis = apvm.Produto.QuantidadeProduto;
            }

            for (int i = 1; i <= quantidadeProdutosDisponiveis; i++)
            {
                opcoesQuantidadeProdutos.Add(i, i);
            }
            TempData["QuantidadeProdutosAAvaliar"] = opcoesQuantidadeProdutos;
        }

        public string RetornaCpfCorrigido(string cpf)
        {
            var CpfCorrigido = string.Empty;
            if (!String.IsNullOrEmpty(cpf))
            {
                if (cpf.Length < 11)
                    CpfCorrigido = cpf.PadLeft(11, '0');
                else

                    CpfCorrigido = cpf;
            }
            return CpfCorrigido;
        }

        public static string GetNomeVariavelTempData(string nomeVariavel, int idPacto)
        {
            if (idPacto > 0)
            {
                return nomeVariavel + idPacto;
            }
            else
            {
                return nomeVariavel;
            }
        }

        [HttpPost]
        public JsonResult GetNotaAvaliacaoDetalhada(String itensAvaliados)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            dynamic objItensAvaliados = serializer.DeserializeObject(itensAvaliados);

            List<ItemAvaliadoViewModel> lstItensAvaliados = new List<ItemAvaliadoViewModel>();

            foreach (Dictionary<String, object> itemAvaliado in objItensAvaliados)
            {
                string[] arritemAvaliado = itemAvaliado["IdCriterioIdItem"].ToString().Split('-');

                ItemAvaliadoViewModel itemAvaliadoViewModel = new ItemAvaliadoViewModel()
                {
                    IdCriterioAvaliacao = Convert.ToInt32(arritemAvaliado[0]),
                    IdItemAvaliacao = Convert.ToInt32(arritemAvaliado[1]),
                };

                lstItensAvaliados.Add(itemAvaliadoViewModel);
            }

            NotaAvaliacaoViewModel notaFinal = _avaliacaoProdutoAppService.CalcularNotaAvaliacaoDetalhada(lstItensAvaliados);

            return Json(new { notaFinal = notaFinal.DescNotaAvaliacao });
        }

    }

}